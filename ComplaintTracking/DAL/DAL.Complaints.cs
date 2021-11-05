using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ComplaintTracking.ViewModels.SearchComplaintsViewModel;

namespace ComplaintTracking
{
    public partial class DAL
    {
        public Task<ComplaintDetailsViewModel> GetComplaintDetailsAsync(int id, bool includeDeleted = false)
        {
            return _context.Complaints.AsNoTracking()
                .Include(e => e.ComplaintCounty)
                .Include(e => e.PrimaryConcern)
                .Include(e => e.SecondaryConcern)
                .Include(e => e.SourceState)
                .Include(e => e.CurrentOffice)
                .Include(e => e.CurrentOwner)
                .Include(e => e.DeletedBy)
                .Include(e => e.ReviewBy)
                .Include(e => e.ReceivedBy)
                .Include(e => e.EnteredBy)
                .Where(e => includeDeleted || !e.Deleted)
                .Where(e => e.Id == id)
                .Select(e => new ComplaintDetailsViewModel(e))
                .SingleOrDefaultAsync();
        }

        public Task<bool> ComplaintExistsAsync(int id, bool includeDeleted = false)
        {
            return _context.Complaints.AsNoTracking()
                .AnyAsync(e => e.Id == id && (includeDeleted || !e.Deleted));
        }

        public Task<ComplaintPublicDetailsViewModel> GetComplaintPublicDetailsAsync(int complaintId)
        {
            return _context.Complaints.AsNoTracking()
                .Include(e => e.ComplaintCounty)
                .Include(e => e.PrimaryConcern)
                .Include(e => e.SecondaryConcern)
                .Include(e => e.SourceState)
                .Include(e => e.CurrentOffice)
                .Where(e => !e.Deleted)
                .Where(e => e.Id == complaintId)
                .Select(e => new ComplaintPublicDetailsViewModel(e))
                .SingleOrDefaultAsync();
        }

        public IQueryable<Complaint> SearchComplaints(
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
            var complaints = _context.Complaints.AsNoTracking();

            // Filters
            if (complaintStatus.HasValue)
            {
                complaints = complaintStatus.Value switch
                {
                    SearchComplaintStatus.Closed => complaints.Where(e =>
                        e.Status == ComplaintStatus.Closed),
                    SearchComplaintStatus.AdministrativelyClosed => complaints.Where(e =>
                        e.Status == ComplaintStatus.AdministrativelyClosed),
                    SearchComplaintStatus.AllClosed => complaints.Where(e => e.ComplaintClosed),
                    SearchComplaintStatus.New => complaints.Where(e => e.Status == ComplaintStatus.New),
                    SearchComplaintStatus.ReviewPending => complaints.Where(e =>
                        e.Status == ComplaintStatus.ReviewPending),
                    SearchComplaintStatus.UnderInvestigation => complaints.Where(e =>
                        e.Status == ComplaintStatus.UnderInvestigation),
                    SearchComplaintStatus.AllOpen => complaints.Where(e => !e.ComplaintClosed),
                    _ => complaints
                };
            }

            if (deleteStatus.HasValue)
            {
                if (deleteStatus.Value == SearchDeleteStatus.Deleted)
                {
                    complaints = complaints.Where(e => e.Deleted);
                }
                // Do not filter if deleteStatus.Value == SearchDeleteStatus.All
            }
            else
            {
                complaints = complaints
                    .Where(e => !e.Deleted);
            }

            if (DateReceivedFrom.HasValue)
            {
                complaints = complaints.Where(e => DateReceivedFrom.Value <= e.DateReceived);
            }

            if (DateReceivedTo.HasValue)
            {
                complaints = complaints.Where(e => e.DateReceived.Date <= DateReceivedTo.Value);
            }

            if (!string.IsNullOrEmpty(ReceivedById))
            {
                complaints = complaints.Where(e => e.ReceivedById == ReceivedById);
            }

            if (DateComplaintClosedFrom.HasValue)
            {
                complaints = complaints.Where(e =>
                    e.DateComplaintClosed.HasValue && DateComplaintClosedFrom.Value <= e.DateComplaintClosed.Value);
            }

            if (DateComplaintClosedTo.HasValue)
            {
                complaints = complaints.Where(e =>
                    e.DateComplaintClosed.HasValue && e.DateComplaintClosed.Value.Date <= DateComplaintClosedTo.Value);
            }

            if (!string.IsNullOrEmpty(CallerName))
            {
                complaints = complaints.Where(e => e.CallerName.ToLower().Contains(CallerName.ToLower()));
            }

            if (!string.IsNullOrEmpty(CallerRepresents))
            {
                complaints = complaints.Where(e => e.CallerRepresents.ToLower().Contains(CallerRepresents.ToLower()));
            }

            if (!string.IsNullOrEmpty(ComplaintNature))
            {
                complaints = complaints.Where(e => e.ComplaintNature.ToLower().Contains(ComplaintNature.ToLower()));
            }

            if (!string.IsNullOrEmpty(ComplaintLocation))
            {
                complaints = complaints.Where(e => e.ComplaintLocation.ToLower().Contains(ComplaintLocation.ToLower()));
            }

            if (!string.IsNullOrEmpty(ComplaintDirections))
            {
                complaints = complaints.Where(e =>
                    e.ComplaintDirections.ToLower().Contains(ComplaintDirections.ToLower()));
            }

            if (!string.IsNullOrEmpty(ComplaintCity))
            {
                complaints = complaints.Where(e => e.ComplaintCity.ToLower().Contains(ComplaintCity.ToLower()));
            }

            if (ComplaintCountyId.HasValue)
            {
                complaints = complaints.Where(e =>
                    e.ComplaintCountyId.HasValue && e.ComplaintCountyId.Value == ComplaintCountyId.Value);
            }

            if (ConcernId.HasValue)
            {
                complaints = complaints
                    .Where(e => (e.PrimaryConcernId.HasValue && e.PrimaryConcernId.Value == ConcernId.Value)
                        || (e.SecondaryConcernId.HasValue && e.SecondaryConcernId.Value == ConcernId.Value));
            }

            if (!string.IsNullOrEmpty(SourceFacilityId))
            {
                complaints = complaints.Where(e => e.SourceFacilityId.ToLower().Contains(SourceFacilityId.ToLower()));
            }

            if (!string.IsNullOrEmpty(SourceFacilityName))
            {
                complaints =
                    complaints.Where(e => e.SourceFacilityName.ToLower().Contains(SourceFacilityName.ToLower()));
            }

            if (!string.IsNullOrEmpty(SourceContactName))
            {
                complaints = complaints.Where(e => e.SourceContactName.ToLower().Contains(SourceContactName.ToLower()));
            }

            if (!string.IsNullOrEmpty(SourceStreet))
            {
                complaints = complaints
                    .Where(e => e.SourceStreet.ToLower().Contains(SourceStreet.ToLower())
                        || e.SourceStreet2.ToLower().Contains(SourceStreet.ToLower()));
            }

            if (!string.IsNullOrEmpty(SourceCity))
            {
                complaints = complaints.Where(e => e.SourceCity.ToLower().Contains(SourceCity.ToLower()));
            }

            if (SourceStateId.HasValue)
            {
                complaints = complaints.Where(e =>
                    e.SourceStateId.HasValue && e.SourceStateId.Value == SourceStateId.Value);
            }

            if (!string.IsNullOrEmpty(SourcePostalCode))
            {
                complaints = complaints.Where(e => e.SourcePostalCode.ToLower().Contains(SourcePostalCode.ToLower()));
            }

            if (Office.HasValue)
            {
                complaints = complaints.Where(e => e.CurrentOfficeId == Office.Value);
            }

            if (!string.IsNullOrEmpty(Owner))
            {
                complaints = complaints.Where(e => e.CurrentOwnerId == Owner);
            }

            // Sort
            complaints = sort switch
            {
                SortBy.IdAsc => complaints.OrderBy(e => e.Id),
                SortBy.ReceivedDateAsc => complaints.OrderBy(e => e.DateReceived).ThenBy(e => e.Id),
                SortBy.ReceivedDateDesc => complaints.OrderByDescending(e => e.DateReceived).ThenBy(e => e.Id),
                SortBy.StatusAsc => complaints.OrderBy(e => (int)e.Status).ThenBy(e => e.Id),
                SortBy.StatusDesc => complaints.OrderByDescending(e => (int)e.Status).ThenBy(e => e.Id),
                _ => complaints.OrderByDescending(e => e.Id),
            };

            return complaints
                .Include(e => e.CurrentOffice)
                .Include(e => e.CurrentOwner)
                .Include(e => e.ReceivedBy)
                .Include(e => e.SourceState)
                .Include(e => e.PrimaryConcern);
        }
    }
}
