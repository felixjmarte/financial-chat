using System.Security.Claims;
using FinancialChat.Application.ChatMessages.Commands.SendChatMessages;
using FinancialChat.Application.Common.DTO;
using FinancialChat.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.WebUI.Hubs
{
    [Authorize]
    public class ChatHub : BaseHub
    {
        public async Task SendMessage(ChatMessageDto message)
        {
            await Mediator.Send(new SendChatMessageCommand()
            {
                UserId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier),
                ChatRoomCode = message.ChatRoomCode,
                Message = message.Message,
            });
        }
    }
}

