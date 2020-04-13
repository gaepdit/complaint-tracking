using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class CreateOfficeViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Master User")]
        public string MasterUserId { get; set; }

        public SelectList UsersSelectList { get; set; }
    }
}
