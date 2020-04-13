using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        [StringLength(100)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, 
            MinimumLength = 6,
            ErrorMessage = "The {0} must be {2} to {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [StringLength(100)]
        public string ConfirmPassword { get; set; }
    }
}
