using ComplaintTracking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ComplaintDetailsViewModel
    {
        public ComplaintDetailsViewModel(Complaint e)
        {
            Id = e.Id;
            Status = e.Status;
            DateEntered = e.DateEntered;
            EnteredBy = e.EnteredBy;
            DateReceived = e.DateReceived;
            ReceivedBy = e.ReceivedBy;
            ComplaintNature = e.ComplaintNature;
            ComplaintLocation = e.ComplaintLocation;
            ComplaintDirections = e.ComplaintDirections;
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
            SourcePhoneNumber = e.SourcePhoneNumber;
            SourcePhoneType = e.SourcePhoneType;
            SourceSecondaryPhoneNumber = e.SourceSecondaryPhoneNumber;
            SourceSecondaryPhoneType = e.SourceSecondaryPhoneType;
            SourceTertiaryPhoneNumber = e.SourceTertiaryPhoneNumber;
            SourceTertiaryPhoneType = e.SourceTertiaryPhoneType;
            SourceEmail = e.SourceEmail;
            ComplaintClosed = e.ComplaintClosed;
            Deleted = e.Deleted;
            DeletedBy = e.DeletedBy;
            DateDeleted = e.DateDeleted;
            DeleteComments = e.DeleteComments;
            CurrentOffice = e.CurrentOffice;
            CurrentOwner = e.CurrentOwner;
            DateCurrentOwnerAssigned = e.DateCurrentOwnerAssigned;
            DateCurrentOwnerAccepted = e.DateCurrentOwnerAccepted;
            CallerName = e.CallerName;
            CallerRepresents = e.CallerRepresents;
            CallerStreet = e.CallerStreet;
            CallerStreet2 = e.CallerStreet2;
            CallerCity = e.CallerCity;
            CallerState = e.CallerState;
            CallerPostalCode = e.CallerPostalCode;
            CallerPhoneNumber = e.CallerPhoneNumber;
            CallerPhoneType = e.CallerPhoneType;
            CallerSecondaryPhoneNumber = e.CallerSecondaryPhoneNumber;
            CallerSecondaryPhoneType = e.CallerSecondaryPhoneType;
            CallerTertiaryPhoneNumber = e.CallerTertiaryPhoneNumber;
            CallerTertiaryPhoneType = e.CallerTertiaryPhoneType;
            CallerEmail = e.CallerEmail;
            ComplaintActions = e.ComplaintActions;
            ReviewComments = e.ReviewComments;
            DateComplaintClosed = e.DateComplaintClosed;
            ReviewBy = e.ReviewBy;
        }

        [Display(Name = "Complaint ID")]
        public int Id { get; set; }

        #region Control properties

        public bool UserCanEdit { get; set; }
        public bool UserCanEditDetails { get; set; }
        public bool UserCanDelete { get; set; }
        public bool UserCanReopen { get; set; }
        public bool UserCanReview { get; set; }
        public bool UserCanAssign { get; set; }
        public bool ReviewRequested { get; set; }
        public bool MustAccept { get; set; }
        public bool UserIsOwner { get; set; }
        public bool IsAssigned { get; set; }

        #endregion

        #region Meta-data

        public ComplaintStatus Status { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime DateEntered { get; set; }

        [Display(Name = "Entered By")]
        public ApplicationUser EnteredBy { get; set; }

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime DateReceived { get; set; }

        [Display(Name = "Received By")]
        public ApplicationUser ReceivedBy { get; set; }

        #endregion

        #region Caller

        [Display(Name = "Name")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CallerName { get; set; }

        [Display(Name = "Represents")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CallerRepresents { get; set; }

        [Display(Name = "Street Address")]
        public string CallerStreet { get; set; }

        [Display(Name = "Apt / Suite / Other")]
        public string CallerStreet2 { get; set; }

        [Display(Name = "City")]
        public string CallerCity { get; set; }

        [Display(Name = "State")]
        public State CallerState { get; set; }

        [Display(Name = "Postal Code")]
        public string CallerPostalCode { get; set; }

        [Display(Name = "Primary Phone")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CallerPhoneNumber { get; set; }

        public PhoneType? CallerPhoneType { get; set; }

        [Display(Name = "Secondary Phone")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CallerSecondaryPhoneNumber { get; set; }

        public PhoneType? CallerSecondaryPhoneType { get; set; }

        [Display(Name = "Other Phone")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CallerTertiaryPhoneNumber { get; set; }

        public PhoneType? CallerTertiaryPhoneType { get; set; }

        [Display(Name = "Email Address")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        [UIHint("EmailAddress")]
        public string CallerEmail { get; set; }

        #endregion

        #region Complaint

        [Display(Name = "Nature of Complaint")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ComplaintNature { get; set; }

        [Display(Name = "Location of Complaint")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ComplaintLocation { get; set; }

        [Display(Name = "Direction to Complaint")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ComplaintDirections { get; set; }

        [Display(Name = "City of Complaint")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ComplaintCity { get; set; }

        [Display(Name = "County of Complaint")]
        public County ComplaintCounty { get; set; }

        [Display(Name = "Primary Concern")]
        public Concern PrimaryConcern { get; set; }

        [Display(Name = "Secondary Concern")]
        public Concern SecondaryConcern { get; set; }

        #endregion

        #region Source

        [Display(Name = "Facility ID Number")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityId { get; set; }

        [Display(Name = "Source Name")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceFacilityName { get; set; }

        [Display(Name = "Source Contact")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceContactName { get; set; }

        [Display(Name = "Street Address")]
        public string SourceStreet { get; set; }

        [Display(Name = "Apt / Suite / Other")]
        public string SourceStreet2 { get; set; }

        [Display(Name = "City")]
        public string SourceCity { get; set; }

        [Display(Name = "State")]
        public State SourceState { get; set; }

        [Display(Name = "Postal Code")]
        public string SourcePostalCode { get; set; }

        [Display(Name = "Primary Phone")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourcePhoneNumber { get; set; }

        public PhoneType? SourcePhoneType { get; set; }

        [Display(Name = "Secondary Phone")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceSecondaryPhoneNumber { get; set; }

        public PhoneType? SourceSecondaryPhoneType { get; set; }

        [Display(Name = "Other Phone")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceTertiaryPhoneNumber { get; set; }

        public PhoneType? SourceTertiaryPhoneType { get; set; }

        [Display(Name = "Email Address")]
        [UIHint("EmailAddress")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceEmail { get; set; }

        #endregion

        #region Assignment/Audit History

        [Display(Name = "Current Assigned Office")]
        public Office CurrentOffice { get; set; }

        [Display(Name = "Current Assigned Associate")]
        public ApplicationUser CurrentOwner { get; set; }

        [Display(Name = "Current Assigned Associate")]
        [DisplayFormat(NullDisplayText = CTS.SelectUserMasterText)]
        public string CurrentOwnerFullName => CurrentOwner?.FullName;

        [UIHint("EmailAddress")]
        public string CurrentOwnerEmail => CurrentOwner?.Email;

        [Display(Name = "Date Assigned")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime? DateCurrentOwnerAssigned { get; set; }

        [Display(Name = "Date Accepted")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime? DateCurrentOwnerAccepted { get; set; }

        public List<ComplaintTransitionListViewModel> ComplaintTransitions { get; set; }

        public DateTime? EarliestComplaintTransition =>
            ComplaintTransitions is { Count: > 0 }
                ? ComplaintTransitions[^1].DateTransferred
                : null;

        #endregion

        #region Actions

        public List<ComplaintAction> ComplaintActions { get; set; }

        #endregion

        #region Review/Closure

        [Display(Name = "Review Comments")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string ReviewComments { get; set; }

        [Display(Name = "Approved/Closed")]
        public bool ComplaintClosed { get; set; }

        [Display(Name = "Date Complaint Closed")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime? DateComplaintClosed { get; set; }

        [Display(Name = "Reviewed By")]
        public ApplicationUser ReviewBy { get; set; }

        #endregion

        #region Attachments

        public List<AttachmentViewModel> Attachments { get; set; }

        #endregion

        #region Deletion

        [Display(Name = "Deleted?")]
        [UIHint("BooleanDeleted")]
        public bool Deleted { get; set; }

        [Display(Name = "Deleted By")]
        public ApplicationUser DeletedBy { get; set; }

        [Display(Name = "Date Deleted")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime? DateDeleted { get; set; }

        [Display(Name = "Deletion Comments")]
        public string DeleteComments { get; set; }

        #endregion

        #region Additional display properties

        [Display(Name = "Caller Address")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string CallerAddress
        {
            get
            {
                string cityState = StringFunctions.ConcatNonEmptyStrings(new[] { CallerCity, CallerState?.Name }, ", ");
                string cityStateZip = StringFunctions.ConcatNonEmptyStrings(new[] { cityState, CallerPostalCode }, " ");
                return StringFunctions.ConcatNonEmptyStrings(new[] { CallerStreet, CallerStreet2, cityStateZip },
                    Environment.NewLine);
            }
        }

        [Display(Name = "Source Address")]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SourceAddress
        {
            get
            {
                string cityState = StringFunctions.ConcatNonEmptyStrings(new[] { SourceCity, SourceState?.Name }, ", ");
                string cityStateZip = StringFunctions.ConcatNonEmptyStrings(new[] { cityState, SourcePostalCode }, " ");
                return StringFunctions.ConcatNonEmptyStrings(new[] { SourceStreet, SourceStreet2, cityStateZip },
                    Environment.NewLine);
            }
        }

        #endregion
    }
}
