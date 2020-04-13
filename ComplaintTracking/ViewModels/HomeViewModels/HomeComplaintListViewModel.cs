using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class HomeComplaintListViewModel
    {
        public HomeComplaintListViewModel(Complaint e)
        {
            Id = e.Id;
            DateReceived = e.DateReceived;
            SourceFacilityId = e.SourceFacilityId;
            SourceFacilityName = e.SourceFacilityName;
            SourceCity = e.SourceCity;
            SourceStateName = e.SourceState?.Name;
            Status = e.Status;
        }

        [Display(Name = "Complaint ID")]
        public int Id { get; set; }

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
        public DateTime DateReceived { get; set; }

        public ComplaintStatus Status { get; set; }

        [Display(Name = "Source")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityName { get; set; }

        public string SourceStateName { get; set; }

        public string SourceCity { get; set; }

        [Display(Name = "Location")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceLocation
        {
            get
            {
                return StringFunctions.ConcatNonEmptyStrings(new string[] { SourceCity, SourceStateName }, ", ");
            }
        }

        [Display(Name = "ID")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityId { get; set; }
    }
}
