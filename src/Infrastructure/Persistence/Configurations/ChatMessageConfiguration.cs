using FinancialChat.Domain.Entities;
using FinancialChat.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialChat.Infrastructure.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.Property(t => t.Message)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
          .WithMany()
          .HasForeignKey(t => t.UserId)
          .IsRequired();
    }
}
