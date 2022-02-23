using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintTracking.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DAL _dal;

        public ReportsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            DAL dal)
        {
            _context = context;
            _userManager = userManager;
            _dal = dal;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ComplaintsAssignedToInactiveUsers(string office)
        {
            var currentUser = await GetCurrentUserAsync();

            if (string.IsNullOrEmpty(office)
                || !Guid.TryParse(office, out var officeId)
                || officeId == Guid.Empty
                || !(await _dal.OfficeExists(officeId)))
            {
                officeId = currentUser.OfficeId ?? Guid.Empty;
            }

            return View("Report", new ReportViewModel
            {
                Title = "Open complaints assigned to inactive users",
                Complaints = await _context.Complaints.AsNoTracking()
                    .Where(e => !e.ComplaintClosed)
                    .Where(e => !e.Deleted)
                    .Where(e => !e.CurrentOwner.Active)
                    .Where(e => e.CurrentOfficeId == officeId)
                    .OrderByDescending(e => e.DateReceived)
                    .Select(e => new ReportViewModel.ComplaintItem(e))
                    .ToListAsync(),
                UseOfficeSelect = true,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Office = officeId.ToString(),
                CurrentAction = nameof(ComplaintsAssignedToInactiveUsers)
            });
        }

        public async Task<IActionResult> UnresolvedComplaints(string office)
        {
            var currentUser = await GetCurrentUserAsync();

            if (string.IsNullOrEmpty(office)
                || !Guid.TryParse(office, out var officeId)
                || officeId == Guid.Empty
                || !await _dal.OfficeExists(officeId))
            {
                officeId = currentUser.OfficeId ?? Guid.Empty;
            }

            return View("Report", new ReportViewModel
            {
                Title = "Unresolved Complaints By Office",
                Complaints = await _context.Complaints.AsNoTracking()
                    .Where(e => !e.ComplaintClosed)
                    .Where(e => !e.Deleted)
                    .Where(e => e.CurrentOfficeId == officeId)
                    .OrderByDescending(e => e.DateReceived)
                    .Select(e => new ReportViewModel.ComplaintItem(e))
                    .ToListAsync(),
                UseOfficeSelect = true,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Office = officeId.ToString(),
                CurrentAction = nameof(UnresolvedComplaints)
            });
        }

        public async Task<IActionResult> UnacceptedComplaints(string office)
        {
            var currentUser = await GetCurrentUserAsync();

            if (string.IsNullOrEmpty(office)
                || !Guid.TryParse(office, out var officeId)
                || officeId == Guid.Empty
                || !await _dal.OfficeExists(officeId))
            {
                officeId = currentUser.OfficeId ?? Guid.Empty;
            }

            return View("Report", new ReportViewModel
            {
                Title = "Not Accepted Complaints By Office",
                Complaints = await _context.Complaints.AsNoTracking()
                    .Where(e => !e.ComplaintClosed)
                    .Where(e => !e.Deleted)
                    .Where(e => e.CurrentOwnerId != null)
                    .Where(e => e.DateCurrentOwnerAccepted == null)
                    .Where(e => e.CurrentOfficeId == officeId)
                    .OrderByDescending(e => e.DateReceived)
                    .Select(e => new ReportViewModel.ComplaintItem(e))
                    .ToListAsync(),
                UseOfficeSelect = true,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Office = officeId.ToString(),
                CurrentAction = nameof(UnacceptedComplaints)
            });
        }

        public async Task<IActionResult> AllComplaintsReceivedByDate(DateTime? selectedDate)
        {
            selectedDate ??= DateTime.Today.AddDays(-1).Date;

            return View("Report", new ReportViewModel
            {
                Title = $"All Complaints Received on {selectedDate:MMMM\u00a0d, yyyy}",
                ComplaintsExpanded = await _context.Complaints.AsNoTracking()
                    .Where(e => !e.Deleted)
                    .Where(e => e.DateReceived.Date == selectedDate)
                    .OrderByDescending(e => e.DateReceived)
                    .Include(e => e.PrimaryConcern)
                    .Include(e => e.SecondaryConcern)
                    .Include(e => e.CurrentOffice)
                    .Include(e => e.CurrentOwner)
                    .Include(e => e.ComplaintCounty)
                    .Select(e => new ReportViewModel.ExpandedComplaintItem(e))
                    .ToListAsync(),
                UseDate = true,
                SelectedDate = selectedDate,
                CurrentAction = nameof(AllComplaintsReceivedByDate)
            });
        }

        public async Task<IActionResult> DaysToClosureByStaff(DateTime? beginDate, DateTime? endDate, string office)
        {
            var currentUser = await GetCurrentUserAsync();

            if (string.IsNullOrEmpty(office)
                || !Guid.TryParse(office, out var officeId)
                || officeId == Guid.Empty
                || !await _dal.OfficeExists(officeId))
            {
                officeId = currentUser.OfficeId ?? Guid.Empty;
            }

            var today = DateTime.Today;
            beginDate ??= new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            endDate ??= new DateTime(today.Year, today.Month, 1).AddDays(-1);

            List<ReportDaysToClosureByStaffViewModel.StaffList> staffList = null;

            if (endDate < beginDate)
            {
                const string msg = "The beginning date must precede the end date.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                var officeStaff = _context.Users.AsNoTracking()
                    .Where(e => e.OfficeId == officeId);

                staffList = await officeStaff
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .Select(e => new ReportDaysToClosureByStaffViewModel.StaffList(e))
                    .ToListAsync();

                foreach (var user in staffList)
                {
                    user.Complaints = await _context.Complaints.AsNoTracking()
                        .Include(e => e.ComplaintCounty)
                        .Where(e => e.ComplaintClosed)
                        .Where(e => !e.Deleted)
                        .Where(e => e.CurrentOwnerId == user.Id)
                        .Where(e => e.DateComplaintClosed >= beginDate)
                        .Where(e => e.DateComplaintClosed <= endDate)
                        .OrderBy(e => e.DateComplaintClosed)
                        .Select(e => new ReportDaysToClosureByStaffViewModel.ComplaintList(e))
                        .ToListAsync();
                }
            }

            return View("DaysToClosureByStaff", new ReportDaysToClosureByStaffViewModel
            {
                Title = "Days To Closure By Staff",
                Staff = staffList,
                BeginDate = beginDate,
                EndDate = endDate,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Office = officeId.ToString(),
                CurrentAction = nameof(DaysToClosureByStaff)
            });
        }

        public async Task<IActionResult> DaysToFollowUpByStaff(DateTime? beginDate, DateTime? endDate, string office)
        {
            var currentUser = await GetCurrentUserAsync();

            if (string.IsNullOrEmpty(office)
                || !Guid.TryParse(office, out var officeId)
                || officeId == Guid.Empty
                || !await _dal.OfficeExists(officeId))
            {
                officeId = currentUser.OfficeId ?? Guid.Empty;
            }

            var today = DateTime.Today;
            beginDate ??= new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            endDate ??= new DateTime(today.Year, today.Month, 1).AddDays(-1);

            List<ReportDaysToFollowUpByStaffViewModel.StaffList> staffList = null;

            if (endDate < beginDate)
            {
                const string msg = "The beginning date must precede the end date.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                var officeStaff = _context.Users.AsNoTracking()
                    .Where(e => e.OfficeId == officeId);

                staffList = await officeStaff
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .Select(e => new ReportDaysToFollowUpByStaffViewModel.StaffList(e))
                    .ToListAsync();

                foreach (var user in staffList)
                {
                    var query = $@"SELECT c.Id, l.Name AS ComplaintCounty, c.SourceFacilityName, 
                        convert(DATE, c.DateReceived) AS DateReceived, a.MinActionDate
                        FROM Complaints c
                        INNER JOIN LookupCounties l ON c.ComplaintCountyId = l.Id
                        INNER JOIN
                        (SELECT c.Id, convert(DATE, min(a.ActionDate)) AS MinActionDate
                        FROM Complaints c
                        INNER JOIN ComplaintActions a ON c.Id = a.ComplaintId
                        GROUP BY c.Id) a ON c.Id = a.Id
                        WHERE c.Deleted = 0
                        AND c.CurrentOwnerId = '{user.Id}'
                        AND a.MinActionDate BETWEEN '{beginDate:d}' AND '{endDate:d}'
                        ORDER BY c.Id";

                    user.Complaints = await DataSqlHelper
                        .ExecSQL<ReportDaysToFollowUpByStaffViewModel.ComplaintList>(query, _context);
                }
            }

            return View("DaysToFollowUpByStaff", new ReportDaysToFollowUpByStaffViewModel
            {
                Title = "Days To Follow Up By Staff",
                Staff = staffList,
                BeginDate = beginDate,
                EndDate = endDate,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Office = officeId.ToString(),
                CurrentAction = nameof(DaysToFollowUpByStaff)
            });
        }

        public async Task<IActionResult> DaysSinceLastAction(string office, int threshold = 30)
        {

            if (string.IsNullOrEmpty(office)
                || !Guid.TryParse(office, out var officeId)
                || officeId == Guid.Empty
                || !await _dal.OfficeExists(officeId))
            {
                officeId = (await GetCurrentUserAsync()).OfficeId ??
                    (await _context.LookupOffices.FirstOrDefaultAsync())!.Id;
            }

            var staffList = await _context.Users.AsNoTracking()
                .Where(e => e.OfficeId == officeId)
                .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
                .Select(e => new ReportDaysSinceLastActionViewModel.StaffList(e))
                .ToListAsync();

            foreach (var user in staffList)
            {
                var query =
                    $@"SELECT c.Id, l.Name AS ComplaintCounty, c.SourceFacilityName,
                           convert(date, c.DateReceived) AS DateReceived, c.Status, a.LastActionDate
                    FROM Complaints c
                    left JOIN LookupCounties l ON c.ComplaintCountyId = l.Id
                    INNER JOIN
                    (SELECT c.Id, convert(date, max(a.ActionDate)) AS LastActionDate
                     FROM Complaints c
                     left JOIN ComplaintActions a ON c.Id = a.ComplaintId
                     GROUP BY c.Id) a ON c.Id = a.Id
                    WHERE c.Deleted = 0 and c.ComplaintClosed = 0 AND c.CurrentOwnerId = '{user.Id}'
                    ORDER BY c.Id desc";

                user.Complaints = (await DataSqlHelper
                    .ExecSQL<ReportDaysSinceLastActionViewModel.ComplaintList>(query, _context))
                    .Where(e => e.DaysSinceLastAction >= threshold);
            }

            return View("DaysSinceLastAction", new ReportDaysSinceLastActionViewModel
            {
                Title = "Days Since Last Action",
                Staff = staffList,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Office = officeId.ToString(),
                Threshold = threshold,
                CurrentAction = nameof(DaysSinceLastAction)
            });
        }

        public async Task<IActionResult> DaysToClosureByOffice(DateTime? beginDate, DateTime? endDate)
        {
            var today = DateTime.Today;
            endDate ??= new DateTime(today.Year - Convert.ToInt32(today.Month < 7), 6, 30);
            beginDate ??= endDate.Value.AddYears(-1).AddDays(1);

            List<ReportDaysToClosureByOfficeViewModel.OfficeList> officeList = null;

            if (endDate < beginDate)
            {
                const string msg = "The beginning date must precede the end date.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                officeList = await _context.LookupOffices.AsNoTracking()
                    .Where(e => e.Active)
                    .OrderBy(e => e.Name)
                    .Select(e => new ReportDaysToClosureByOfficeViewModel.OfficeList(e))
                    .ToListAsync();

                foreach (var office in officeList)
                {
                    office.Complaints = await _context.Complaints.AsNoTracking()
                        .Where(e => e.ComplaintClosed)
                        .Where(e => !e.Deleted)
                        .Where(e => e.CurrentOfficeId == office.Id)
                        .Where(e => e.DateComplaintClosed >= beginDate)
                        .Where(e => e.DateComplaintClosed <= endDate)
                        .OrderBy(e => e.DateComplaintClosed)
                        .Select(e => new ReportDaysToClosureByOfficeViewModel.ComplaintList(e))
                        .ToListAsync();
                }
            }

            return View("DaysToClosureByOffice", new ReportDaysToClosureByOfficeViewModel
            {
                Title = "Days To Closure By Office",
                Offices = officeList,
                BeginDate = beginDate,
                EndDate = endDate,
                CurrentAction = nameof(DaysToClosureByOffice)
            });
        }

        public async Task<IActionResult> ComplaintsByStaff(DateTime? beginDate, DateTime? endDate, string office)
        {
            var currentUser = await GetCurrentUserAsync();

            if (string.IsNullOrEmpty(office)
                || !Guid.TryParse(office, out var officeId)
                || officeId == Guid.Empty
                || !await _dal.OfficeExists(officeId))
            {
                officeId = currentUser.OfficeId ?? Guid.Empty;
            }

            var today = DateTime.Today;
            beginDate ??= new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            endDate ??= new DateTime(today.Year, today.Month, 1).AddDays(-1);

            List<ReportComplaintsByStaffViewModel.StaffList> staffList = null;

            if (endDate < beginDate)
            {
                const string msg = "The beginning date must precede the end date.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                var officeStaff = _context.Users.AsNoTracking()
                    .Where(e => e.OfficeId == officeId);

                staffList = await officeStaff
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .Select(e => new ReportComplaintsByStaffViewModel.StaffList(e))
                    .ToListAsync();

                foreach (var user in staffList)
                {
                    user.Complaints = await _context.Complaints.AsNoTracking()
                        .Include(e => e.ComplaintCounty)
                        .Where(e => !e.Deleted)
                        .Where(e => e.CurrentOwnerId == user.Id)
                        .Where(e => e.DateReceived >= beginDate)
                        .Where(e => e.DateReceived <= endDate)
                        .OrderBy(e => e.DateReceived)
                        .Select(e => new ReportComplaintsByStaffViewModel.ComplaintList(e))
                        .ToListAsync();
                }
            }

            return View("ComplaintsByStaff", new ReportComplaintsByStaffViewModel
            {
                Title = "All Complaints By Staff",
                Staff = staffList,
                BeginDate = beginDate,
                EndDate = endDate,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Office = officeId.ToString(),
                CurrentAction = nameof(ComplaintsByStaff)
            });
        }

        public async Task<IActionResult> ComplaintsByCounty(DateTime? beginDate, DateTime? endDate)
        {
            var today = DateTime.Today;
            beginDate ??= new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            endDate ??= new DateTime(today.Year, today.Month, 1).AddDays(-1);

            List<ReportComplaintsByCountyViewModel.CountyList> countyList = null;

            if (endDate < beginDate)
            {
                const string msg = "The beginning date must precede the end date.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                countyList = await _context.LookupCounties.AsNoTracking()
                    .OrderBy(e => e.Name)
                    .Select(e => new ReportComplaintsByCountyViewModel.CountyList(e))
                    .ToListAsync();

                foreach (var county in countyList)
                {
                    county.Complaints = await _context.Complaints.AsNoTracking()
                        .Include(e => e.CurrentOwner)
                        .Where(e => !e.Deleted)
                        .Where(e => e.ComplaintCountyId == county.Id)
                        .Where(e => e.DateReceived >= beginDate)
                        .Where(e => e.DateReceived <= endDate)
                        .OrderBy(e => e.DateReceived)
                        .Select(e => new ReportComplaintsByCountyViewModel.ComplaintList(e))
                        .ToListAsync();
                }
            }

            return View("ComplaintsByCounty", new ReportComplaintsByCountyViewModel
            {
                Title = "All Complaints By County",
                Counties = countyList,
                BeginDate = beginDate,
                EndDate = endDate,
                CurrentAction = nameof(ComplaintsByCounty)
            });
        }

        public async Task<IActionResult> UsersAssignedToInactiveOffices()
        {
            var staff = _context.Users.AsNoTracking()
                .Include(e => e.Office)
                .Where(e => e.Active && !e.Office.Active)
                .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
                .Select(e => new ReportStaffListViewModel.StaffList(e));

            return View("StaffListReport", new ReportStaffListViewModel
            {
                Title = "Active users currently assigned to inactive offices",
                Staff = await staff.ToListAsync()
            });
        }

        public async Task<IActionResult> UnconfirmedUserAccounts()
        {
            var staff = _context.Users.AsNoTracking()
                .Include(e => e.Office)
                .Where(e => e.Active && !e.EmailConfirmed)
                .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
                .Select(e => new ReportStaffListViewModel.StaffList(e));

            return View("StaffListReport", new ReportStaffListViewModel
            {
                Title = "Active users who have not confirmed their account",
                Staff = await staff.ToListAsync()
            });
        }

        // Helpers
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
