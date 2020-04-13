using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class CreateConcernViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
