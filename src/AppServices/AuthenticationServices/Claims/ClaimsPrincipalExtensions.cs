using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Cts.AppServices.AuthenticationServices.Claims;

public static class ClaimsPrincipalExtensions
{
    // Identity Provider claim types
    private const string IdentityProviderId = "idp";
    private const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

    extension(ClaimsPrincipal principal)
    {
        public string? GetIdentityProviderId() => principal.GetClaimValue(IdentityProviderId, TenantId);

        public string? GetAuthenticationMethod() => principal.FindFirstValue(ClaimTypes.AuthenticationMethod);

        public string? GetEmail() => principal.FindFirstValue(ClaimTypes.Email);

        public string GetGivenName() => principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

        public string GetFamilyName() => principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

        public bool HasMatchingClaim(string type, [NotNullWhen(true)] string? value) =>
            value is not null && principal.HasClaim(type, value);

        public bool IsActive() => principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());

        private string? GetClaimValue(params string[] claimNames)
        {
            foreach (var claim in claimNames)
            {
                var currentValue = principal.FindFirstValue(claim);
                if (!string.IsNullOrEmpty(currentValue)) return currentValue;
            }

            return null;
        }
    }
}
