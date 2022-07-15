using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FinancialChat.Application.MessageBroker;

public class RabbitMQProducer : IMessageProducer
{
    private readonly RabbitOptions _options;
    public RabbitMQProducer(IOptions<RabbitOptions> options)
    {
        _options = options.Value;
    }

    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VHost
        };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(_options.CommandsQueue, true, false, false);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: _options.CommandsQueue, body: body);
    }
}

