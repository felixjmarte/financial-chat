
using FinancialChat.Application.Common.DTO;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Domain.Events;
using FinancialChat.WebUI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.WebUI.Services;

public class NotifierService : IClientAppNotifier<BaseDto>
{
    private readonly ILogger<NotifierService> _logger;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly IDateTime _date;
    public NotifierService(IDateTime dateTime,
        ILogger<NotifierService> logger,
        IHubContext<ChatHub> chatHub)
    {
        _date = dateTime;
        _logger = logger;
        _chatHub = chatHub;
    }

    public Task Notify(string eventName, BaseDto entity)
    {
         try
        {
            _logger.LogInformation("NotifierService: {Fecha}", _date.Now);

            switch (eventName)
            {
                case nameof(ChatMessageSentEvent):
                    SendMessage((ChatMessageDto)entity);
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Notify Service Error", ex);
        }

        return Task.CompletedTask;
    }

    private void SendMessage(ChatMessageDto message)
    {
        _chatHub.Clients.All.SendAsync("MessageReceived", message);
    }
}