using ComplaintTracking.Data;
using ComplaintTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class Complaint : IAuditable
    {
        #region Constructors

        public Complaint() { }

        public Complaint(CreateComplaintViewModel m)
        {
            // Received
            DateReceived = m.DateReceivedDate?.Date ?? DateTime.Today;
            if (m.DateReceivedTime.HasValue)
            {
                DateReceived = DateReceived.Add(m.DateReceivedTime.Value.TimeOfDay);
            }
            ReceivedById = m.ReceivedById;
            // Caller
            CallerName = m.CallerName;
            CallerRepresents = m.CallerRepresents;
            CallerStreet = m.CallerStreet;
            CallerStreet2 = m.CallerStreet2;
            CallerCity = m.CallerCity;
            CallerStateId = m.CallerStateId;
            CallerPostalCode = m.CallerPostalCode;
            CallerPhoneNumber = m.CallerPhoneNumber;
            CallerPhoneType = m.CallerPhoneType;
            CallerSecondaryPhoneNumber = m.CallerSecondaryPhoneNumber;
            CallerSecondaryPhoneType = m.CallerSecondaryPhoneType;
            CallerTertiaryPhoneNumber = m.CallerTertiaryPhoneNumber;
            CallerTertiaryPhoneType = m.CallerTertiaryPhoneType;
            CallerEmail = m.CallerEmail;
            // Complaint
            ComplaintNature = m.ComplaintNature;
            ComplaintLocation = m.ComplaintLocation;
            ComplaintDirections = m.ComplaintDirections;
            ComplaintCity = m.ComplaintCity;
            ComplaintCountyId = m.ComplaintCountyId;
            PrimaryConcernId = m.PrimaryConcernId;
            SecondaryConcernId = m.SecondaryConcernId;
            // Source
            SourceFacilityId = m.SourceFacilityId;
            SourceContactName = m.SourceContactName;
            SourceFacilityName = m.SourceFacilityName;
            SourceStreet = m.SourceStreet;
            SourceStreet2 = m.SourceStreet2;
            SourceCity = m.SourceCity;
            SourceStateId = m.SourceStateId;
            SourcePostalCode = m.SourcePostalCode;
            SourcePhoneNumber = m.SourcePhoneNumber;
            SourcePhoneType = m.SourcePhoneType;
            SourceSecondaryPhoneNumber = m.SourceSecondaryPhoneNumber;
            SourceSecondaryPhoneType = m.SourceSecondaryPhoneType;
            SourceTertiaryPhoneNumber = m.SourceTertiaryPhoneNumber;
            SourceTertiaryPhoneType = m.SourceTertiaryPhoneType;
            SourceEmail = m.SourceEmail;
            // Assignment
            CurrentOfficeId = m.CurrentOfficeId.Value;
            CurrentOwnerId = m.CurrentOwnerId;
        }

        #endregion

        public int Id { get; set; }

        #region Meta-data

        [DefaultValue(ComplaintStatus.New)]
        public ComplaintStatus Status { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime DateEntered { get; set; }

        [Display(Name = "Entered By")]
        public ApplicationUser EnteredBy { get; set; }
        public string EnteredById { get; set; }

        [Display(Name = "Date Received")]
        public DateTime DateReceived { get; set; }

        [Display(Name = "Received By")]
        public virtual ApplicationUser ReceivedBy { get; set; }
        public string ReceivedById { get; set; }

        #endregion

        #region Caller

        [Display(Name = "Name")]
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
        public virtual State CallerState { get; set; }
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
        [EmailAddress]
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
        public virtual County ComplaintCounty { get; set; }
        public int? ComplaintCountyId { get; set; }

        [Display(Name = "Primary Concerns")]
        public Concern PrimaryConcern { get; set; }
        public Guid? PrimaryConcernId { get; set; }

        [Display(Name = "Secondary Concerns")]
        public Concern SecondaryConcern { get; set; }
        public Guid? SecondaryConcernId { get; set; }

        #endregion

        #region Source

        [Display(Name = "Facility ID Number")]
        [StringLength(50)]
        public string SourceFacilityId { get; set; }

        [Display(Name = "Source Name")]
        [StringLength(100)]
        public string SourceFacilityName { get; set; }

        [Display(Name = "Source Contact")]
        [StringLength(100)]
        public string SourceContactName { get; set; }

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
        public State SourceState { get; set; }
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

        #region Assignment/History

        [Display(Name = "Current Assigned Office")]
        public virtual Office CurrentOffice { get; set; }
        public Guid CurrentOfficeId { get; set; }

        [Display(Name = "Current Assigned Associate")]
        public virtual ApplicationUser CurrentOwner { get; set; }
        public string CurrentOwnerId { get; set; }

        [Display(Name = "Date Assigned")]
        public DateTime? DateCurrentOwnerAssigned { get; set; }

        public Guid? CurrentAssignmentTransitionId { get; set; }

        [Display(Name = "Date Accepted")]
        public DateTime? DateCurrentOwnerAccepted { get; set; }

        public List<ComplaintTransition> ComplaintTransitions { get; set; }

        #endregion

        #region Actions

        public List<ComplaintAction> ComplaintActions { get; set; }

        #endregion

        #region Review/Closure

        [Display(Name = "Review By")]
        public ApplicationUser ReviewBy { get; set; }
        public string ReviewById { get; set; }

        [Display(Name = "Review Comments")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string ReviewComments { get; set; }

        [Display(Name = "Approved/Closed")]
        public bool ComplaintClosed { get; set; }

        [Display(Name = "Date Complaint Closed")]
        public DateTime? DateComplaintClosed { get; set; }

        #endregion

        #region Deletion

        public bool Deleted { get; set; }

        [Display(Name = "Deleted By")]
        public ApplicationUser DeletedBy { get; set; }
        public string DeletedById { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DateDeleted { get; set; }

        [Display(Name = "Deletion Comments")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string DeleteComments { get; set; }

        #endregion

        #region Attachments

        public virtual ICollection<Attachment> Attachments { get; set; }

        #endregion
    }

    #region enums

    public enum PhoneType
    {
        Cell = 0,
        Fax = 1,
        Home = 2,
        Office = 3
    }

    public enum ComplaintStatus
    {
        //
        // Summary:
        //     Represents a new complaint that has not been accepted.
        New = 0,

        //
        // Summary:
        //     Represents a new complaint that has been accepted.
        [Display(Name = "Under Investigation")]
        UnderInvestigation = 1,

        //
        // Summary:
        //     Represents a complaint that has been submitted for review.
        [Display(Name = "Review Pending")] 
        ReviewPending = 2,

        //
        // Summary:
        //     Represents a complaint that has been approved by a reviewer.
        [Display(Name = "Approved/Closed")] 
        Closed = 3,

        //
        // Summary:
        //     Represents a complaint that has been administratively closed (e.g., by EPD-IT).
        [Display(Name = "Administratively Closed")]
        AdministrativelyClosed = 4,
    }

    #endregion
}
