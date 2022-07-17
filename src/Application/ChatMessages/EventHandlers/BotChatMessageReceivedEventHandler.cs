using FinancialChat.Application.ChatMessages.Commands.SendChatMessages;
using FinancialChat.Application.Common.Exceptions;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Application.Services;
using FinancialChat.Domain.Entities;
using FinancialChat.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialChat.Application.ChatMessages.EventHandlers;

public class BotChatMessageReceivedEventHandler : INotificationHandler<BotChatMessageReceivedEvent>
{
    private readonly ILogger<BotChatMessageReceivedEventHandler> _logger;
    private readonly BotUserService _botService;
    private readonly IMediator _meadiator;
    public BotChatMessageReceivedEventHandler(BotUserService botService,
        IMediator mediator,
        ILogger<BotChatMessageReceivedEventHandler> logger)
    {
        _logger = logger;
        _botService = botService;
        _meadiator = mediator;
    }

    public Task Handle(BotChatMessageReceivedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("FinancialChat Domain Event: {DomainEvent}", notification.GetType().Name);

            _botService.SetUserIdAsync().Wait();
            if (string.IsNullOrEmpty(_botService.UserId))
            {
                throw new NotFoundException("Bot user not found.");
            }

            var sendCommand = new SendChatMessageCommand()
            {
                IgnoreCommand = true,
                UserId = _botService.UserId,
                Message = notification.Message,
                ChatRoomCode = notification.ChatRoomCode,
            };

            _meadiator.Send(sendCommand, cancellationToken).Wait();
        }
        catch (Exception ex)
        {
            _logger.LogError("Event Error", ex);
        }

        return Task.CompletedTask;
    }
}