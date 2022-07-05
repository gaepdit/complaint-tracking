using ComplaintTracking.AlertMessages;
using ComplaintTracking.Generic;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ComplaintTracking.ViewModels.SearchComplaintActionsViewModel;

namespace ComplaintTracking.Controllers
{
    public class ComplaintActionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DAL _dal;

        public ComplaintActionsController(
            UserManager<ApplicationUser> userManager,
            DAL dal)
        {
            _userManager = userManager;
            _dal = dal;
        }

        public async Task<IActionResult> Index(
            int page = 1,
            string submit = null,
            SortBy sort = SortBy.ActionDateDesc,
            DateTime? ActionDateFrom = null,
            DateTime? ActionDateTo = null,
            Guid? ActionType = null,
            string Investigator = null,
            DateTime? DateEnteredFrom = null,
            DateTime? DateEnteredTo = null,
            string EnteredBy = null,
            string Comments = null,
            Guid? ConcernId = null,
            SearchDeleteStatus? deleteStatus = null
        )
        {
            var currentUser = await GetCurrentUserAsync();
            var includeDeleted = currentUser != null && User.IsInRole(CtsRole.DivisionManager.ToString());
            if (!includeDeleted) deleteStatus = null;

            // ViewModel
            var model = new SearchComplaintActionsViewModel
            {
                IncludeDeleted = includeDeleted,
                ActionDateFrom = ActionDateFrom,
                ActionDateTo = ActionDateTo,
                ActionType = ActionType,
                Investigator = Investigator,
                DateEnteredFrom = DateEnteredFrom,
                DateEnteredTo = DateEnteredTo,
                EnteredBy = EnteredBy,
                Comments = Comments,
                ConcernId = ConcernId,
                DeleteStatus = deleteStatus,
                Sort = sort,
                AllUsersSelectList = await _dal.GetAllUsersSelectListAsync(true),
                ActionTypesSelectList = await _dal.GetActionTypesSelectListAsync(),
                ConcernSelectList = await _dal.GetAreasOfConcernSelectListAsync(),
            };

            if (string.IsNullOrEmpty(submit))
            {
                return View(model);
            }

            string msg = null;

            if (ActionDateFrom.HasValue && ActionDateTo.HasValue
                && ActionDateFrom.Value > ActionDateTo.Value)
            {
                msg += "The beginning action date must precede the end date. ";
            }

            if (DateEnteredFrom.HasValue && DateEnteredTo.HasValue
                && DateEnteredFrom.Value > DateEnteredTo.Value)
            {
                msg += "The beginning date entered must precede the end date. ";
            }

            if (msg != null)
            {
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                // Search
                var complaintActions = _dal.SearchComplaintActions(
                 sort,
                 ActionDateFrom,
                 ActionDateTo,
                 ActionType,
                 Investigator,
                 DateEnteredFrom,
                 DateEnteredTo,
                 EnteredBy,
                 Comments,
                 ConcernId,
                 deleteStatus);

                // Sorters
                switch (sort)
                {
                    case SortBy.ActionDateDesc:
                        model.DateSortAction = SortBy.ActionDateAsc;
                        break;
                    case SortBy.ActionTypeAsc:
                        model.TypeSortAction = SortBy.ActionTypeDesc;
                        break;
                    case SortBy.ComplaintIdAsc:
                        model.ComplaintIdSortAction = SortBy.ComplaintIdDesc;
                        break;
                }

                // Count
                var count = await complaintActions.CountAsync().ConfigureAwait(false);

                // Paging
                complaintActions = complaintActions
                    .Skip((page - 1) * CTS.PageSize)
                    .Take(CTS.PageSize);

                // Select
                var items = await complaintActions
                    .Select(e => new ComplaintActionsListViewModel(e))
                    .ToListAsync().ConfigureAwait(false);

                model.ComplaintActions = new PaginatedList<ComplaintActionsListViewModel>(items, count, page);
            }

            return View(model);
        }

        // Helpers

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
