using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Infrastructure.DataSeeding;

internal class IdentityDataSeeder(StoreIdentityDbContext dbContext, UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSeeder> logger) : IDataSeeder
{
    private readonly StoreIdentityDbContext _dbContext = dbContext;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly ILogger<IdentityDataSeeder> _logger = logger;

    public async Task SeedDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken);

            if (pendingMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (!await _roleManager.Roles.AnyAsync(cancellationToken))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!await _userManager.Users.AnyAsync(cancellationToken))
            {
                var admin = new ApplicationUser
                {
                    DisplayName = "Mohamed Ahmed",
                    Email = "Mohamed@gmail.com",
                    UserName = "Mohamed",
                    PhoneNumber = "01152635285"
                };

                var createResult = await _userManager.CreateAsync(admin, "P@ssw0rd");

                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    _logger.LogWarning("Could not seed default admin user: {Errors}",
                        string.Join("; ", createResult.Errors.Select(e => e.Description)));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Identity data seeding failed.");
            return;
        }

    }

}
