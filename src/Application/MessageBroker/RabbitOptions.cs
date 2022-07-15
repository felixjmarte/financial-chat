using System;
namespace FinancialChat.Application.MessageBroker;

public class RabbitOptions
{
    public const string KEY = "RabbitMQ";

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? HostName { get; set; }

    public int Port { get; set; } = 5672;

    public string VHost { get; set; } = "/";

    public string CommandsQueue { get; set; } = "commands";
    public string BotMessagesQueue { get; set; } = "bot_message";
}

