using Cts.AppServices.StaffServices;
using Cts.Domain.Entities;
using Cts.Infrastructure.Contexts;
using Cts.Infrastructure.Identity;
using Cts.LocalRepository.ServiceCollectionExtensions;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace Cts.WebApp.Platform.Program;

public static class IdentityStores
{
    public static void AddIdentityStores(this IServiceCollection services, bool isLocal)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();

        // When running locally, you have the option to use in-memory data or build the database using LocalDB.
        if (isLocal && !ApplicationSettings.LocalDevSettings.BuildLocalDb)
        {
            // Adds local UserStore, RoleSore, and StaffAppService
            services.AddLocalIdentity();
        }
        else
        {
            // Add EF identity stores
            identityBuilder.AddRoles<IdentityRole>().AddEntityFrameworkStores<CtsDbContext>();

            // Add Staff App Services
            services.AddTransient<IStaffAppService, StaffAppService>();
        }
    }
}
