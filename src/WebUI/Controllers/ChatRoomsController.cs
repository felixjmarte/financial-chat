using FinancialChat.Application.ChatRooms.Queries.GetChatRooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.WebUI.Controllers;


[Authorize]
public class ChatRoomsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ChatRoomVm>> Get()
    {
        return await Mediator.Send(new GetChatRoomsQuery());
    }

    [HttpGet("{chatRoomCode}")]
    public async Task<ActionResult<ChatRoomVm>> Get(string chatRoomCode)
    {
        return await Mediator.Send(new GetChatRoomsQuery { ChatRoomCode = chatRoomCode });
    }
}