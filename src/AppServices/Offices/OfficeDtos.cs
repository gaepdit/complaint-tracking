using Cts.AppServices.Staff;
using Cts.Domain.Offices;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Offices;

public class OfficeViewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    [UIHint("BoolActive")]
    public bool Active { get; init; }

    public StaffViewDto? MasterUser { get; init; }
}

public class OfficeCreateDto
{
    [Required]
    [StringLength(Office.MaxNameLength, MinimumLength = Office.MinNameLength,
        ErrorMessage = "The Name must be at least {2} characters but no longer than {1}.")]
    public string Name { get; init; } = string.Empty;
    public StaffViewDto? MasterUser { get; init; }
}

public class OfficeUpdateDto
{
    public Guid Id { get; init; }

    [Required]
    [StringLength(Office.MaxNameLength, MinimumLength = Office.MinNameLength,
        ErrorMessage = "The Name must be at least {2} characters but no longer than {1}.")]
    public string Name { get; init; } = string.Empty;

    public StaffViewDto? MasterUser { get; init; }
    public bool Active { get; init; }
}
