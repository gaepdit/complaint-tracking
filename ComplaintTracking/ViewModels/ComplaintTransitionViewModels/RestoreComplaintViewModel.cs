using ComplaintTracking.Models;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class RestoreComplaintViewModel
    {
        public int Id { get; set; }

        #region Constructors

        public RestoreComplaintViewModel() { }

        public RestoreComplaintViewModel(Complaint e)
        {
            Id = e.Id;
            ComplaintIsDeleted = e.Deleted;
        }

        #endregion

        #region Control properties

        public bool ComplaintIsDeleted { get; set; }

        #endregion

        #region Data

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string Comment { get; set; }

        #endregion
    }
}
