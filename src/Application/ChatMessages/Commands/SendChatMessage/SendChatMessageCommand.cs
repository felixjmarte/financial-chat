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
    public string? UserId { get; init; }
    internal bool IgnoreCommand { get; init; }
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
        if (string.IsNullOrEmpty(request.Message) || string.IsNullOrEmpty(request.ChatRoomCode))
        {
            throw new ValidationException();
        }

        var chatRoom = _context.ChatRooms.SingleOrDefault(s => s.Code!.ToLower() == request.ChatRoomCode);

        if (chatRoom == null)
        {
            throw new NotFoundException(nameof(ChatRoom), request.ChatRoomCode!);
        }
        else if (IsCommand(request))
        {
            return await HandlerAsCommand(request, chatRoom, cancellationToken);
        }


        var message = new ChatMessage
        {
            ChatRoomId = chatRoom.Id,
            Message = request.Message,
            UserId = request.UserId ?? _currentUserService.UserId
        };

        message.AddDomainEvent(new ChatMessageSentEvent(message));

        _context.ChatMessages.Add(message);

        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }

    private bool IsCommand(SendChatMessageCommand request)
    {
        return !request.IgnoreCommand && request.Message!.Split(' ').First().StartsWith("/stock=");
    }

    private async Task<int> HandlerAsCommand(SendChatMessageCommand request, ChatRoom chatRoom, CancellationToken cancellationToken)
    {
        var command = new ChatCommand
        {
            ChatRoom = chatRoom,
            ChatRoomId = chatRoom.Id,
            Name = request.Message!.Split('=').First().Trim(),
            Param = request.Message!.Split('=').Last().Trim(),
            UserId = request.UserId ?? _currentUserService.UserId,
        };

        command.AddDomainEvent(new ChatCommandSentEvent(command));

        _context.ChatCommands.Add(command);

        await _context.SaveChangesAsync(cancellationToken);

        return 0;
    }
}