using ComplaintTracking.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class SearchComplaintActionsViewModel
    {
        public PaginatedList<ComplaintActionsListViewModel> ComplaintActions { get; set; }

        public bool IncludeDeleted { get; set; }

        #region Search fields

        [Display(Name = "Action Date From")]
        [DataType(DataType.Text)]
        public DateTime? ActionDateFrom { get; set; }

        [Display(Name = "Through")]
        [DataType(DataType.Text)]
        public DateTime? ActionDateTo { get; set; }

        [Display(Name = "Action Type")]
        public Guid? ActionType { get; set; }

        public SelectList ActionTypesSelectList { get; set; }

        [StringLength(100)]
        public string Investigator { get; set; }

        [Display(Name = "Date Entered From")]
        [DataType(DataType.Text)]
        public DateTime? DateEnteredFrom { get; set; }

        [Display(Name = "Through")]
        [DataType(DataType.Text)]
        public DateTime? DateEnteredTo { get; set; }

        [Display(Name = "Entered By")]
        public string EnteredBy { get; set; }

        public SelectList AllUsersSelectList { get; set; }

        [StringLength(100)]
        public string Comments { get; set; }

        [Display(Name = "Concern")]
        public Guid? ConcernId { get; set; }

        public SelectList ConcernSelectList { get; set; }

        [Display(Name = "Deletion Status")]
        public SearchDeleteStatus? DeleteStatus { get; set; }

        #endregion

        #region Sort order

        public SortBy Sort { get; set; }
        public SortBy TypeSortAction { get; set; } = SortBy.ActionTypeAsc;
        public SortBy DateSortAction { get; set; } = SortBy.ActionDateDesc;
        public SortBy ComplaintIdSortAction { get; set; } = SortBy.ComplaintIdAsc;

        #endregion

        #region Enums

        public enum SortBy
        {
            ActionTypeAsc,
            ActionTypeDesc,
            ActionDateAsc,
            ActionDateDesc,
            ComplaintIdAsc,
            ComplaintIdDesc,
        }

        public enum SearchDeleteStatus
        {
            // [Display(Name = "Not Deleted")] Extant,
            Deleted,
            All,
        }

        #endregion
    }
}
