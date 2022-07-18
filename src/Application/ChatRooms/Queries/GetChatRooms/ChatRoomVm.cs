
using FinancialChat.Application.Common.DTO;

namespace FinancialChat.Application.ChatRooms.Queries.GetChatRooms;

public class ChatRoomVm
{
    public IList<ChatRoomDto> ChatRooms { get; set; } = new List<ChatRoomDto>();
}

