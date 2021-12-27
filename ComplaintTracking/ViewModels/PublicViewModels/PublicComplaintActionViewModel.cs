using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class PublicComplaintActionViewModel
    {
        public PublicComplaintActionViewModel(ComplaintAction e)
        {
            ComplaintId = e.ComplaintId;
            ActionDate = e.ActionDate;
            ActionType = e.ActionType;
            Comments = e.Comments;
        }

        [Display(Name = "Complaint ID")]
        public int ComplaintId { get; }

        [Display(Name = "Action Date")]
        [DisplayFormat(DataFormatString = CTS.FormatDateDisplay)]
        public DateTime ActionDate { get; }

        [Display(Name = "Action Type")]
        public ActionType ActionType { get; }

        public string Comments
        {
            get => StringFunctions.RedactPII(_comments);
            set => _comments = value;
        }
        private string _comments;
    }
}
