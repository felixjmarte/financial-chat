using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Add DbSet Entities

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
