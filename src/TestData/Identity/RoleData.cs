using Cts.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.TestData.Identity;

internal static partial class UserData
{
    public static IEnumerable<IdentityRole> GetRoles
    {
        get
        {
            if (field is not null) return field;
            field = AppRole.AllRoles
                .Select(pair => new IdentityRole(pair.Value.Name) { NormalizedName = pair.Key.ToUpperInvariant() })
                .ToList();
            return field;
        }
    }
}
