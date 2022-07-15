using FinancialChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ChatRoom> ChatRooms { get; }
    DbSet<ChatMessage> ChatMessages { get; }
    DbSet<ChatCommand> ChatCommands { get; }
    DbSet<ChatRoomUser> ChatRoomUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
