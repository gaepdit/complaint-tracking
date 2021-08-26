using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ReportComplaintsByStaffViewModel
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
            public string Id { get; }
            public string Name { get; }
            public IEnumerable<ComplaintList> Complaints { get; set; }
        }

        public class ComplaintList
        {
            public ComplaintList(Complaint e)
            {
                Id = e.Id;
                Status = e.Status;
                ComplaintCounty = e.ComplaintCounty?.Name;
                SourceFacilityName = e.SourceFacilityName;
                DateReceived = e.DateReceived;
            }

            [Display(Name = "Complaint ID")]
            public int Id { get; }

            public ComplaintStatus Status { get; }

            [Display(Name = "County of Complaint")]
            public string ComplaintCounty { get; }

            [Display(Name = "Source Name")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceFacilityName { get; }

            [Display(Name = "Received")]
            [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
            public DateTime DateReceived { get; }
        }
    }
}
