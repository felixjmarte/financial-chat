using FinancialChat.Application.Common.Exceptions;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Domain.Entities;
using FinancialChat.Domain.Events;
using MediatR;

namespace FinancialChat.Application.ChatMessages.Commands.SendChatMessages;

public record SendChatMessageCommand : IRequest<int>
{
    public string? ChatRoomCode { get; init; }

    public string? Message { get; init; }
}

public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SendChatMessageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
    {
        var chatRoom = _context.ChatRooms.SingleOrDefault(s => s.Code!.ToLower() == request.ChatRoomCode);

        if (chatRoom == null)
        {
            throw new NotFoundException(nameof(ChatRoom), request.ChatRoomCode!);
        }

        var message = new ChatMessage
        {
            ChatRoomId = chatRoom.Id,
            Message = request.Message,
            UserId = _currentUserService.UserId
        };

        message.AddDomainEvent(new ChatMessageSentEvent(message));

        _context.ChatMessages.Add(message);

        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}