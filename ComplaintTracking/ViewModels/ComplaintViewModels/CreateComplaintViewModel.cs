using ComplaintTracking.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class CreateComplaintViewModel
    {
        #region Select Lists

        public CommonSelectLists SelectLists { get; set; }

        #endregion

        #region Control properties

        public bool DisableCurrentOwnerSelect { get; set; }

        #endregion

        #region Meta-data

        [Display(Name = "Date Received")]
        [DisplayFormat(DataFormatString = CTS.FormatDateEdit,
            ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Text)]
        public DateTime? DateReceivedDate { get; set; }

        [Display(Name = "Time Received")]
        [DisplayFormat(DataFormatString = CTS.FormatTimeEdit,
            ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Text)]
        public DateTime? DateReceivedTime { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedById { get; set; }

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

        #region Attachments

        public List<IFormFile> Attachments { get; set; }

        #endregion

        #region Assignment

        [Display(Name = "Assigned Office")]
        [Required]
        public Guid? CurrentOfficeId { get; set; }

        [Display(Name = "Assigned Associate")]
        public string CurrentOwnerId { get; set; }

        #endregion
    }
}
