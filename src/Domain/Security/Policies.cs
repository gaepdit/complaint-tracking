using Microsoft.AspNetCore.Authorization;

namespace Cts.Domain.Security.Policies;

public static class PolicyName
{
    public const string UserAdministrator = nameof(UserAdministrator);
    public const string SiteMaintainer = nameof(SiteMaintainer);
    public const string DivisionManager = nameof(DivisionManager);
}

public static class Policies
{
    public static AuthorizationPolicy SiteMaintainerPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new SiteMaintainerRequirement())
            .Build();

    public static AuthorizationPolicy UserAdministratorPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new UserAdministratorRequirement())
            .Build();

    public static AuthorizationPolicy DivisionManagerPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new DivisionManagerRequirement())
            .Build();
}
