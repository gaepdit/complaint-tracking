using Cts.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Staff.Dto;

public record StaffUpdateDto
{
    public string Id { get; init; } = string.Empty;

    [StringLength(ApplicationUser.MaxPhoneLength,
        ErrorMessage = "The Phone Number must not be longer than {1} characters.")]
    public string? Phone { get; init; }

    [Required]
    [Display(Name = "Office")]
    public Guid? OfficeId { get; init; }

    [Required]
    public bool Active { get; set; }
}
