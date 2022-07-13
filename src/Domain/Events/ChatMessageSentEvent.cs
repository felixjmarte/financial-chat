using System;
namespace FinancialChat.Domain.Events;

public class ChatMessageSentEvent : BaseEvent
{
    public ChatMessageSentEvent(ChatMessage message)
    {
        ChatMessage = message;
    }

    public ChatMessage ChatMessage { get; }
}

