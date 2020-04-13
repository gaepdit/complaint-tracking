using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ViewAttachmentsViewModel
    {
        #region Constructors

        public ViewAttachmentsViewModel() { }

        public ViewAttachmentsViewModel(Complaint e)
        {
            ComplaintId = e.Id;
            ComplaintClosed = e.ComplaintClosed;
            ComplaintDeleted = e.Deleted;
            DateCurrentOwnerAccepted = e.DateCurrentOwnerAccepted;
            CurrentOfficeId = e.CurrentOfficeId;
            CurrentOwnerId = e.CurrentOwnerId;
            EnteredById = e.EnteredById;
            DateEntered = e.DateEntered;
        }

        #endregion

        [Display(Name = "Complaint ID")]
        [Required]
        [HiddenInput]
        public int ComplaintId { get; set; }

        public List<AttachmentViewModel> Attachments { get; set; }

        #region Control properties

        public bool ComplaintClosed { get; set; }
        public bool ComplaintDeleted { get; set; } = false;
        public DateTime? DateCurrentOwnerAccepted { get; set; }
        public Guid? CurrentOfficeId { get; set; }
        public string CurrentOwnerId { get; set; }
        public string EnteredById { get; private set; }
        public DateTime DateEntered { get; private set; }

        #endregion
    }
}
