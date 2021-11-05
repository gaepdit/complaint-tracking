using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ComplaintListViewModel
    {
        public ComplaintListViewModel(Complaint e)
        {
            ComplaintId = e.Id;
            DateReceived = e.DateReceived;
            Deleted = e.Deleted ? "Deleted" : "No";
            ReceivedByName = e.ReceivedBy?.SortableFullName;
            SourceFacilityId = e.SourceFacilityId;
            SourceFacilityName = e.SourceFacilityName;
            SourceLocation = StringFunctions.ConcatNonEmptyStrings(new[] { e.SourceCity, e.SourceState?.Name }, ", ");
            Status = e.Status.GetDisplayName();
            CurrentOfficeName = e.CurrentOffice?.Name;
            CurrentOwnerName = e.CurrentOwner?.SortableFullName;
            PrimaryConcern = e.PrimaryConcern.Name;
        }

        // ID column

        [Display(Name = "Complaint ID")]
        public int ComplaintId { get; set; }

        // Received column

        [Display(Name = "Received By")]
        public string ReceivedByName { get; set; }

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
        public DateTime DateReceived { get; set; }

        // Status column

        public string Status { get; set; }

        // Source column

        [Display(Name = "Source Name")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceFacilityName { get; set; }

        [Display(Name = "Source Location")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceLocation { get; set; }

        [Display(Name = "Facility ID")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityId { get; set; }

        // Assignment column

        [Display(Name = "Current Assignment")]
        [DisplayFormat(
            NullDisplayText = CTS.SelectUserMasterText,
            ConvertEmptyStringToNull = true)]
        public string CurrentOwnerName { get; set; }

        [Display(Name = "EPD Office")]
        [DisplayFormat(
            NullDisplayText = CTS.NoOfficeDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CurrentOfficeName { get; set; }

        // Area of Concern column

        [Display(Name = "Primary Area of Concern")]
        public string PrimaryConcern { get; set; }

        // Deleted column

        [Display(Name = "Deleted?")]
        public string Deleted { get; set; }
    }
}
