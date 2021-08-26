using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ComplaintTransitionListViewModel
    {
        [Display(Name = "Transferred By")]
        public ApplicationUser TransferredByUser { get; set; }

        [Display(Name = "From")]
        public ApplicationUser TransferredFromUser { get; set; }
        public Office TransferredFromOffice { get; set; }

        [Display(Name = "To")]
        public ApplicationUser TransferredToUser { get; set; }
        public Office TransferredToOffice { get; set; }

        [Display(Name = "Action Date")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime DateTransferred { get; set; }

        [Display(Name = "Date Accepted")]
        [DisplayFormat(
            DataFormatString = CTS.FormatDateTimeDisplay,
            NullDisplayText = CTS.NADisplayText)]
        public DateTime? DateAccepted { get; set; }

        [Display(Name = "Action")]
        public TransitionType TransitionType { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public string TransferredByUserName => TransferredByUser?.FullName;

        public string TransferredToUserName => TransferredToUser?.FullName;

        public string TransferredFromUserName => TransferredFromUser?.FullName;
    }
}
