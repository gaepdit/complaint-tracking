using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintTracking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DAL _dal;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            DAL dal)
        {
            _context = context;
            _userManager = userManager;
            _dal = dal;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("PortalHome");
            }

            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return View("PortalHome");
            }

            var myOfficeName = await _dal.GetOfficeName(currentUser.OfficeId);

            // My New Complaints
            var myNewComplaints = await _context.Complaints.AsNoTracking()
                .Where(e => e.CurrentOwnerId == currentUser.Id)
                .Where(e => e.DateCurrentOwnerAccepted == null)
                .Where(e => !e.ComplaintClosed)
                .Where(e => !e.Deleted)
                .OrderByDescending(e => e.DateReceived)
                .Select(e => new HomeComplaintListViewModel(e))
                .ToListAsync();

            // My Open Complaints
            var myOpenComplaints = await _context.Complaints.AsNoTracking()
                .Where(e => e.CurrentOwnerId == currentUser.Id)
                .Where(e => e.DateCurrentOwnerAccepted != null)
                .Where(e => !e.ComplaintClosed)
                .Where(e => !e.Deleted)
                .OrderByDescending(e => e.DateReceived)
                .Select(e => new HomeComplaintListViewModel(e))
                .ToListAsync();

            // Manager-only lists
            IEnumerable<HomeComplaintListViewModel> mgrComplaintsPendingReview = null;
            IEnumerable<HomeComplaintListViewModel> mgrUnassignedComplaints = null;
            IEnumerable<HomeComplaintListViewModel> mgrUnacceptedComplaints = null;

            if (User.IsInRole(CtsRole.Manager.ToString()))
            {
                // Manager: Complaints Pending Review
                mgrComplaintsPendingReview = await _context.Complaints.AsNoTracking()
                    .Where(e => e.ReviewById == currentUser.Id)
                    .Where(e => e.Status == ComplaintStatus.ReviewPending)
                    .Where(e => !e.ComplaintClosed)
                    .Where(e => !e.Deleted)
                    .OrderByDescending(e => e.DateReceived)
                    .Select(e => new HomeComplaintListViewModel(e))
                    .ToListAsync();

                // Manager: Unassigned Complaints
                mgrUnassignedComplaints = await _context.Complaints.AsNoTracking()
                   .Where(e => e.CurrentOfficeId == currentUser.OfficeId)
                   .Where(e => e.CurrentOwnerId == null)
                   .Where(e => !e.ComplaintClosed)
                   .Where(e => !e.Deleted)
                   .OrderByDescending(e => e.DateReceived)
                   .Select(e => new HomeComplaintListViewModel(e))
                   .ToListAsync();

                // Manager: Unaccepted Complaints 
                mgrUnacceptedComplaints = await _context.Complaints.AsNoTracking()
                   .Where(e => e.CurrentOfficeId == currentUser.OfficeId)
                   .Where(e => e.CurrentOwnerId != null)
                   .Where(e => e.DateCurrentOwnerAccepted == null)
                   .Where(e => !e.ComplaintClosed)
                   .Where(e => !e.Deleted)
                   .OrderByDescending(e => e.DateReceived)
                   .Select(e => new HomeComplaintListViewModel(e))
                   .ToListAsync();
            }

            // Master: Unassigned Complaints
            var officesForMaster = await _dal.GetOfficesForMasterAsync(currentUser.Id);
            var unassignedComplaintsForMaster = new Dictionary<Office, List<HomeComplaintListViewModel>>();
            if (officesForMaster != null && officesForMaster.Count() > 0)
            {
                foreach (var item in officesForMaster)
                {
                    var complaints = await _context.Complaints.AsNoTracking()
                        .Where(e => e.CurrentOfficeId == item.Id)
                        .Where(e => e.CurrentOwnerId == null)
                        .Where(e => !e.ComplaintClosed)
                        .Where(e => !e.Deleted)
                        .OrderByDescending(e => e.DateReceived)
                        .Select(e => new HomeComplaintListViewModel(e))
                        .ToListAsync();
                    unassignedComplaintsForMaster.Add(item, complaints);
                }
            }

            var model = new HomeIndexViewModel()
            {
                MyOfficeName = myOfficeName,
                MyNewComplaints = myNewComplaints,
                MyOpenComplaints = myOpenComplaints,
                MgrComplaintsPendingReview = mgrComplaintsPendingReview,
                MgrUnacceptedComplaints = mgrUnacceptedComplaints,
                MgrUnassignedComplaints = mgrUnassignedComplaints,
                MasterUnassignedComplaints = unassignedComplaintsForMaster
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeIndexViewModel model)
        {
            string msg = "Please enter a Complaint ID first.";

            if (model.FindComplaint.HasValue)
            {
                bool includeDeleted = User != null && User.IsInRole(CtsRole.DivisionManager.ToString());

                var complaintExists = await _context.Complaints.AsNoTracking()
                    .AnyAsync(e => e.Id == model.FindComplaint.Value && (includeDeleted || !e.Deleted));

                if (complaintExists)
                {
                    return RedirectToAction("Details", "Complaints", new { id = model.FindComplaint });
                }

                msg = "A Complaint with that ID could not be found.";
            }

            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Warning);
            return View(model);
        }

        // Helpers
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

    }
}
