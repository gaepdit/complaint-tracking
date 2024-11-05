using Cts.AppServices.Offices;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Staff.Dto;

public record StaffViewDto : INamedEntity
{
    public string Id { get; init; } = null!;
    public string GivenName { get; init; } = null!;
    public string FamilyName { get; init; } = null!;

    [Display(Name = "Email (cannot be changed)")]
    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }
    public OfficeViewDto? Office { get; init; }
    public bool Active { get; init; }
    public string? ObjectIdentifier { get; set; }

    // Display properties
    [JsonIgnore]
    public string Name => new[] { GivenName, FamilyName }.ConcatWithSeparator();

    [JsonIgnore]
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");

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

    public StaffUpdateDto AsUpdateDto() => new()
    {
        PhoneNumber = PhoneNumber,
        OfficeId = Office?.Id,
        Active = Active,
    };
}
