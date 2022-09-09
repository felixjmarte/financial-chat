using FinancialChat.Domain.Entities;
using FinancialChat.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        var agentRole = new IdentityRole("Agent");
        if (_roleManager.Roles.All(r => r.Name != agentRole.Name))
        {
            await _roleManager.CreateAsync(agentRole);
        }

        var botRole = new IdentityRole("Bot");
        if (_roleManager.Roles.All(r => r.Name != botRole.Name))
        {
            await _roleManager.CreateAsync(botRole);
        }


        // Default users
        var administratorUser = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };
        if (_userManager.Users.All(u => u.UserName != administratorUser.UserName))
        {
            await _userManager.CreateAsync(administratorUser, "Administrator#2022!");
            await _userManager.AddToRolesAsync(administratorUser, new[] { administratorRole.Name });
        }

        var user1 = new ApplicationUser { UserName = "user1@financhat.com", Email = "user1@financhat.com" };
        if (_userManager.Users.All(u => u.UserName != user1.UserName))
        {
            await _userManager.CreateAsync(user1, "Administrator#2022!");
            await _userManager.AddToRolesAsync(user1, new[] { agentRole.Name });
        }

        var user2 = new ApplicationUser { UserName = "user2@financhat.com", Email = "user2@financhat.com" };
        if (_userManager.Users.All(u => u.UserName != user2.UserName))
        {
            await _userManager.CreateAsync(user2, "Administrator#2022!");
            await _userManager.AddToRolesAsync(user2, new[] { agentRole.Name });
        }

        var botUser = new ApplicationUser { UserName = "bot@financhat.com", Email = "bot@financhat.com" };
        if (_userManager.Users.All(u => u.UserName != botUser.UserName))
        {
            await _userManager.CreateAsync(botUser, "Bot#2022!");
            await _userManager.AddToRolesAsync(botUser, new[] { botRole.Name });
        }


        // Default data
        // Seed, if necessary
        if (!_context.ChatRooms.Any())
        {
            _context.ChatRooms.Add(new ChatRoom
            {
                Name = "DefaultRoom",
                Code = "default",
                Global = true
            });

            await _context.SaveChangesAsync();
        }
    }
}
