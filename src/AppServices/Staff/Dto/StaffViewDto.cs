using Cts.AppServices.BaseDto;
using Cts.AppServices.Offices;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Staff.Dto;

public class StaffViewDto : IDtoHasNameProperty
{
    public string Id { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public OfficeDisplayViewDto? Office { get; init; }

    [UIHint("BoolActive")]
    public bool Active { get; init; }

    // Read-only properties
    [JsonIgnore]
    public string Name =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string DisplayNameWithOffice
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append(Name);

            if (Office != null) sn.Append($" ({Office.Name})");
            if (!Active) sn.Append(" [Inactive]");

            return sn.ToString();
        }
    }

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string SortableNameWithOffice
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append(SortableFullName);

            if (Office != null) sn.Append($" ({Office.Name})");
            if (!Active) sn.Append(" [Inactive]");

            return sn.ToString();
        }
    }

    public StaffUpdateDto AsUpdateDto() => new() { Id = Id, Phone = Phone, OfficeId = Office?.Id, Active = Active };
}
