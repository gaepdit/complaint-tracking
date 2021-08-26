using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ReportComplaintsByCountyViewModel
    {
        public string Title { get; set; }
        public string CurrentAction { get; set; }
        public IEnumerable<CountyList> Counties { get; set; }

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

        public class CountyList
        {
            public CountyList(County county)
            {
                Id = county.Id;
                Name = county.Name;
            }

            [Display(Name = "County ID")]
            public int Id { get; }
            public string Name { get; }
            public IEnumerable<ComplaintList> Complaints { get; set; }
        }

        public class ComplaintList
        {
            public ComplaintList(Complaint e)
            {
                Id = e.Id;
                Status = e.Status;
                AssignedTo = e.CurrentOwner?.SortableFullName;
                SourceFacilityName = e.SourceFacilityName;
                DateReceived = e.DateReceived;
            }

            [Display(Name = "Complaint ID")]
            public int Id { get; }

            public ComplaintStatus Status { get; }

            [Display(Name = "AssignedTo")]
            public string AssignedTo { get; }

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
