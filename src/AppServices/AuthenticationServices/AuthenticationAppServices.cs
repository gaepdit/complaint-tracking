using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.AuthenticationServices;

public static class AuthenticationAppServices
{
    public static IServiceCollection AddAuthenticationAppServices(this IServiceCollection services) => services
        .AddScoped<IClaimsTransformation, AppClaimsTransformation>()
        .AddScoped<IAuthenticationManager, AuthenticationManager>()
        .AddScoped<IUserService, UserService>();
}
