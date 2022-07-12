using FinancialChat.Domain.Entities;
using FinancialChat.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialChat.Infrastructure.Persistence.Configurations;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired();

        builder.Property(t => t.Code)
            .IsRequired();

        builder.HasIndex(t => t.Code)
            .IsUnique();
    }
}

