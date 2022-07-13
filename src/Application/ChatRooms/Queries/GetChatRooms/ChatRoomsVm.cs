namespace FinancialChat.Application.ChatRooms.Queries.GetChatRooms;

public class ChatRoomsVm
{
    public string? Name { get; set; }

    public string? Code { get; set; }

    public bool Global { get; set; }

    public IList<ChatMessageDto> Messages { get; set; } = new List<ChatMessageDto>();
}
