using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ActionTypeViewModel
    {
        public ActionTypeViewModel(ActionType item)
        {
            if (item != null)
            {
                Id = item.Id;
                Active = item.Active;
                Name = item.Name;
            }
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Active?")]
        [UIHint("BooleanActive")]
        public bool Active { get; set; } = true;
    }
}
