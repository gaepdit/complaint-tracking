using ComplaintTracking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ComplaintPublicDetailsViewModel
    {
        public ComplaintPublicDetailsViewModel(Complaint e)
        {
            Id = e.Id;
            Status = e.Status;
            DateReceived = e.DateReceived;
            ComplaintNature = e.ComplaintNature;
            ComplaintLocation = e.ComplaintLocation;
            ComplaintCity = e.ComplaintCity;
            ComplaintCounty = e.ComplaintCounty;
            PrimaryConcern = e.PrimaryConcern;
            SecondaryConcern = e.SecondaryConcern;
            SourceFacilityId = e.SourceFacilityId;
            SourceFacilityName = e.SourceFacilityName;
            SourceContactName = e.SourceContactName;
            SourceStreet = e.SourceStreet;
            SourceStreet2 = e.SourceStreet2;
            SourceCity = e.SourceCity;
            SourceState = e.SourceState;
            SourcePostalCode = e.SourcePostalCode;
            ComplaintClosed = e.ComplaintClosed;
            CurrentOffice = e.CurrentOffice;
        }

        [Display(Name = "Complaint ID")]
        public int Id { get; }

        #region Meta-data

        public ComplaintStatus Status { get; }

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime DateReceived { get; }

        #endregion

        #region Complaint

        [Display(Name = "Nature of Complaint")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ComplaintNature
        {
            get => _complaintNature.RedactPII();
            set => _complaintNature = value;
        }
        private string _complaintNature;

        [Display(Name = "Location of Complaint")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ComplaintLocation
        {
            get => _complaintLocation.RedactPII();
            set => _complaintLocation = value;
        }
        private string _complaintLocation;

        [Display(Name = "City of Complaint")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ComplaintCity { get; }

        [Display(Name = "County of Complaint")]
        public County ComplaintCounty { get; }

        [Display(Name = "Primary Concern")]
        public Concern PrimaryConcern { get; }

        [Display(Name = "Secondary Concern")]
        public Concern SecondaryConcern { get; }

        #endregion

        #region Source

        [Display(Name = "Facility ID Number")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityId { get; }

        [Display(Name = "Source Name")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityName { get; }

        [Display(Name = "Source Contact")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceContactName { get; }

        [Display(Name = "Street Address")]
        public string SourceStreet { get; }

        [Display(Name = "Apt / Suite / Other")]
        public string SourceStreet2 { get; }

        [Display(Name = "City")]
        public string SourceCity { get; }

        [Display(Name = "State")]
        public State SourceState { get; }

        [Display(Name = "Postal Code")]
        public string SourcePostalCode { get; }

        #endregion

        #region Assignment

        [Display(Name = "Current Assigned Office")]
        public Office CurrentOffice { get; }

        #endregion

        #region Review/Closure

        [Display(Name = "Approved/Closed")]
        public bool ComplaintClosed { get; } = false;

        #endregion

        #region Attachments

        public List<AttachmentViewModel> Attachments { get; set;  }

        #endregion

        #region Additional display properties

        [Display(Name = "Source Address")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceAddress
        {
            get
            {
                string cityState = StringFunctions.ConcatNonEmptyStrings(new string[] { SourceCity, SourceState?.Name }, ", ");
                string cityStateZip = StringFunctions.ConcatNonEmptyStrings(new string[] { cityState, SourcePostalCode }, " ");
                return StringFunctions.ConcatNonEmptyStrings(new string[] { SourceStreet, SourceStreet2, cityStateZip }, Environment.NewLine);
            }
        }

        #endregion
    }
}
