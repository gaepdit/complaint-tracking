using ComplaintTracking.Models;
using Microsoft.EntityFrameworkCore;
using static ComplaintTracking.ViewModels.SearchComplaintActionsViewModel;

namespace ComplaintTracking
{
    public partial class DAL
    {
        public IQueryable<ComplaintAction> GetComplaintActionsByComplaintId(
            int complaintId,
            SortOrder sortOrder = SortOrder.Ascending,
            bool includeDeleted = false)
        {
            var actions = _context.ComplaintActions.AsNoTracking()
                .Include(e => e.ActionType)
                .Include(e => e.EnteredBy)
                .Include(e => e.DeletedBy)
                .Where(e => e.Complaint.Id == complaintId)
                .Where(e => includeDeleted || !e.Deleted);

            if (sortOrder == SortOrder.Ascending)
            {
                return actions.OrderBy(e => e.ActionDate)
                    .ThenBy(e => e.DateEntered);
            }

            return actions.OrderByDescending(e => e.ActionDate)
                .ThenByDescending(e => e.DateEntered);
        }

        public IQueryable<ComplaintAction> SearchComplaintActions(
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
            var complaintActions = _context.ComplaintActions.AsNoTracking();

            // Filters
            if (deleteStatus.HasValue)
            {
                if (deleteStatus.Value == SearchDeleteStatus.Deleted)
                {
                    complaintActions = complaintActions.Where(e => e.Deleted);
                }
                // Do not filter if deleteStatus.Value == SearchDeleteStatus.All
            }
            else
            {
                complaintActions = complaintActions.Where(e => !e.Deleted);
            }

            if (ActionDateFrom.HasValue)
            {
                complaintActions = complaintActions.Where(e => ActionDateFrom.Value <= e.ActionDate);
            }

            if (ActionDateTo.HasValue)
            {
                complaintActions = complaintActions.Where(e => e.ActionDate.Date <= ActionDateTo.Value);
            }

            if (ActionType.HasValue)
            {
                complaintActions = complaintActions.Where(e => e.ActionType.Id == ActionType.Value);
            }

            if (!string.IsNullOrEmpty(Investigator))
            {
                complaintActions =
                    complaintActions.Where(e => e.Investigator.ToLower().Contains(Investigator.ToLower()));
            }

            if (DateEnteredFrom.HasValue)
            {
                complaintActions = complaintActions.Where(e => DateEnteredFrom.Value <= e.DateEntered);
            }

            if (DateEnteredTo.HasValue)
            {
                complaintActions = complaintActions.Where(e => e.DateEntered <= DateEnteredTo.Value);
            }

            if (!string.IsNullOrEmpty(EnteredBy))
            {
                complaintActions = complaintActions.Where(e => e.EnteredById == EnteredBy);
            }

            if (!string.IsNullOrEmpty(Comments))
            {
                complaintActions = complaintActions.Where(e => e.Comments.ToLower().Contains(Comments.ToLower()));
            }

            if (ConcernId.HasValue)
            {
                complaintActions = complaintActions
                    .Where(e => (e.Complaint.PrimaryConcernId.HasValue &&
                            e.Complaint.PrimaryConcernId.Value == ConcernId.Value)
                        || (e.Complaint.SecondaryConcernId.HasValue &&
                            e.Complaint.SecondaryConcernId.Value == ConcernId.Value));
            }

            // Sort
            complaintActions = sort switch
            {
                SortBy.ActionDateAsc => complaintActions.OrderBy(e => e.ActionDate).ThenBy(e => e.ComplaintId),
                SortBy.ActionTypeAsc => complaintActions.OrderBy(e => e.ActionType.Name).ThenBy(e => e.ComplaintId),
                SortBy.ActionTypeDesc => complaintActions.OrderByDescending(e => e.ActionType.Name)
                    .ThenBy(e => e.ComplaintId),
                SortBy.ComplaintIdAsc => complaintActions.OrderBy(e => e.ComplaintId),
                SortBy.ComplaintIdDesc => complaintActions.OrderByDescending(e => e.ComplaintId),
                _ => complaintActions.OrderByDescending(e => e.ActionDate).ThenBy(e => e.ComplaintId),
            };

            return complaintActions
                .Include(e => e.ActionType)
                .Include(e => e.EnteredBy);
        }
    }
}
