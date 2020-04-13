using ComplaintTracking.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class ReportStaffListViewModel
    {
        public string Title { get; set; }
        public string CurrentAction { get; set; }
        public IEnumerable<StaffList> Staff { get; set; }

        public class StaffList
        {
            public StaffList(ApplicationUser user)
            {
                Id = user.Id;
                Name = user.FullName;
                LastName = user.LastName;
                Office = user.Office.Name;
            }

            [Display(Name = "Staff ID")]
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Office { get; set; }
        }
    }
}
