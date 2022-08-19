using Cts.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.TestData.Identity;

internal static partial class Data
{
    private static IEnumerable<IdentityRole>? _roles;

    public static IEnumerable<IdentityRole> GetRoles
    {
        get
        {
            if (_roles is not null) return _roles;
            _roles = CtsRole.AllRoles
                .Select(r => new IdentityRole(r.Name) { NormalizedName = r.Name.ToUpperInvariant() })
                .ToList();
            return _roles;
        }
    }
}
