using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class PublicSearchResultsViewModel
    {
        public PublicSearchResultsViewModel(Complaint e)
        {
            ComplaintId = e.Id;
            DateReceived = e.DateReceived;
            SourceFacilityName = e.SourceFacilityName;
            SourceCity = e.SourceCity;
            SourceStateName = e.SourceState?.Name;
            CurrentOfficeName = e.CurrentOffice.Name;
        }

        #region ID column

        [Display(Name = "Complaint ID")]
        public int ComplaintId { get; set; }

        #endregion

        #region Received column

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
        public DateTime DateReceived { get; set; }

        #endregion

        #region Source column

        [Display(Name = "Source/Location")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceFacilityName { get; set; }

        public string SourceStateName { get; set; }

        public string SourceCity { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceLocation
        {
            get
            {
                return StringFunctions.ConcatNonEmptyStrings(new string[] { SourceCity, SourceStateName }, ", ");
            }
        }

        #endregion

        #region Assignment column

        [Display(Name = "EPD Office Assigned")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CurrentOfficeName { get; set; }

        #endregion
    }
}
