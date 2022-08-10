using Cts.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.LocalRepository.Identity;

public static class IdentityServiceCollectionExtensions
{
    public static void AddLocalIdentity(this IServiceCollection services)
    {
        services.AddTransient<IUserStore<ApplicationUser>, LocalUserStore>();
        services.AddTransient<IRoleStore<IdentityRole>, LocalRoleStore>();
    }
}
