using ComplaintTracking.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

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

        [StringLength(25)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Office")]
        public Guid? OfficeId { get; set; }

        [Display(Name = "Division Manager")]
        public bool IsDivisionManager { get; set; }

        [Display(Name = "Manager")]
        public bool IsManager { get; set; }

        [Display(Name = "User Account Admin")]
        public bool IsUserAdmin { get; set; }

        [Display(Name = "Data Export")]
        public bool IsDataExporter { get; set; }

        [Display(Name = "Attachment Editor")]
        public bool IsAttachmentEditor { get; set; }

        public bool Active { get; set; }

        public SelectList OfficeSelectList { get; set; }

        public bool CurrentUserIsDivisionManager { get; set; }
    }
}
