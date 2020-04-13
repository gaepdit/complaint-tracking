using ComplaintTracking.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class HomeIndexViewModel
    {
        [Display(Name = "Complaint ID")]
        public int? FindComplaint { get; set; }

        // Staff
        public IEnumerable<HomeComplaintListViewModel> MyNewComplaints { get; set; }
        public IEnumerable<HomeComplaintListViewModel> MyOpenComplaints { get; set; }

        // Managers
        public string MyOfficeName { get; set; }
        public IEnumerable<HomeComplaintListViewModel> MgrComplaintsPendingReview { get; set; }
        public IEnumerable<HomeComplaintListViewModel> MgrUnassignedComplaints { get; set; }
        public IEnumerable<HomeComplaintListViewModel> MgrUnacceptedComplaints { get; set; }

        // Masters
        public Dictionary<Office, List<HomeComplaintListViewModel>> MasterUnassignedComplaints { get; set; }
    }
}
