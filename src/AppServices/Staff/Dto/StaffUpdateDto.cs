using Cts.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Staff.Dto;

public record StaffUpdateDto
{
    [StringLength(ApplicationUser.MaxPhoneLength,
        ErrorMessage = "The Phone Number must not be longer than {1} characters.")]
    [Display(Name = "Phone")]
    public string? PhoneNumber { get; init; }

    [Required]
    [Display(Name = "Office")]
    public Guid? OfficeId { get; init; }

    [Required]
    public bool Active { get; set; }
}
