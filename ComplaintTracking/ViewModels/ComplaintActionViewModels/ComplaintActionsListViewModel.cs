using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ComplaintActionsListViewModel
    {
        public ComplaintActionsListViewModel(ComplaintAction i)
        {
            if (i != null)
            {
                Id = i.Id;
                ComplaintId = i.ComplaintId;
                ActionDate = i.ActionDate;
                ActionType = i.ActionType;
                Investigator = i.Investigator;
                EnteredBy = i.EnteredBy;
                Comments = (i.Comments == null) ? null : i.Comments.Substring(0, Math.Min(100, i.Comments.Length)) + (i.Comments.Length > 100 ? "…" : string.Empty);
                Deleted = i.Deleted;
            }
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
        public bool Deleted { get; set; } = false;
    }
}
