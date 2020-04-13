using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ReopenComplaintViewModel
    {
        public int Id { get; set; }

        #region Constructors

        public ReopenComplaintViewModel() { }

        public ReopenComplaintViewModel(Complaint e)
        {
            Id = e.Id;
            ComplaintIsClosed = e.ComplaintClosed;
            ComplaintIsDeleted = e.Deleted;
            CurrentOfficeId = e.CurrentOfficeId;
            Status = e.Status;
        }

        #endregion

        #region Control properties

        public bool ComplaintIsClosed { get; set; }
        public bool ComplaintIsDeleted { get; set; }
        public Guid? CurrentOfficeId { get; set; }
        public ComplaintStatus Status { get; set; }

        #endregion

        #region Data

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string Comment { get; set; }

        #endregion
    }
}
