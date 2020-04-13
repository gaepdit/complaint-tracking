using ComplaintTracking.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class SearchComplaintsViewModel
    {
        public PaginatedList<ComplaintListViewModel> Complaints { get; set; }

        public bool IsForPublic { get; set; }
        public bool IncludeDeleted { get; set; }

        #region Search fields

        // Status
        [Display(Name = "Complaint Status")]
        public SearchComplaintStatus? ComplaintStatus { get; set; }

        [Display(Name = "Deletion Status")]
        public SearchDeleteStatus? DeleteStatus { get; set; }

        // Received
        [Display(Name = "Date Received From")]
        [DataType(DataType.Text)]
        public DateTime? DateReceivedFrom { get; set; }
        [Display(Name = "Through")]
        [DataType(DataType.Text)]
        public DateTime? DateReceivedTo { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedById { get; set; }
        public SelectList AllUsersSelectList { get; set; }

        [Display(Name = "Date Complaint Closed From")]
        [DataType(DataType.Text)]
        public DateTime? DateComplaintClosedFrom { get; set; }
        [Display(Name = "Through")]
        [DataType(DataType.Text)]
        public DateTime? DateComplaintClosedTo { get; set; }

        // Caller
        [Display(Name = "Caller Name")]
        [StringLength(100)]
        public string CallerName { get; set; }

        [Display(Name = "Represents")]
        [StringLength(25)]
        public string CallerRepresents { get; set; }

        // Complaint
        [Display(Name = "Nature of Complaint")]
        [StringLength(100)]
        public string ComplaintNature { get; set; }

        [Display(Name = "Location of Complaint")]
        [StringLength(100)]
        public string ComplaintLocation { get; set; }

        [Display(Name = "Direction to Complaint")]
        [StringLength(100)]
        public string ComplaintDirections { get; set; }

        [Display(Name = "City of Complaint")]
        [StringLength(50)]
        public string ComplaintCity { get; set; }

        [Display(Name = "County of Complaint")]
        public int? ComplaintCountyId { get; set; }
        public SelectList CountySelectList { get; set; }

        [Display(Name = "Concern")]
        public Guid? ConcernId { get; set; }
        public SelectList ConcernSelectList { get; set; }

        // Source
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

        [Display(Name = "City")]
        [StringLength(50)]
        public string SourceCity { get; set; }

        [Display(Name = "State")]
        public int? SourceStateId { get; set; }
        public SelectList StateSelectList { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(10)]
        public string SourcePostalCode { get; set; }

        // Assignment
        [Display(Name = "Office")]
        public Guid? Office { get; set; }
        public SelectList OfficeSelectList { get; set; }

        [Display(Name = "Current Assigned Associate")]
        public string Owner { get; set; }
        public SelectList OwnerSelectList { get; set; }

        #endregion

        #region Sort order

        public SortBy Sort { get; set; }

        public SortBy IdSortAction { get; set; } = SortBy.IdDesc;
        public SortBy ReceivedDateSortAction { get; set; } = SortBy.ReceivedDateDesc;
        public SortBy StatusSortAction { get; set; } = SortBy.StatusAsc;

        #endregion

        #region Enums

        public enum SortBy
        {
            IdAsc,
            IdDesc,
            ReceivedDateAsc,
            ReceivedDateDesc,
            StatusAsc,
            StatusDesc,
        }

        public enum SearchComplaintStatus
        {
            New,
            [Display(Name = "Under Investigation")] UnderInvestigation,
            [Display(Name = "Review Pending")] ReviewPending,
            [Display(Name = "Approved/Closed")] Closed,
            [Display(Name = "All Open")] Open,
        }

        public enum SearchDeleteStatus
        {
            Deleted,
            All,
        }

        #endregion
    }
}
