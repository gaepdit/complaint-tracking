using Cts.AppServices.DtoBase;
using Cts.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Offices;

public record OfficeViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);

public record OfficeAdminViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active)
{
    public StaffViewDto? Assignor { get; init; }

    [JsonIgnore]
    public string? AssignorNameWithOffice => Assignor?.DisplayNameWithOffice;
}

public record OfficeCreateDto(string Name) : StandardNamedEntityCreateDto(Name)
{
    [Required]
    [Display(Name = "Assignor")]
    public string? AssignorId { get; init; }
}

public record OfficeUpdateDto(Guid Id, string Name, bool Active) : StandardNamedEntityUpdateDto(Id, Name, Active)
{
    [Required]
    [Display(Name = "Assignor")]
    public string? AssignorId { get; init; }
}
