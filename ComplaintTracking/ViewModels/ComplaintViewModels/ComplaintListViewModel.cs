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
            SetDeleted(e.Deleted);
            ReceivedByName = e.ReceivedBy?.SortableFullName;
            SourceFacilityId = e.SourceFacilityId;
            SourceFacilityName = e.SourceFacilityName;
            SetSourceCity(e.SourceCity);
            SetSourceStateName(e.SourceState?.Name);
            SetStatus(e.Status);
            CurrentOfficeName = e.CurrentOffice?.Name;
            CurrentOwnerName = e.CurrentOwner?.SortableFullName;
            ReviewComments = e.ReviewComments;
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

        private ComplaintStatus _status;

        private void SetStatus(ComplaintStatus value) => _status = value;

        public string Status => _status.GetDisplayName();

        #endregion

        #region Source column

        [Display(Name = "Source/Location")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceFacilityName { get; set; }

        private string sourceStateName;
        private void SetSourceStateName(string value) => sourceStateName = value;

        private string sourceCity;
        private void SetSourceCity(string value) => sourceCity = value;

        [Display(Name = "Location")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string SourceLocation =>
            StringFunctions.ConcatNonEmptyStrings(new[] { sourceCity, sourceStateName }, ", ");

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

        #region Comments

        [Display(Name = "Review Comments")]
        public string ReviewComments { get; set; }

        #endregion

        #region Deleted column

        private bool _deleted;

        private void SetDeleted(bool value) => _deleted = value;

        [Display(Name = "Deleted?")]
        public string Deleted => _deleted ? "Deleted" : "No";

        #endregion
    }
}
