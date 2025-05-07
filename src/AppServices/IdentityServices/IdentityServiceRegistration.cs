using Cts.AppServices.IdentityServices.Claims;
using Cts.AppServices.Staff;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.IdentityServices;

public static class IdentityServiceRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services) => services
        // Add claims transformations
        .AddScoped<IClaimsTransformation, AppClaimsTransformation>()

        // Add staff and user services.
        .AddTransient<IStaffService, StaffService>()
        .AddScoped<IUserService, UserService>();
}
