namespace FinancialChat.Domain.Entities;

public class ChatMessage : BaseAuditableEntity
{
    public int ChatRoomId { get; set; }

    public string? Message { get; set; }

    public string? UserId { get; set; }

    public ChatRoom ChatRoom { get; set; } = null!;
}