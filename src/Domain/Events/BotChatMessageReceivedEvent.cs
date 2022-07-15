using System;
namespace FinancialChat.Domain.Events;

public class BotChatMessageReceivedEvent : BaseEvent
{
    public BotChatMessageReceivedEvent(string roomCode, string message)
    {
        ChatRoomCode = roomCode;
        Message = message;
    }

    public string ChatRoomCode { get; }
    public string Message { get; }

}

