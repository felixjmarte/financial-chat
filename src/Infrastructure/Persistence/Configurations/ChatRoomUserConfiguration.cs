using FinancialChat.Domain.Entities;
using FinancialChat.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialChat.Infrastructure.Persistence.Configurations;

public class ChatRoomUserConfiguration : IEntityTypeConfiguration<ChatRoomUser>
{
    public void Configure(EntityTypeBuilder<ChatRoomUser> builder)
    {
        builder.Property(t => t.UserId)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}

