using System;
namespace FinancialChat.Domain.Events;

public class ChatCommandSentEvent : BaseEvent
{
    public ChatCommandSentEvent(ChatCommand command)
    {
        ChatCommand = command;
    }

    public ChatCommand ChatCommand { get; }
}