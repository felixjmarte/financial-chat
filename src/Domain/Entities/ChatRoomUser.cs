namespace FinancialChat.Domain.Entities;

public class ChatRoomUser : BaseEntity
{
    public int ChatRoomId { get; set; }
    public string? UserId { get; set; }
}

