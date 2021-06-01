using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintTracking.ViewModels
{
    public class ReportDaysSinceLastActionByStaffViewModel
    {
        public string Title { get; init; }
        public string CurrentAction { get; init; }
        public IEnumerable<StaffList> Staff { get; init; }

        public SelectList OfficeSelectList { get; init; }
        public string Office { get; init; }

        public class StaffList
        {
            public StaffList(ApplicationUser user)
            {
                Id = user.Id;
                Name = user.SortableFullName;
            }

            [Display(Name = "Staff ID")]
            public string Id { get; }

            public string Name { get; }
            public IEnumerable<ComplaintList> Complaints { get; set; }

            [DisplayFormat(DataFormatString = "{0:N1}")]
            public double AverageDaysSinceLastAction =>
                Complaints != null && Complaints.Any() ? Complaints.Average(e => e.DaysSinceLastAction) : 0;
        }

        public class ComplaintList
        {
            [Display(Name = "Complaint ID")]
            public int Id { get; init; }

            [Display(Name = "County of Complaint")]
            public string ComplaintCounty { get; init; }

            [Display(Name = "Source Name")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceFacilityName { get; init; }

            [Display(Name = "Received")]
            [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
            public DateTime DateReceived { get; init; }

            public ComplaintStatus Status { get; init; }

            [Display(Name = "Most Recent Action Date")]
            [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
            public DateTime? LastActionDate { get; init; }

            [Display(Name = "Days Since Last Action")]
            public int DaysSinceLastAction =>
                LastActionDate.HasValue
                    ? DateTime.Today.Date.Subtract(LastActionDate.Value).Days
                    : DateTime.Today.Date.Subtract(DateReceived).Days;
        }
    }
}
