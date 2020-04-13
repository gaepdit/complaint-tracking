using ComplaintTracking.Models;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class DeleteComplaintViewModel
    {
        public int Id { get; set; }

        #region Constructors

        public DeleteComplaintViewModel() { }

        public DeleteComplaintViewModel(Complaint e)
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
