using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class CreateActionTypeViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
