using System.Globalization;
using System.Net;
using System.Text;
using CsvHelper;
using FinancialChat.Worker;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Worker;

public class RabbitMQWorker : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RabbitMQWorker> _logger;
    private readonly RabbitOptions _options;
    private IConnection? _connection;

    public RabbitMQWorker(IHttpClientFactory httpClientFactory,
        ILogger<RabbitMQWorker> logger,
        IOptions<RabbitOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            Connet();
            AddCommandsConsumer();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
            Disconnect();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Worker stopped at: {time}", DateTimeOffset.Now);
        }
    }

    private void Connet()
    {
        if (_connection != null && _connection.IsOpen)
        {
            return;
        }
        _connection = GetConnection();
    }

    private IConnection GetConnection()
    {
        var factory = new ConnectionFactory
        {
            Port = _options.Port,
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password
        };
        return factory.CreateConnection();
    }

    private void Disconnect()
    {
        if (_connection == null)
        {
            return;
        }
        _connection.Close();
    }


    private void AddCommandsConsumer()
    {
        if (_connection == null)
        {
            return;
        }

        var channel = _connection.CreateModel();
        channel.QueueDeclare(_options.CommandsQueue, true, false, false, null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, eventArgs) =>
        {
            if (channel.IsClosed)
            {
                return;
            }

            string queueMessage = string.Empty;
            try
            {
                queueMessage = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                JObject queueObj = JObject.Parse(queueMessage);
                await ProcessCommand(queueObj);
                channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (RuntimeBinderException ex)
            {
                _logger.LogError(ex, $"Error RuntimeBinderException: {queueMessage}.");
                channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al procesar mensaje: {queueMessage}.");
                channel.BasicNack(eventArgs.DeliveryTag, false, true);
                Thread.Sleep(5000);
            }
        };

        channel.BasicConsume(_options.CommandsQueue, false, string.Empty, true, false, null, consumer);
    }

    private void Publish<T>(T message)
    {
        var channel = GetConnection().CreateModel();

        channel.QueueDeclare(_options.BotMessagesQueue, true, false, false);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: _options.BotMessagesQueue, body: body);
    }

    private async Task ProcessCommand(JObject jObject)
    {
        string commandArgument = jObject["CommandArgument"]?.ToString()!;
        string userId = jObject["UserId"]?.ToString()!;
        string chatRoom = jObject["ChatRoomCode"]?.ToString()!;

        string remoteUri = "https://stooq.com/q/l/?s=[stockcode]&f=sd2t2ohlcv&h&e=csv".Replace("[stockcode]", commandArgument?.ToLower());
        var httpClient = _httpClientFactory.CreateClient();
        using (var response = await httpClient.GetAsync(remoteUri, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var anonymousTypeDefinition = new
                {
                    Symbol = string.Empty,
                    Open = string.Empty
                };
                var records = csv.GetRecords(anonymousTypeDefinition);
                var selectedRecord = records.LastOrDefault();
                if (selectedRecord != null && selectedRecord.Open != "N/D")
                {
                    Publish(new
                    {
                        ChatRoomCode = chatRoom,
                        UserId = userId,
                        Message = $"{selectedRecord.Symbol} quote is ${selectedRecord.Open} per share",
                        PublishDate = DateTime.Now
                    });
                }
            }
        }
    }
}

