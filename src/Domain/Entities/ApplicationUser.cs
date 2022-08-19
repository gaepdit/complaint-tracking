using Microsoft.AspNetCore.Identity;

namespace Cts.Domain.Entities;

public class ApplicationUser : IdentityUser, IEntity<string>
{
    // Properties from external login provider
    // (Email is part of IdentityUser)
    [ProtectedPersonalData]
    [StringLength(150)]
    public string FirstName { get; internal init; } = string.Empty;

    [ProtectedPersonalData]
    [StringLength(150)]
    public string LastName { get; internal init; } = string.Empty;

    // Editable user/staff properties
    public const int MaxPhoneLength = 25;

    [StringLength(MaxPhoneLength)]
    public string? Phone { get; set; }

    [InverseProperty("Staff")]
    public Office? Office { get; set; }

    public bool Active { get; set; } = true;
}
