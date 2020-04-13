using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ComplaintTracking.ViewModels
{
    public class ViewComplaintActionsViewModel : AddComplaintActionViewModel
    {
        #region Constructors

        public ViewComplaintActionsViewModel() { }

        public ViewComplaintActionsViewModel(Complaint e)
        {
            ComplaintId = e.Id;
            CurrentOfficeId = e.CurrentOfficeId;
            CurrentOwnerId = e.CurrentOwnerId;
            ComplaintClosed = e.ComplaintClosed;
            ComplaintDeleted = e.Deleted;
            DateCurrentOwnerAccepted = e.DateCurrentOwnerAccepted;
        }

        public ViewComplaintActionsViewModel(Complaint e, AddComplaintActionViewModel vm)
        {
            CurrentOfficeId = e.CurrentOfficeId;
            CurrentOwnerId = e.CurrentOwnerId;
            ComplaintClosed = e.ComplaintClosed;
            ComplaintDeleted = e.Deleted;
            DateCurrentOwnerAccepted = e.DateCurrentOwnerAccepted;

            ComplaintId = vm.ComplaintId;
            ActionDate = vm.ActionDate;
            ActionTypeId = vm.ActionTypeId;
            Investigator = vm.Investigator;
            Comments = vm.Comments;
        }

        #endregion

        #region Control properties

        public bool UserCanDelete { get; set; }
        public bool ComplaintClosed { get; set; }
        public bool ComplaintDeleted { get; set; } = false;
        public DateTime? DateCurrentOwnerAccepted { get; set; }
        public Guid? CurrentOfficeId { get; set; }
        public string CurrentOwnerId { get; set; }

        #endregion

        public SelectList ActionTypesSelectList { get; set; }
        public List<ComplaintAction> ComplaintActions { get; set; }
    }
}
