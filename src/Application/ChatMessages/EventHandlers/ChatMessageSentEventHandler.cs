using AutoMapper;
using FinancialChat.Application.Common.DTO;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Application.ChatMessages.EventHandlers;

public class ChatMessageSentEventHandler : INotificationHandler<ChatMessageSentEvent>
{
    private readonly ILogger<ChatMessageSentEventHandler> _logger;
    private readonly IMediator _meadiator;
    private readonly IMapper _mapper;
    private readonly IClientAppNotifier<BaseDto> _clientAppNotifier;
    public ChatMessageSentEventHandler(IMediator mediator, IMapper mapper,
        IClientAppNotifier<BaseDto> clientAppNotifier,
        ILogger<ChatMessageSentEventHandler> logger)
    {
        _logger = logger;
        _meadiator = mediator;
        _mapper = mapper;
        _clientAppNotifier = clientAppNotifier;
    }

    public Task Handle(ChatMessageSentEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("FinancialChat Domain Event: {DomainEvent}", notification.GetType().Name);

            _clientAppNotifier.Notify(nameof(ChatMessageSentEvent), _mapper.Map<ChatMessageDto>(notification.ChatMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError("Event Error", ex);
        }

        return Task.CompletedTask;
    }
}