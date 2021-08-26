using ComplaintTracking.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class ComplaintAction : IAuditable
    {
        public ComplaintAction() { }

        public ComplaintAction(AddComplaintActionViewModel m)
        {
            ComplaintId = m.ComplaintId;
            ActionDate = m.ActionDate ?? DateTime.Today;
            ActionTypeId = m.ActionTypeId ?? Guid.Empty;
            Investigator = m.Investigator;
            Comments = m.Comments;
        }

        public Guid Id { get; set; }

        public Complaint Complaint { get; set; }
        public int ComplaintId { get; set; }

        [Display(Name = "Action Date")]
        [DisplayFormat(DataFormatString = CTS.FormatDateDisplay)]
        public DateTime ActionDate { get; set; }

        [Display(Name = "Action Type")]
        public ActionType ActionType { get; set; }

        public Guid ActionTypeId { get; set; }

        [StringLength(100)]
        public string Investigator { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeShortDisplay)]
        public DateTime? DateEntered { get; set; }

        [Display(Name = "Entered By")]
        public ApplicationUser EnteredBy { get; set; }

        public string EnteredById { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        #region Deletion

        public bool Deleted { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DateDeleted { get; set; }

        [Display(Name = "Deleted By")]
        public ApplicationUser DeletedBy { get; set; }

        public string DeletedById { get; set; }

        #endregion
    }
}
