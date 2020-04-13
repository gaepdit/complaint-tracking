using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class AddComplaintActionViewModel
    {
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
    }
}
