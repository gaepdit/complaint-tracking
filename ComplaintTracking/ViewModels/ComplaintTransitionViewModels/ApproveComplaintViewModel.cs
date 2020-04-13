using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ApproveComplaintViewModel
    {
        public int Id { get; set; }

        #region Constructors

        public ApproveComplaintViewModel() { }

        public ApproveComplaintViewModel(Complaint e)
        {
            Id = e.Id;
            ComplaintIsClosed = e.ComplaintClosed;
            ComplaintIsDeleted = e.Deleted;
            CurrentOfficeId = e.CurrentOfficeId;
        }

        #endregion

        #region Control properties

        public bool ComplaintIsClosed { get; set; }
        public bool ComplaintIsDeleted { get; set; }
        public Guid? CurrentOfficeId { get; set; }

        #endregion

        #region Data

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string Comment { get; set; }

        #endregion
    }
}
