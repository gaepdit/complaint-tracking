using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
