using FinancialChat.Domain.Entities;
using FinancialChat.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialChat.Infrastructure.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatCommand>
{
    public void Configure(EntityTypeBuilder<ChatCommand> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired();

        builder.Property(t => t.Param)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
          .WithMany()
          .HasForeignKey(t => t.UserId)
          .IsRequired();
    }
}
