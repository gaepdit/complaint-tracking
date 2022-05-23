using ComplaintTracking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComplaintTracking.Data
{
    // ref: https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
    public class MigratorHostedService : IHostedService
    {
        // We need to inject the IServiceProvider so we can create 
        // the scoped service, MyDbContext
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public MigratorHostedService(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();

            // Get the DbContext instance
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Initialize database
            if (Startup.IsLocal || dbContext.Database.IsSqlite())
            {
                await InitializeLocalAsync(dbContext, roleManager, userManager, _configuration);
            }
            else if (dbContext.Database.IsSqlServer())
            {
                await InitializeAsync(dbContext, roleManager);
            }
        }

        // noop
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task InitializeAsync(
            ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            // Apply database migrations 
            await dbContext.Database.MigrateAsync();

            // Initialize Roles 
            foreach (CtsRole role in Enum.GetValues(typeof(CtsRole)))
            {
                if (!await dbContext.Roles.AnyAsync(e => e.Name == role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }
        }

        private static async Task InitializeLocalAsync(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            // Test data: will not run in production

            // Create database schema
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Initialize Roles 
            foreach (CtsRole role in Enum.GetValues(typeof(CtsRole)))
            {
                if (!await context.Roles.AnyAsync(e => e.Name == role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            // Create Default Admin User
            var email = CTS.AdminEmail;
            var password = configuration.GetValue<string>("DefaultAdminPassword");
            if (!await context.Users.AnyAsync(e => e.Email == email))
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "EPD-IT",
                    LastName = "Admin",
                    Office = null
                };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(await userManager.FindByNameAsync(email),
                    CtsRole.DivisionManager.ToString());
                await userManager.AddToRoleAsync(await userManager.FindByNameAsync(email),
                    CtsRole.DataExport.ToString());
            }

            var adminUser = await userManager.FindByNameAsync(email);

            if (!await context.LookupCounties.AnyAsync())
                await context.LookupCounties.AddRangeAsync(SeedTestData.GetCounties());
            if (!await context.LookupStates.AnyAsync())
                await context.LookupStates.AddRangeAsync(SeedTestData.GetStates());
            if (!await context.LookupOffices.AnyAsync())
                await context.LookupOffices.AddRangeAsync(SeedTestData.GetOffices(adminUser));
            if (!await context.LookupConcerns.AnyAsync())
                await context.LookupConcerns.AddRangeAsync(SeedTestData.GetConcerns());
            if (!await context.LookupActionTypes.AnyAsync())
                await context.LookupActionTypes.AddRangeAsync(SeedTestData.GetActionTypes());
            await context.SaveChangesAsync();

            if (!await context.Complaints.AnyAsync())
                await context.Complaints.AddRangeAsync(await SeedTestData.GetComplaintsAsync(context, adminUser));
            await context.SaveChangesAsync();

            if (!await context.ComplaintActions.AnyAsync())
                await context.ComplaintActions.AddRangeAsync(
                    await SeedTestData.GetComplaintActions(context, adminUser));
            if (!await context.ComplaintTransitions.AnyAsync())
                await context.ComplaintTransitions.AddRangeAsync(
                    await SeedTestData.GetComplaintTransitions(context, adminUser));
            await context.SaveChangesAsync();

            // Add additional users
            if (await context.Users.CountAsync() <= 1) await SeedTestData.AddUsersAsync(context, userManager, password);
        }
    }
}
