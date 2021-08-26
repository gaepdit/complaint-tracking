using ComplaintTracking.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintTracking.ViewModels
{
    public class EditAccountViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        [EmailAddress]
        [DnrEmailAddress(ErrorMessage = "A valid DNR email address is required")]
        public string Email { get; set; }
        public string EmailMD5Hash =>
            string.Join("",
                System.Security.Cryptography.MD5.Create()
                    .ComputeHash(System.Text.Encoding.ASCII.GetBytes(Email.Trim().ToLower()))
                    .Select(s => s.ToString("x2")));

        [StringLength(25)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Office")]
        public Guid? OfficeId { get; set; }

        public SelectList OfficeSelectList { get; set; }
    }
}
