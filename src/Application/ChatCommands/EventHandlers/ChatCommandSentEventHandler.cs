using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Application.ChatCommands.EventHandlers;

public class ChatCommandSentEventHandler : INotificationHandler<ChatCommandSentEvent>
{
    private readonly ILogger<ChatCommandSentEventHandler> _logger;
    private readonly IMessageBrokerProducer _publisher;

    public ChatCommandSentEventHandler(ILogger<ChatCommandSentEventHandler> logger, IMessageBrokerProducer publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public Task Handle(ChatCommandSentEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("FinancialChat Domain Event: {DomainEvent}", notification.GetType().Name);

        _publisher.Publish(new
        {
            UserId = notification.ChatCommand?.UserId,
            ChatRoomCode = notification.ChatCommand?.ChatRoom?.Code,
            CommandName = notification.ChatCommand?.Name,
            CommandArgument = notification.ChatCommand?.Param,
            PublishDate = DateTime.Now
        });

        return Task.CompletedTask;
    }
}