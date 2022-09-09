using System.Text;
using FinancialChat.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FinancialChat.Infrastructure.MessageBroker;

public class RabbitMQProducer : IMessageBrokerProducer
{
    private readonly RabbitOptions _options;
    public RabbitMQProducer(IOptions<RabbitOptions> options)
    {
        _options = options.Value;
    }

    public void Publish<T>(T message)
    {
        var factory = new ConnectionFactory {
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
}

