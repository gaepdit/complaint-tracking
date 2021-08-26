using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintTracking.ViewModels
{
    public class ReportDaysToFollowUpByStaffViewModel
    {
        public string Title { get; set; }
        public string CurrentAction { get; set; }
        public IEnumerable<StaffList> Staff { get; set; }

        public SelectList OfficeSelectList { get; set; }
        public string Office { get; set; }

        [DisplayFormat(DataFormatString = CTS.FormatDateEdit,
            ApplyFormatInEditMode = true)]
        [DataType(DataType.Text)]
        [Display(Name = "Date From")]
        public DateTime? BeginDate { get; set; }

        [DisplayFormat(DataFormatString = CTS.FormatDateEdit,
            ApplyFormatInEditMode = true)]
        [DataType(DataType.Text)]
        [Display(Name = "Through")]
        public DateTime? EndDate { get; set; }

        public class StaffList
        {
            public StaffList(ApplicationUser user)
            {
                Id = user.Id;
                Name = user.SortableFullName;
            }

            [Display(Name = "Staff ID")]
            public string Id { get; set; }
            public string Name { get; set; }
            public IEnumerable<ComplaintList> Complaints { get; set; }

            [DisplayFormat(DataFormatString = "{0:N1}")]
            public double AverageDaysToFollowUp => 
                Complaints != null && Complaints.Any()
                    ? Complaints.Average(e => e.DaysToFollowUp) 
                    : 0;
        }

        public class ComplaintList
        {
            [Display(Name = "Complaint ID")]
            public int Id { get; set; }

            [Display(Name = "County of Complaint")]
            public string ComplaintCounty { get; set; }

            [Display(Name = "Source Name")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceFacilityName { get; set; }

            [Display(Name = "Received")]
            [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
            public DateTime DateReceived { get; set; }

            [Display(Name = "Earliest Investigation Date")]
            [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
            public DateTime MinActionDate { get; set; }

            [Display(Name = "Days to Follow Up")]
            public int DaysToFollowUp => MinActionDate.Date.Subtract(DateReceived.Date).Days;
        }
    }
}
