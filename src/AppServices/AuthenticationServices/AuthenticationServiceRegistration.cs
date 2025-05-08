using Cts.AppServices.AuthenticationServices.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.AuthenticationServices;

public static class AuthenticationServiceRegistration
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services) => services
        .AddScoped<IClaimsTransformation, AppClaimsTransformation>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<IAuthenticationManager, AuthenticationManager>();
}
