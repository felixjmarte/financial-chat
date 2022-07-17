using System.Text;
using FinancialChat.Application.Common.Exceptions;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Domain.Events;
using MediatR;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialChat.Infrastructure.MessageBroker
{
    public sealed class RabbitMQConsumer : IMessageBrokerConsumer
    {
        private readonly ILogger _logger;
        private readonly RabbitOptions _options;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQConsumer(IServiceScopeFactory serviceScopeFactory,
                                IOptions<RabbitOptions> options,
                                ILogger<RabbitMQConsumer> logger)
        {
            _logger = logger;
            _options = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Consume()
        {
            BotMessagesConsumer();
        }

        private void BotMessagesConsumer()
        {
            var factory = new ConnectionFactory
            {
                Port = _options.Port,
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VHost
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(_options.BotMessagesQueue, true, false, false, null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=  async (model, eventArgs) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                string queueMessage = string.Empty;
                try
                {
                    queueMessage = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    JObject queueObj = JObject.Parse(queueMessage);
                    var botQueueEvent = new BotChatMessageReceivedEvent(queueObj["ChatRoomCode"]?.ToString()!, queueObj["Message"]?.ToString()!);
                    if (!string.IsNullOrEmpty(botQueueEvent.ChatRoomCode) && !string.IsNullOrEmpty(botQueueEvent.Message))
                    {
                        await mediator.Publish(botQueueEvent);
                    }
                    channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (RuntimeBinderException ex)
                {
                    _logger.LogError(ex, $"Error RuntimeBinderException: {queueMessage}.");
                    channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }
                catch (NotFoundException ex)
                {
                    _logger.LogError(ex, $"Error NotFoundException: {queueMessage}.");
                    channel.BasicNack(eventArgs.DeliveryTag, false, false);
                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al procesar mensaje: {queueMessage}.");
                    channel.BasicNack(eventArgs.DeliveryTag, false, true);
                    Thread.Sleep(5000);
                }
            };

            channel.BasicConsume(_options.BotMessagesQueue, false, string.Empty, true, false, null, consumer);
        }
    }
}

