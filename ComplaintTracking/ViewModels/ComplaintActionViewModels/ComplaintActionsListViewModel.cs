using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ComplaintActionsListViewModel
    {
        public ComplaintActionsListViewModel(ComplaintAction i)
        {
            if (i == null) return;

            Id = i.Id;
            ComplaintId = i.ComplaintId;
            ActionDate = i.ActionDate;
            ActionType = i.ActionType;
            Investigator = i.Investigator;
            EnteredBy = i.EnteredBy;
            Comments = TruncateComment(i.Comments);
            Deleted = i.Deleted;
        }

        private static string TruncateComment(string comment)
        {
            if (comment is null) return null;
            return comment.Length > 100 ? comment[..100] + "…" : comment;
        }

        public Guid Id { get; set; }

        [Display(Name = "Complaint ID")]
        public int ComplaintId { get; set; }

        [Display(Name = "Action Date")]
        [DisplayFormat(DataFormatString = CTS.FormatDateDisplay)]
        [DataType(DataType.Text)]
        public DateTime ActionDate { get; set; }

        [Display(Name = "Action Type")]
        public ActionType ActionType { get; set; }

        public string Investigator { get; set; }

        [Display(Name = "Entered By")]
        public ApplicationUser EnteredBy { get; set; }

        public string Comments { get; set; }

        [Display(Name = "Deleted?")]
        [UIHint("BooleanDeleted")]
        public bool Deleted { get; set; }
    }
}
