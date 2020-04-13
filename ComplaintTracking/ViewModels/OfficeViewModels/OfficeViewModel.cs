using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class OfficeViewModel
    {
        public OfficeViewModel(Office item)
        {
            if (item != null)
            {
                Id = item.Id;
                Active = item.Active;
                Name = item.Name;
                MasterUser = item.MasterUser;
            }
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Active?")]
        [UIHint("BooleanActive")]
        public bool Active { get; set; } = true;

        [Required]
        [Display(Name = "Master User")]
        public virtual ApplicationUser MasterUser { get; set; }
    }
}
