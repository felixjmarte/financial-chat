using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialChat.Domain.Entities;

public class ChatRoom : BaseAuditableEntity
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public bool Global { get; set; } 

    public IList<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();
    public IList<ChatRoomUser> Users { get; private set; } = new List<ChatRoomUser>();

    [NotMapped]
    public const int MESSAGES_LIMIT = 50;
}

