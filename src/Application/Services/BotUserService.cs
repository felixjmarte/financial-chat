using FinancialChat.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialChat.Application.Services;

public class BotUserService : ICurrentUserService
{
    private readonly IIdentityService _identity;
    private readonly BotOptions _botOptions;
    private readonly ILogger<BotUserService> _logger;
    public BotUserService(ILogger<BotUserService> logger,
        IOptions<BotOptions> botOptions,
        IIdentityService identityService)
    {
        _logger = logger;
        _identity = identityService;
        _botOptions = botOptions.Value;
    }

    private static string? _userId;
    public string? UserId => _userId;

    internal async Task SetUserIdAsync()
    {
        try
        {
            _userId = await _identity.GetUserIdAsync(_botOptions.UserName!, _botOptions.Password!);
        }
        catch (Exception ex)
        {
            _logger.LogError("FinancialChat UserId Getter", ex);
            _userId = null;
        }
    }
}