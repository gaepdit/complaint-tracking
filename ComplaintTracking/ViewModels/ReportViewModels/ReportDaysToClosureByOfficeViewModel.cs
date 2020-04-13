using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintTracking.ViewModels
{
    public class ReportDaysToClosureByOfficeViewModel
    {
        public string Title { get; set; }
        public string CurrentAction { get; set; }
        public IEnumerable<OfficeList> Offices { get; set; }

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

        public int TotalComplaints
        {
            get
            {
                if (Offices == null || Offices.Count() == 0)
                {
                    return 0;
                }

                return Offices.Sum(e => e.Complaints != null ? e.Complaints.Count() : 0);
            }
        }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public double TotalAverageDaysToClosure
        {
            get
            {
                if (Offices == null || Offices.Count() == 0)
                {
                    return 0;
                }

                if (TotalComplaints == 0)
                {
                    return 0;
                }

                return Offices.Sum(e => e.TotalDaysToClosure) / Convert.ToDouble(TotalComplaints);
            }
        }

        public class OfficeList
        {
            public OfficeList(Office office)
            {
                Id = office.Id;
                Name = office.Name;
            }

            public Guid Id { get; set; }
            public string Name { get; set; }

            public IEnumerable<ComplaintList> Complaints { get; set; }

            [DisplayFormat(DataFormatString = "{0:N1}")]
            public double AverageDaysToClosure
            {
                get
                {
                    if (Complaints == null || Complaints.Count() == 0)
                    {
                        return 0;
                    }
                    
                    return Complaints.Average(e => e.DaysToClosure);
                }
            }
            public int TotalDaysToClosure
            {
                get
                {
                    if (Complaints == null || Complaints.Count() == 0)
                    {
                        return 0;
                    }
                    
                    return Complaints.Sum(e => e.DaysToClosure);
                }
            }
        }

        public class ComplaintList
        {
            public ComplaintList(Complaint e)
            {
                Id = e.Id;
                DateReceived = e.DateReceived;
                DateComplaintClosed = e.DateComplaintClosed;
            }

            public int Id { get; set; }
            public DateTime DateReceived { get; set; }
            public DateTime? DateComplaintClosed { get; set; }

            public int DaysToClosure
            {
                get
                {
                    return DateComplaintClosed.Value.Date.Subtract(DateReceived.Date).Days;
                }
            }
        }
    }
}
