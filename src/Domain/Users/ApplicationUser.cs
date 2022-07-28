using Cts.Domain.Offices;
using Microsoft.AspNetCore.Identity;

namespace Cts.Domain.Users;

public sealed class ApplicationUser : IdentityUser, IEntity<string>
{
    /// <summary>
    /// Navigation property for the roles this user belongs to.
    /// </summary>
    public IEnumerable<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

    [ProtectedPersonalData]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [ProtectedPersonalData]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(25)]
    public string? Phone { get; set; }

    [InverseProperty("Users")]
    public Office? Office { get; set; }

    public bool Active { get; set; } = true;
}
