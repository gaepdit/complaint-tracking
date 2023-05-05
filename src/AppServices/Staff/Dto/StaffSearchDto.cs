using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Staff.Dto;

public record StaffSearchDto
{
    // Sorting
    public SortBy Sort { get; init; } = SortBy.NameAsc;

    // Fields
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Role { get; init; }
    public Guid? Office { get; init; }
    public SearchStaffStatus? Status { get; init; } = SearchStaffStatus.Active;

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Name), Name },
        { nameof(Email), Email },
        { nameof(Role), Role },
        { nameof(Office), Office.ToString() },
        { nameof(Status), Status?.ToString() },
    };

    public void TrimAll()
    {
        Name = Name?.Trim();
        Email = Email?.Trim();
    }
}

// Search enums
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchStaffStatus
{
    Active,
    Inactive,
    All,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortBy
{
    [Description("FamilyName, GivenName")] NameAsc,

    [Description("FamilyName desc, GivenName desc")]
    NameDesc,

    [Description("Office.Name, FamilyName, GivenName")]
    OfficeAsc,

    [Description("Office.Name desc, FamilyName, GivenName")]
    OfficeDesc,
}
