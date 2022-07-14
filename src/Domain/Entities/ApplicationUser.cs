using Microsoft.AspNetCore.Identity;

namespace Cts.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Navigation property for the roles this user belongs to.
    /// </summary>
    public IEnumerable<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

    [StringLength(50)]
    public string FirstName { get; set; } = "";

    [StringLength(50)]
    public string LastName { get; set; } = "";

    [StringLength(25)]
    public string? Phone { get; set; }

    [InverseProperty("Users")]
    public Office? Office { get; set; }

    public Guid? OfficeId { get; set; }

    public bool Active { get; set; } = true;

    // Generated fields
#pragma warning disable S125
    // public string FullName => 
    //     string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrEmpty(s)));
    //
    // [DisplayFormat(NullDisplayText = CTS.NotAvailableDisplayText, ConvertEmptyStringToNull = true)]
    // public string SortableFullName => 
    //     string.Join(", ", new[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));
    //
    // public string SelectableName
    // {
    //     get
    //     {
    //         var sn = new StringBuilder();
    //         sn.AppendJoin(", ", new[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));
    //         if (!Active)
    //         {
    //             sn.Append(" [Inactive]");
    //         }
    //
    //         return sn.ToString();
    //     }
    // }
    //
    // public string SelectableNameWithOffice
    // {
    //     get
    //     {
    //         var sn = new StringBuilder();
    //         sn.AppendJoin(", ", new[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));
    //         if(Office != null)
    //         {
    //             sn.Append(" – ");
    //             sn.Append(Office.Name);
    //         }
    //         if (!Active)
    //         {
    //             sn.Append(" [Inactive]");
    //         }
    //         return sn.ToString();
    //     }
    // }
#pragma warning restore S125
}
