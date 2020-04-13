using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class EditComplaintActionViewModel
    {
        public EditComplaintActionViewModel() { }

        public EditComplaintActionViewModel(ComplaintAction a)
        {
            Id = a.Id;
            ComplaintId = a.ComplaintId;
            ActionDate = a.ActionDate;
            ActionTypeId = a.ActionTypeId;
            Investigator = a.Investigator;
            Comments = a.Comments;
            Deleted = a.Deleted;
        }

        [Display(Name = "Complaint Action ID")]
        [Required]
        [HiddenInput]
        public Guid Id { get; set; }

        [Display(Name = "Complaint ID")]
        [Required]
        [HiddenInput]
        public int ComplaintId { get; set; }

        [Display(Name = "Action Date")]
        [DisplayFormat(DataFormatString = CTS.FormatDateEdit,
            ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Text)]
        public DateTime? ActionDate { get; set; }

        [Display(Name = "Action Description")]
        [Required]
        public Guid? ActionTypeId { get; set; }

        [StringLength(100)]
        public string Investigator { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        public bool Deleted { get; set; }

        public SelectList ActionTypesSelectList { get; set; }
    }
}
