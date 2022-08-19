using Cts.AppServices.StaffServices;
using Cts.Domain.Entities;
using Cts.LocalRepository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.LocalRepository.ServiceCollectionExtensions;

public static class IdentityServices
{
    public static void AddLocalIdentity(this IServiceCollection services)
    {
        services.AddTransient<IUserStore<ApplicationUser>, LocalUserStore>();
        services.AddTransient<IRoleStore<IdentityRole>, LocalRoleStore>();
        services.AddTransient<IStaffAppService, LocalStaffAppService>();
    }
}
