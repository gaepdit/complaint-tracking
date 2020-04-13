using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ReturnComplaintViewModel
    {
        public int Id { get; set; }

        #region Constructors

        public ReturnComplaintViewModel() { }

        public ReturnComplaintViewModel(Complaint e)
        {
            Id = e.Id;
            ComplaintIsClosed = e.ComplaintClosed;
            ComplaintIsDeleted = e.Deleted;
            CurrentOfficeId = e.CurrentOfficeId;
            CurrentOwnerId = e.CurrentOwnerId;
            Status = e.Status;
        }

        #endregion

        #region Control properties

        public bool ComplaintIsClosed { get; set; }
        public bool ComplaintIsDeleted { get; set; }
        public bool DisableCurrentOwnerSelect { get; set; }
        public ComplaintStatus Status { get; set; }

        #endregion

        #region Select Lists

        public SelectList OfficesSelectList { get; set; }
        public SelectList UsersInOfficeSelectList { get; set; }

        #endregion

        #region Data

        [Display(Name = "Assigned Office")]
        [Required]
        public Guid? CurrentOfficeId { get; set; }

        [Display(Name = "Assigned Associate")]
        public string CurrentOwnerId { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string Comment { get; set; }

        #endregion
    }
}
