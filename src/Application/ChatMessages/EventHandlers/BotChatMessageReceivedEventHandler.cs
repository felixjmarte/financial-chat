using FinancialChat.Application.Common.Exceptions;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Application.MessageBroker;
using FinancialChat.Domain.Entities;
using FinancialChat.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialChat.Application.ChatMessages.EventHandlers;

public class BotChatMessageReceivedEventHandler : INotificationHandler<BotChatMessageReceivedEvent>
{
    private readonly ILogger<BotChatMessageReceivedEventHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identity;
    private readonly BotOptions _botOptions;
    public BotChatMessageReceivedEventHandler(IApplicationDbContext context,
        IOptions<BotOptions> botOptions,
        ILogger<BotChatMessageReceivedEventHandler> logger,
        IIdentityService identityService)
    {
        _context = context;
        _logger = logger;
        _identity = identityService;
        _botOptions = botOptions.Value;
    }

    public Task Handle(BotChatMessageReceivedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("FinancialChat Domain Event: {DomainEvent}", notification.GetType().Name);

        var getUserTask = _identity.GetUserIdAsync(_botOptions.UserName!, _botOptions.Password!);
        getUserTask.Wait();

        var botUserId = getUserTask.Result;
        if (string.IsNullOrEmpty(botUserId))
        {
            throw new NotFoundException("Bot user not found.");
        }

        var chatRoom = _context.ChatRooms.SingleOrDefault(s => s.Code!.ToLower() == notification.ChatRoomCode);

        if (chatRoom == null)
        {
            throw new NotFoundException(nameof(ChatRoom), notification.ChatRoomCode!);
        }

        var message = new ChatMessage
        {
            UserId = botUserId,
            ChatRoomId = chatRoom.Id,
            Message = notification.Message,
        };

        message.AddDomainEvent(new ChatMessageSentEvent(message));

        _context.ChatMessages.Add(message);

        _context.SaveChangesAsync(cancellationToken).Wait();

        return Task.CompletedTask;
    }
}