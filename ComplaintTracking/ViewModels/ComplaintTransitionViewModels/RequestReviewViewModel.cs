using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class RequestReviewViewModel
    {
        public int Id { get; set; }

        #region Constructors

        public RequestReviewViewModel() { }

        public RequestReviewViewModel(Complaint e)
        {
            Id = e.Id;
            ComplaintIsClosed = e.ComplaintClosed;
            ComplaintIsDeleted = e.Deleted;
            CurrentOfficeId = e.CurrentOffice.Id;
            CurrentOfficeName = e.CurrentOffice.Name;
            CurrentOwnerId = e.CurrentOwnerId;
            ReviewById = e.ReviewById;
        }

        #endregion

        #region Control properties

        public bool ComplaintIsClosed { get; set; }
        public bool ComplaintIsDeleted { get; set; }
        public Guid CurrentOfficeId { get; set; }
        public string CurrentOfficeName { get; set; }
        public string CurrentOwnerId { get; set; }

        #endregion

        #region Select List

        public SelectList ManagersInOfficeSelectList { get; set; }

        #endregion

        #region Data

        [Display(Name = "Request Review From")]
        public string ReviewById { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string Comment { get; set; }

        #endregion
    }
}
