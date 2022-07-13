using FinancialChat.Application.Common.Mappings;
using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.ChatRooms.Queries.GetChatRooms;

public class ChatMessageDto : IMapFrom<ChatMessage>
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }
}