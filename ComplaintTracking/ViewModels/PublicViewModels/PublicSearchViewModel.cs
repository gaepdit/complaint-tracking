using ComplaintTracking.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class PublicSearchViewModel
    {
        #region Complaint ID Search

        [Display(Name = "Complaint ID")]
        public int? FindComplaint { get; set; }
        
        #endregion

        #region Details Search

        public PaginatedList<PublicSearchResultsViewModel> Complaints { get; set; }

        // Dates

        [Display(Name = "From")]
        [DataType(DataType.Text)]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Through")]
        [DataType(DataType.Text)]
        public DateTime? DateTo { get; set; }

        // Complaint

        [Display(Name = "Description of Complaint")]
        [StringLength(100)]
        public string Nature { get; set; }

        [Display(Name = "Type of Complaint")]
        public Guid? TypeId { get; set; }
        public SelectList ConcernSelectList { get; set; }

        // Source

        [Display(Name = "Source (origin of or entity associated with the incident)")]
        [StringLength(100)]
        public string SourceName { get; set; }

        // Location

        [Display(Name = "County")]
        public int? CountyId { get; set; }
        public SelectList CountySelectList { get; set; }

        [Display(Name = "Street")]
        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }
        public SelectList StateSelectList { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        #endregion

        #region Sort order

        public SortBy Sort { get; set; }

        public SortBy IdSortAction { get; set; } = SortBy.IdDesc;
        public SortBy ReceivedDateSortAction { get; set; } = SortBy.ReceivedDateDesc;

        #endregion

        #region Enums

        public enum SortBy
        {
            IdAsc,
            IdDesc,
            ReceivedDateAsc,
            ReceivedDateDesc,
        }

        #endregion
    }
}
