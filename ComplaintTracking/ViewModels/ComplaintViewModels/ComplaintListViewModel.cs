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
            SetDeleted = e.Deleted;
            ReceivedByName = e.ReceivedBy?.SortableFullName;
            SourceFacilityId = e.SourceFacilityId;
            SourceFacilityName = e.SourceFacilityName;
            SourceCity = e.SourceCity;
            SourceStateName = e.SourceState?.Name;
            SetStatus = e.Status;
            CurrentOfficeName = e.CurrentOfficeId == null ? null : e.CurrentOffice.Name;
            CurrentOwnerName = e.CurrentOwnerId == null ? null : e.CurrentOwner.SortableFullName;
        }

        #region ID column

        [Display(Name = "Complaint ID")]
        public int ComplaintId { get; set; }

        #endregion

        #region Received column

        [Display(Name = "Received By")]
        public string ReceivedByName { get; set; }

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
        public DateTime DateReceived { get; set; }

        #endregion

        #region Status column

        private ComplaintStatus status;

        public ComplaintStatus SetStatus { set { status = value; } }

        public string Status { get { return status.GetDisplayName(); } }

        #endregion

        #region Source column

        [Display(Name = "Source/Location")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceFacilityName { get; set; }

        private string sourceStateName;
        public string SourceStateName { set { sourceStateName = value; } }

        private string sourceCity;
        public string SourceCity { set { sourceCity = value; } }

        [Display(Name = "Location")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceLocation
        {
            get
            {
                return StringFunctions.ConcatNonEmptyStrings(new string[] { sourceCity, sourceStateName }, ", ");
            }
        }

        [Display(Name = "ID")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityId { get; set; }

        #endregion

        #region Assignment column

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

        #endregion

        #region Deleted column

        private bool deleted;

        public bool SetDeleted { set { deleted = value; } }

        [Display(Name = "Deleted?")]
        public string Deleted { get { return deleted ? "Deleted" : "No"; } }

        #endregion
    }
}
