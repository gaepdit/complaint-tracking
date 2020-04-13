using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class SetPasswordViewModel
    {
        public string UserId { get; set; }

        public string Code { get; set; }

        [Required]
        [StringLength(100, 
            MinimumLength = 6,
            ErrorMessage = "The {0} must be {2} to {1} characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
