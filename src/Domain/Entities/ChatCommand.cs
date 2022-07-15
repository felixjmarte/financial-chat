namespace FinancialChat.Domain.Entities;

public class ChatCommand : BaseAuditableEntity
{
    public int ChatRoomId { get; set; }

    public string? Name { get; set; }
    public string? Param { get; set; }
    public string? UserId { get; set; }

    public ChatRoom ChatRoom { get; set; } = null!;
}
