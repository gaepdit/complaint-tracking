using Cts.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.TestData.Identity;

internal static partial class UserData
{
    private static IEnumerable<IdentityRole>? _roles;

    public static IEnumerable<IdentityRole> GetRoles
    {
        get
        {
            if (_roles is not null) return _roles;
            _roles = AppRole.AllRoles
                .Select(pair => new IdentityRole(pair.Value.Name) { NormalizedName = pair.Key.ToUpperInvariant() })
                .ToList();
            return _roles;
        }
    }
}
