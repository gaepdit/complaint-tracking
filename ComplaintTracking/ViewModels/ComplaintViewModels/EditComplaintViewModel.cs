using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class EditComplaintViewModel
    {
        public int Id { get; set; }

        #region Constructors

        public EditComplaintViewModel() { }

        public EditComplaintViewModel(Complaint e)
        {
            Id = e.Id;
            ComplaintIsClosed = e.ComplaintClosed;
            ComplaintIsDeleted = e.Deleted;
            DateReceivedDate = e.DateReceived.Date;
            DateReceivedTime = e.DateReceived;
            ReceivedById = e.ReceivedById;
            EnteredById = e.EnteredById;
            DateEntered = e.DateEntered;
            CallerName = e.CallerName;
            CallerRepresents = e.CallerRepresents;
            CallerStreet = e.CallerStreet;
            CallerStreet2 = e.CallerStreet2;
            CallerCity = e.CallerCity;
            CallerStateId = e.CallerStateId;
            CallerPostalCode = e.CallerPostalCode;
            CallerPhoneNumber = e.CallerPhoneNumber;
            CallerPhoneType = e.CallerPhoneType;
            CallerSecondaryPhoneNumber = e.CallerSecondaryPhoneNumber;
            CallerSecondaryPhoneType = e.CallerSecondaryPhoneType;
            CallerTertiaryPhoneNumber = e.CallerTertiaryPhoneNumber;
            CallerTertiaryPhoneType = e.CallerTertiaryPhoneType;
            CallerEmail = e.CallerEmail;
            ComplaintNature = e.ComplaintNature;
            ComplaintLocation = e.ComplaintLocation;
            ComplaintDirections = e.ComplaintDirections;
            ComplaintCity = e.ComplaintCity;
            ComplaintCountyId = e.ComplaintCountyId;
            PrimaryConcernId = e.PrimaryConcernId;
            SecondaryConcernId = e.SecondaryConcernId;
            SourceFacilityId = e.SourceFacilityId;
            SourceContactName = e.SourceContactName;
            SourceFacilityName = e.SourceFacilityName;
            SourceStreet = e.SourceStreet;
            SourceStreet2 = e.SourceStreet2;
            SourceCity = e.SourceCity;
            SourceStateId = e.SourceStateId;
            SourcePostalCode = e.SourcePostalCode;
            SourcePhoneNumber = e.SourcePhoneNumber;
            SourcePhoneType = e.SourcePhoneType;
            SourceSecondaryPhoneNumber = e.SourceSecondaryPhoneNumber;
            SourceSecondaryPhoneType = e.SourceSecondaryPhoneType;
            SourceTertiaryPhoneNumber = e.SourceTertiaryPhoneNumber;
            SourceTertiaryPhoneType = e.SourceTertiaryPhoneType;
            SourceEmail = e.SourceEmail;
            CurrentOfficeId = e.CurrentOfficeId;
            CurrentOwnerId = e.CurrentOwnerId;
            DateCurrentOwnerAccepted = e.DateCurrentOwnerAccepted;
        }

        #endregion

        #region Control properties

        public bool ComplaintIsClosed { get; set; }

        public bool ComplaintIsDeleted { get; set; }

        #endregion

        #region Select Lists

        public CommonSelectLists SelectLists { get; set; }

        #endregion

        #region Meta-data

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateEdit,
            ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Text)]
        public DateTime? DateReceivedDate { get; set; }

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatTimeEdit,
            ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Text)]
        public DateTime? DateReceivedTime { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedById { get; set; }

        public string EnteredById { get; set; }
        public DateTime DateEntered { get; set; }

        #endregion

        #region Caller

        [Display(Name = "Caller Name")]
        [StringLength(100)]
        public string CallerName { get; set; }

        [Display(Name = "Represents")]
        [StringLength(25)]
        public string CallerRepresents { get; set; }

        [Display(Name = "Street Address")]
        [StringLength(100)]
        public string CallerStreet { get; set; }

        [Display(Name = "Apt / Suite / Other")]
        [StringLength(100)]
        public string CallerStreet2 { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string CallerCity { get; set; }

        [Display(Name = "State")]
        public int? CallerStateId { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(10)]
        [DataType(DataType.PostalCode)]
        public string CallerPostalCode { get; set; }

        [Display(Name = "Primary Phone")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string CallerPhoneNumber { get; set; }

        public PhoneType? CallerPhoneType { get; set; }

        [Display(Name = "Secondary Phone")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string CallerSecondaryPhoneNumber { get; set; }

        public PhoneType? CallerSecondaryPhoneType { get; set; }

        [Display(Name = "Other Phone")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string CallerTertiaryPhoneNumber { get; set; }

        public PhoneType? CallerTertiaryPhoneType { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(150)]
        [DataType(DataType.EmailAddress)]
        public string CallerEmail { get; set; }

        #endregion

        #region Complaint

        [Display(Name = "Nature of Complaint")]
        [DataType(DataType.MultilineText)]
        public string ComplaintNature { get; set; }

        [Display(Name = "Location of Complaint")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string ComplaintLocation { get; set; }

        [Display(Name = "Direction to Complaint")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string ComplaintDirections { get; set; }

        [Display(Name = "City of Complaint")]
        [StringLength(50)]
        public string ComplaintCity { get; set; }

        [Display(Name = "County of Complaint")]
        public int? ComplaintCountyId { get; set; }

        [Display(Name = "Primary Concern")]
        [Required]
        public Guid? PrimaryConcernId { get; set; }

        [Display(Name = "Secondary Concern")]
        public Guid? SecondaryConcernId { get; set; }

        #endregion

        #region Source

        [Display(Name = "Facility ID Number")]
        [StringLength(50)]
        public string SourceFacilityId { get; set; }

        [Display(Name = "Source Contact")]
        [StringLength(100)]
        public string SourceContactName { get; set; }

        [Display(Name = "Source Name")]
        [StringLength(100)]
        public string SourceFacilityName { get; set; }

        [Display(Name = "Street Address")]
        [StringLength(100)]
        public string SourceStreet { get; set; }

        [Display(Name = "Apt / Suite / Other")]
        [StringLength(100)]
        public string SourceStreet2 { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string SourceCity { get; set; }

        [Display(Name = "State")]
        public int? SourceStateId { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(10)]
        [DataType(DataType.PostalCode)]
        public string SourcePostalCode { get; set; }

        [Display(Name = "Primary Phone")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string SourcePhoneNumber { get; set; }

        public PhoneType? SourcePhoneType { get; set; }

        [Display(Name = "Secondary Phone")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string SourceSecondaryPhoneNumber { get; set; }

        public PhoneType? SourceSecondaryPhoneType { get; set; }

        [Display(Name = "Other Phone")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string SourceTertiaryPhoneNumber { get; set; }

        public PhoneType? SourceTertiaryPhoneType { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        [StringLength(150)]
        [DataType(DataType.EmailAddress)]
        public string SourceEmail { get; set; }

        #endregion

        #region Assignment (used for access control only)

        public Guid? CurrentOfficeId { get; set; }
        public string CurrentOwnerId { get; set; }
        public DateTime? DateCurrentOwnerAccepted { get; set; }

        #endregion
    }
}
