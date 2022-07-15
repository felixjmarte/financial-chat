using System;
using Microsoft.AspNetCore.SignalR;
using FinancialChat.Domain.Events;
using MediatR;

namespace FinancialChat.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(string message)
        {
            await Clients.All.SendAsync("MessageReceived", message);
        }
    }
}

