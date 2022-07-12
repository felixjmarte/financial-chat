using FinancialChat.Application.Common.Interfaces;

namespace FinancialChat.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
