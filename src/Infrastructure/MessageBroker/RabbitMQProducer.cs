using System.Text;
using FinancialChat.Application.ChatCommands.EventHandlers;
using FinancialChat.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FinancialChat.Infrastructure.MessageBroker;

public class RabbitMQProducer : IMessageBrokerProducer
{
    private readonly RabbitOptions _options;
    private readonly ILogger<RabbitMQProducer> _logger;
    public RabbitMQProducer(IOptions<RabbitOptions> options, ILogger<RabbitMQProducer> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public void Publish<T>(T message)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(_options.CommandsQueue, true, false, false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: _options.CommandsQueue, body: body);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Could not publish message to Rabbit MQ. Verify your MQ Server is running and accessible.");
            _logger.LogError("RabbitMQProducer: {DomainEvent}", ex, message.GetType().Name);
            Thread.Sleep(10000);
            Publish(message);
        }
    }
}

