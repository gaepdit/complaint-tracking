using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.Domain.Identity;
using MyAppRoot.LocalRepository.Identity;

namespace MyAppRoot.LocalRepository.ServiceCollectionExtensions;

public static class IdentityServices
{
    public static void AddLocalIdentity(this IServiceCollection services)
    {
        services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
        services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        services.AddTransient<IStaffAppService, LocalStaffAppService>();
    }
}
