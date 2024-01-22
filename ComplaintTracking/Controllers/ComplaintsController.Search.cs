using ComplaintTracking.AlertMessages;
using ComplaintTracking.Generic;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ComplaintTracking.ViewModels.SearchComplaintsViewModel;

namespace ComplaintTracking.Controllers
{
    public partial class ComplaintsController
    {
        public async Task<IActionResult> Index(
            int page = 1,
            string submit = null,
            SortBy sort = SortBy.IdDesc,
            SearchComplaintStatus? complaintStatus = null,
            SearchDeleteStatus? deleteStatus = null,
            DateTime? DateReceivedFrom = null,
            DateTime? DateReceivedTo = null,
            string ReceivedById = null,
            DateTime? DateComplaintClosedFrom = null,
            DateTime? DateComplaintClosedTo = null,
            string CallerName = null,
            string CallerRepresents = null,
            string ComplaintNature = null,
            string ComplaintLocation = null,
            string ComplaintDirections = null,
            string ComplaintCity = null,
            int? ComplaintCountyId = null,
            Guid? ConcernId = null,
            string SourceFacilityId = null,
            string SourceFacilityName = null,
            string SourceContactName = null,
            string SourceStreet = null,
            string SourceCity = null,
            int? SourceStateId = null,
            string SourcePostalCode = null,
            Guid? Office = null,
            string Owner = null,
            bool export = false
        )
        {
            var currentUser = await GetCurrentUserAsync();
            var isForPublic = currentUser == null;
            var includeDeleted = !isForPublic
                && User.IsInRole(CtsRole.DivisionManager.ToString());
            if (!includeDeleted) deleteStatus = null;

            var allUsers = await _dal.GetAllUsersSelectListAsync(true).ConfigureAwait(false);
            var owners = await _dal.GetUsersSelectListAsync(Office, true).ConfigureAwait(false);

            // ViewModel
            var model = new SearchComplaintsViewModel()
            {
                IsForPublic = isForPublic,
                IncludeDeleted = includeDeleted,
                ComplaintStatus = complaintStatus,
                DeleteStatus = deleteStatus,
                DateReceivedFrom = DateReceivedFrom,
                DateReceivedTo = DateReceivedTo,
                ReceivedById = ReceivedById,
                DateComplaintClosedFrom = DateComplaintClosedFrom,
                DateComplaintClosedTo = DateComplaintClosedTo,
                CallerName = CallerName,
                CallerRepresents = CallerRepresents,
                ComplaintNature = ComplaintNature,
                ComplaintLocation = ComplaintLocation,
                ComplaintDirections = ComplaintDirections,
                ComplaintCity = ComplaintCity,
                ComplaintCountyId = ComplaintCountyId,
                ConcernId = ConcernId,
                SourceFacilityId = SourceFacilityId,
                SourceFacilityName = SourceFacilityName,
                SourceContactName = SourceContactName,
                SourceStreet = SourceStreet,
                SourceCity = SourceCity,
                SourceStateId = SourceStateId,
                SourcePostalCode = SourcePostalCode,
                Office = Office,
                Owner = Owner,
                Sort = sort,
                AllUsersSelectList = allUsers,
                CountySelectList = await _dal.GetCountiesSelectListAsync(),
                ConcernSelectList = await _dal.GetAreasOfConcernSelectListAsync(),
                StateSelectList = await _dal.GetStatesSelectListAsync(),
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(true),
                OwnerSelectList = owners
            };

            if (string.IsNullOrEmpty(submit) && !export)
            {
                return View(model);
            }

            string msg = null;

            if (DateComplaintClosedFrom.HasValue && DateComplaintClosedTo.HasValue
                && DateComplaintClosedFrom.Value > DateComplaintClosedTo.Value)
            {
                msg += "The beginning closed date must precede the end date. ";
            }

            if (DateReceivedFrom.HasValue && DateReceivedTo.HasValue
                && DateReceivedFrom.Value > DateReceivedTo.Value)
            {
                msg += "The beginning received date must precede the end date. ";
            }

            if (msg != null)
            {
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                // Search
                var complaints = _dal.SearchComplaints(
                    sort,
                    complaintStatus,
                    deleteStatus,
                    DateReceivedFrom,
                    DateReceivedTo,
                    ReceivedById,
                    DateComplaintClosedFrom,
                    DateComplaintClosedTo,
                    CallerName,
                    CallerRepresents,
                    ComplaintNature,
                    ComplaintLocation,
                    ComplaintDirections,
                    ComplaintCity,
                    ComplaintCountyId,
                    ConcernId,
                    SourceFacilityId,
                    SourceFacilityName,
                    SourceContactName,
                    SourceStreet,
                    SourceCity,
                    SourceStateId,
                    SourcePostalCode,
                    Office,
                    Owner);

                // Sorters
                switch (sort)
                {
                    case SortBy.IdDesc:
                        model.IdSortAction = SortBy.IdAsc;
                        break;
                    case SortBy.ReceivedDateDesc:
                        model.ReceivedDateSortAction = SortBy.ReceivedDateAsc;
                        break;
                    case SortBy.StatusAsc:
                        model.StatusSortAction = SortBy.StatusDesc;
                        break;
                }

                // Export
                if (export)
                {
                    var fileName = $"cts_search_{DateTime.Now:yyyy-MM-dd-HH-mm-ss.FFF}.xlsx";
                    complaints = complaints
                        .Include(e => e.ComplaintActions.OrderByDescending(r => r.ActionDate)
                        .ThenByDescending(r => r.DateEntered).Take(1))
                        // including the action type navigation property causes this query to fail on SQLite
                        .ThenInclude(e => e.ActionType); 

                    if (includeDeleted && deleteStatus is not null)
                    {
                        var list = await complaints
                            .Select(e => new SearchResultsWithDeleteExportViewModel(e))
                            .ToListAsync().ConfigureAwait(false);
                        return File(list.ExportExcelAsStream(), FileTypes.ExcelContentType, fileName);
                    }
                    else
                    {
                        var list = await complaints
                            .Select(e => new SearchResultsExportViewModel(e))
                            .ToListAsync().ConfigureAwait(false);
                        return File(list.ExportExcelAsStream(), FileTypes.ExcelContentType, fileName);
                    }
                }

                // Paging
                var totalCount = await complaints.CountAsync().ConfigureAwait(false);

                complaints = complaints
                    .Skip((page - 1) * CTS.PageSize)
                    .Take(CTS.PageSize);

                var items = await complaints
                    .Select(e => new ComplaintListViewModel(e))
                    .ToListAsync().ConfigureAwait(false);

                model.Complaints = new PaginatedList<ComplaintListViewModel>(items, totalCount, page);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Download(
            SortBy sort = SortBy.IdDesc,
            SearchComplaintStatus? complaintStatus = null,
            SearchDeleteStatus? deleteStatus = null,
            DateTime? DateReceivedFrom = null,
            DateTime? DateReceivedTo = null,
            string ReceivedById = null,
            DateTime? DateComplaintClosedFrom = null,
            DateTime? DateComplaintClosedTo = null,
            string CallerName = null,
            string CallerRepresents = null,
            string ComplaintNature = null,
            string ComplaintLocation = null,
            string ComplaintDirections = null,
            string ComplaintCity = null,
            int? ComplaintCountyId = null,
            Guid? ConcernId = null,
            string SourceFacilityId = null,
            string SourceFacilityName = null,
            string SourceContactName = null,
            string SourceStreet = null,
            string SourceCity = null,
            int? SourceStateId = null,
            string SourcePostalCode = null,
            Guid? Office = null,
            string Owner = null
        )
        {
            var url = Url.Action("Index", new
            {
                sort,
                complaintStatus,
                deleteStatus,
                DateReceivedFrom,
                DateReceivedTo,
                ReceivedById,
                DateComplaintClosedFrom,
                DateComplaintClosedTo,
                CallerName,
                CallerRepresents,
                ComplaintNature,
                ComplaintLocation,
                ComplaintDirections,
                ComplaintCity,
                ComplaintCountyId,
                ConcernId,
                SourceFacilityId,
                SourceFacilityName,
                SourceContactName,
                SourceStreet,
                SourceCity,
                SourceStateId,
                SourcePostalCode,
                Office,
                Owner,
                export = true
            });

            return View("Download", url);
        }

        [HttpGet]
        [ActionName("Download")]
        public IActionResult GetDownload() => RedirectToAction("Index");
    }
}
