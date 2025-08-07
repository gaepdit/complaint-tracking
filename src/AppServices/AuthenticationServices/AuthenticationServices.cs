using Cts.AppServices.AuthenticationServices.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.AppServices.AuthenticationServices;

public static class AuthenticationServices
{
    public static IServiceCollection AddAuthenticationAppServices(this IServiceCollection services) => services
        .AddScoped<IClaimsTransformation, AppClaimsTransformation>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<IAuthenticationManager, AuthenticationManager>();
}
