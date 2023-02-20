using Cts.AppServices.Staff;
using Cts.AppServices.UserServices;
using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using Cts.LocalRepository.Identity;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace Cts.WebApp.Platform.Services;

public static class IdentityStores
{
    public static void AddIdentityStores(this IServiceCollection services, bool isLocal)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();

        // When running locally, you have the option to use in-memory data or build the database using LocalDB.
        if (isLocal && ApplicationSettings.LocalDevSettings.UseInMemoryData)
        {
            // Add local UserStore and RoleStore.
            services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
            services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        }
        else
        {
            // Add EF identity stores.
            identityBuilder.AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }

        // Add staff and user services.
        services.AddTransient<IStaffAppService, StaffAppService>();
        services.AddScoped<IUserService, UserService>();
    }
}
