using Cts.AppServices.BaseDto;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Entities.Offices;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Offices;

public class OfficeDisplayViewDto : IDtoHasNameProperty
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    [UIHint("BoolActive")]
    public bool Active { get; init; }
}

public class OfficeAdminViewDto : IDtoHasNameProperty
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    [UIHint("BoolActive")]
    public bool Active { get; init; }

    public StaffViewDto? Assignor { get; init; }

    [JsonIgnore]
    public string? AssignorNameWithOffice => Assignor?.DisplayNameWithOffice;
}

public class OfficeCreateDto
{
    [Required]
    [StringLength(Office.MaxNameLength, MinimumLength = Office.MinNameLength,
        ErrorMessage = "The Name must be at least {2} characters but no longer than {1}.")]
    public string Name { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Assignor")]
    public string? AssignorId { get; init; }
}

public class OfficeUpdateDto
{
    public Guid Id { get; init; }

    [Required]
    [StringLength(Office.MaxNameLength, MinimumLength = Office.MinNameLength,
        ErrorMessage = "The Name must be at least {2} characters but no longer than {1}.")]
    public string Name { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Assignor")]
    public string? AssignorId { get; init; }

    public bool Active { get; init; }
}
