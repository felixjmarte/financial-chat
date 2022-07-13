using FinancialChat.Application.ChatMessages.Commands.SendChatMessages;
using FinancialChat.Application.ChatRooms.Queries.GetChatRooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.WebUI.Controllers;

public class ChatMessagesController : ApiControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<int>> Send(SendChatMessageCommand command)
    {
        return await Mediator.Send(command);
    }
}

