using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class EditOfficeViewModel
    {
        public EditOfficeViewModel() { }

        public EditOfficeViewModel(Office item)
        {
            Id = item.Id;
            Active = item.Active;
            Name = item.Name;
            MasterUserId = item.MasterUserId;
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Active")]
        [UIHint("BooleanActive")]
        public bool Active { get; set; } = true;

        [Required]
        [Display(Name = "Master User")]
        public string MasterUserId { get; set; }

        public SelectList UsersSelectList { get; set; }
    }
}
