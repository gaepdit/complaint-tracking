using ComplaintTracking.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class UserIndexViewModel
    {
        public PaginatedList<UserViewModel> Users { get; set; }
        public string CurrentUserId { get; set; }

        #region Search fields

        [StringLength(100)]
        [Display(Name = "Name (first or last)")]
        public string Name { get; set; }

        public Guid? Office { get; set; }
        public SelectList OfficeSelectList { get; set; }

        public UserStatus? Status { get; set; }

        #endregion

        #region Sort order

        public SortBy Sort { get; set; }
        public SortBy NameSortAction { get; set; } = SortBy.NameAsc;
        public SortBy OfficeSortAction { get; set; } = SortBy.OfficeAsc;

        #endregion

        #region Enums

        public enum SortBy
        {
            NameAsc,
            NameDesc,
            OfficeAsc,
            OfficeDesc,
        }

        public enum UserStatus
        {
            // Active,
            Inactive,
            All,
        }

        #endregion
    }
}
