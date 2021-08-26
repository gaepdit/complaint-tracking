using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ReportViewModel
    {
        public string Title { get; set; }
        public string CurrentAction { get; set; }

        public IEnumerable<ComplaintItem> Complaints { get; set; }
        public IEnumerable<ExpandedComplaintItem> ComplaintsExpanded { get; set; }

        public bool UseOfficeSelect { get; set; } = false;
        public SelectList OfficeSelectList { get; set; }
        public string Office { get; set; }

        public bool UseDate { get; set; } = false;

        [DisplayFormat(DataFormatString = CTS.FormatDateEdit,
            ApplyFormatInEditMode = true)]
        [DataType(DataType.Text)]
        [Display(Name = "Select Date")]
        public DateTime? SelectedDate { get; set; }

        public class ComplaintItem
        {
            public ComplaintItem(Complaint e)
            {
                Id = e.Id;
                DateReceived = e.DateReceived;
                SourceFacilityId = e.SourceFacilityId;
                SourceFacilityName = e.SourceFacilityName;
                SourceCity = e.SourceCity;
                SourceStateName = e.SourceState?.Name;
                Status = e.Status;
            }

            [Display(Name = "Complaint ID")]
            public int Id { get; set; }

            [Display(Name = "Date Received")]
            [DisplayFormat(DataFormatString = CTS.FormatDateShortDisplay)]
            public DateTime DateReceived { get; set; }

            public ComplaintStatus Status { get; set; }

            [Display(Name = "Source")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceFacilityName { get; set; }

            public string SourceCity { get; set; }

            public string SourceStateName { get; set; }

            [Display(Name = "Location")]
            [DisplayFormat(ConvertEmptyStringToNull = true)]
            public string SourceLocation => 
                StringFunctions.ConcatNonEmptyStrings(new[] { SourceCity, SourceStateName }, ", ");

            [Display(Name = "ID")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceFacilityId { get; set; }
        }

        public class ExpandedComplaintItem
        {
            public ExpandedComplaintItem(Complaint e)
            {
                Id = e.Id;
                DateReceived = e.DateReceived;
                Status = e.Status;
                PrimaryConcernName = e.PrimaryConcern?.Name;
                SecondaryConcernName = e.SecondaryConcern?.Name;
                SourceFacilityId = e.SourceFacilityId;
                SourceFacilityName = e.SourceFacilityName;
                SourceCity = e.SourceCity;
                CountyName = e.ComplaintCounty?.Name;
                Nature = e.ComplaintNature;
                Location = e.ComplaintLocation;
                AssignedOffice = e.CurrentOffice?.Name;
                AssignedStaff = e.CurrentOwner?.FullName;
            }

            [Display(Name = "Complaint ID")]
            public int Id { get; set; }

            [Display(Name = "Complaint Received")]
            [DisplayFormat(DataFormatString = CTS.FormatDateTimeShortDisplay)]
            public DateTime DateReceived { get; set; }

            public ComplaintStatus Status { get; set; }

            [Display(Name = "Primary Concern")]
            [DisplayFormat(
                NullDisplayText = CTS.NoneDisplayText,
                ConvertEmptyStringToNull = true)]
            public string PrimaryConcernName { get; set; }

            [Display(Name = "Secondary Concern")]
            [DisplayFormat(
                NullDisplayText = CTS.NoneDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SecondaryConcernName { get; set; }

            [Display(Name = "ID")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceFacilityId { get; set; }

            [Display(Name = "Source")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceFacilityName { get; set; }

            [Display(Name = "Source City")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string SourceCity { get; set; }

            [Display(Name = "County")]
            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string CountyName { get; set; }

            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string Nature { get; set; }

            [DisplayFormat(
                NullDisplayText = CTS.NotEnteredDisplayText,
                ConvertEmptyStringToNull = true)]
            public string Location { get; set; }

            [Display(Name = "Assigned To")]
            public string AssignedOffice { get; set; }

            [Display(Name = "Assigned Staff")]
            [DisplayFormat(
                NullDisplayText = CTS.SelectUserMasterText,
                ConvertEmptyStringToNull = true)]
            public string AssignedStaff { get; set; }
        }
    }
}
