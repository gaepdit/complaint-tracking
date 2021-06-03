using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Generic;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ComplaintTracking.ViewModels.PublicSearchViewModel;

namespace ComplaintTracking.Controllers
{
    [AllowAnonymous]
    public partial class PublicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PublicSearchViewModel model)
        {
            if (model.FindComplaint.HasValue && ModelState.IsValid)
            {
                return RedirectToAction("ComplaintDetails", "Public", new {id = model.FindComplaint});
            }

            const string msg = "Please enter a Complaint ID first.";

            // ViewModel
            model = new PublicSearchViewModel()
            {
                FindComplaint = model.FindComplaint,
                CountySelectList = await GetCountiesSelectListAsync(),
                ConcernSelectList = await GetAreasOfConcernSelectListAsync(),
                StateSelectList = await GetStatesSelectListAsync(),
            };

            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Warning);
            return View(model);
        }

        [HttpGet, HttpHead]
        public async Task<IActionResult> Index(
            int page = 1,
            string submit = null,
            SortBy sort = SortBy.IdDesc,
            DateTime? DateFrom = null,
            DateTime? DateTo = null,
            string Nature = null,
            int? CountyId = null,
            Guid? TypeId = null,
            string SourceName = null,
            string Street = null,
            string City = null,
            int? StateId = null,
            string PostalCode = null
        )
        {
            // ViewModel
            var model = new PublicSearchViewModel()
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
                CountyId = CountyId,
                Nature = Nature,
                TypeId = TypeId,
                SourceName = SourceName,
                Street = Street,
                City = City,
                StateId = StateId,
                PostalCode = PostalCode,
                Sort = sort,
                CountySelectList = await GetCountiesSelectListAsync(),
                ConcernSelectList = await GetAreasOfConcernSelectListAsync(),
                StateSelectList = await GetStatesSelectListAsync(),
            };

            if (string.IsNullOrEmpty(submit))
            {
                return View(model);
            }

            if (DateFrom.HasValue && DateTo.HasValue
                && DateFrom.Value > DateTo.Value)
            {
                var msg = "The beginning date must precede the end date.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }
            else
            {
                // Search
                var complaints = _context.Complaints.AsNoTracking()
                    .Where(e => !e.Deleted && e.ComplaintClosed);

                // Filters
                if (DateFrom.HasValue)
                {
                    complaints = complaints
                        .Where(e => DateFrom.Value <= e.DateReceived);
                }

                if (DateTo.HasValue)
                {
                    complaints = complaints
                        .Where(e => e.DateReceived.Date <= DateTo.Value);
                }

                if (CountyId.HasValue)
                {
                    complaints = complaints
                        .Where(e => e.ComplaintCountyId.HasValue && e.ComplaintCountyId.Value == CountyId.Value);
                }

                if (!string.IsNullOrEmpty(Nature))
                {
                    complaints = complaints
                        .Where(e => e.ComplaintNature.ToLower().Contains(Nature.ToLower()));
                }

                if (TypeId.HasValue)
                {
                    complaints = complaints
                        .Where(e => (e.PrimaryConcernId.HasValue && e.PrimaryConcernId.Value == TypeId.Value)
                            || (e.SecondaryConcernId.HasValue && e.SecondaryConcernId.Value == TypeId.Value));
                }

                if (!string.IsNullOrEmpty(SourceName))
                {
                    complaints = complaints
                        .Where(e => e.SourceFacilityName.ToLower().Contains(SourceName.ToLower()));
                }

                if (!string.IsNullOrEmpty(Street))
                {
                    complaints = complaints
                        .Where(e => e.SourceStreet.ToLower().Contains(Street.ToLower())
                            || e.SourceStreet2.ToLower().Contains(Street.ToLower()));
                }

                if (!string.IsNullOrEmpty(City))
                {
                    complaints = complaints
                        .Where(e => e.SourceCity.ToLower().Contains(City.ToLower()) ||
                            e.ComplaintCity.ToLower().Contains(City.ToLower()));
                }

                if (StateId.HasValue)
                {
                    complaints = complaints
                        .Where(e => e.SourceStateId.HasValue && e.SourceStateId.Value == StateId.Value);
                }

                if (!string.IsNullOrEmpty(PostalCode))
                {
                    complaints = complaints
                        .Where(e => e.SourcePostalCode.ToLower().Contains(PostalCode.ToLower()));
                }

                // Count
                var count = await complaints.CountAsync().ConfigureAwait(false);

                // Sort
                switch (sort)
                {
                    case SortBy.IdAsc:
                        complaints = complaints.OrderBy(e => e.Id);
                        break;
                    case SortBy.IdDesc:
                        complaints = complaints.OrderByDescending(e => e.Id);
                        model.IdSortAction = SortBy.IdAsc;
                        break;
                    case SortBy.ReceivedDateAsc:
                        complaints = complaints.OrderBy(e => e.DateReceived)
                            .ThenBy(e => e.Id);
                        break;
                    case SortBy.ReceivedDateDesc:
                        complaints = complaints.OrderByDescending(e => e.DateReceived)
                            .ThenBy(e => e.Id);
                        model.ReceivedDateSortAction = SortBy.ReceivedDateAsc;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sort), sort, null);
                }

                // Include
                complaints = complaints
                    .Include(e => e.CurrentOffice)
                    .Include(e => e.SourceState);

                // Paging
                complaints = complaints
                    .Skip((page - 1) * CTS.PageSize)
                    .Take(CTS.PageSize);

                // Select
                var items = await complaints
                    .Select(e => new PublicSearchResultsViewModel(e))
                    .ToListAsync().ConfigureAwait(false);

                model.Complaints = new PaginatedList<PublicSearchResultsViewModel>(items, count, page);
            }

            return View(model);
        }

        public async Task<IActionResult> ComplaintDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetPublicComplaintDetailsAsync(id.Value);

            if (model == null)
            {
                var notFoundModel = new PublicSearchViewModel()
                {
                    FindComplaint = id
                };

                return View("ComplaintNotFound", notFoundModel);
            }

            model.ComplaintActions = await GetPublicComplaintActions(id.Value).ToListAsync();
            model.Attachments = await GetPublicComplaintAttachments(id.Value).ToListAsync();

            return View(model);
        }

        [Route("/Public/Attachment/{attachmentId}")]
        public async Task<IActionResult> Attachment(Guid attachmentId)
        {
            var fileName = await _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted)
                .Where(e => e.Complaint.ComplaintClosed)
                .Select(e => e.FileName)
                .SingleOrDefaultAsync();

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Attachment), new {attachmentId, fileName});
        }

        [Route("/Public/Attachment/{attachmentId}/{fileName}")]
        public async Task<IActionResult> Attachment(Guid attachmentId, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            var attachment = await _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted)
                .Where(e => e.Complaint.ComplaintClosed)
                .Select(e => new AttachmentViewModel(e))
                .SingleOrDefaultAsync();

            if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName))
            {
                return NotFound();
            }

            var filePath = Path.Combine(FilePaths.AttachmentsFolder,
                string.Concat(attachment.Id, attachment.FileExtension));

            return await TryReturnFile(filePath, attachment.FileName);
        }

        [Route("/Public/Thumbnail/{attachmentId}")]
        public async Task<IActionResult> Thumbnail(Guid attachmentId)
        {
            var attachment = await _context.Attachments.AsNoTracking()
                .Where(e => e.Id == attachmentId)
                .Where(e => !e.Deleted)
                .Where(e => !e.Complaint.Deleted)
                .Where(e => e.Complaint.ComplaintClosed)
                .Select(e => new AttachmentViewModel(e))
                .SingleOrDefaultAsync();

            if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName) || !attachment.IsImage)
            {
                return NotFound();
            }

            var filePath = Path.Combine(FilePaths.ThumbnailsFolder,
                string.Concat(attachment.Id, attachment.FileExtension));

            return await TryReturnFile(filePath, attachment.FileName);
        }

        private async Task<IActionResult> TryReturnFile(string filePath, string fileName)
        {
            byte[] fileBytes;

            try
            {
                fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            }
            catch (FileNotFoundException)
            {
                return FileNotFound(fileName);
            }
            catch (DirectoryNotFoundException)
            {
                return FileNotFound(fileName);
            }

            return fileBytes.Length == 0 ? FileNotFound(fileName) : File(fileBytes, FileTypes.GetContentType(fileName));
        }

        private IActionResult FileNotFound(string fileName)
        {
            if (FileTypes.FilenameImpliesImage(fileName))
            {
                return Redirect("~/static/Georgia_404.svg");
            }

            return NotFound();
        }
    }
}
