using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Controllers
{
    public partial class ComplaintsController : Controller
    {
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id, bool PublicCopy = false)
        {
            if (User.Identity != null && (PublicCopy || !User.Identity.IsAuthenticated))
            {
                return RedirectToAction("ComplaintDetails", "Public", new {id});
            }

            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("ComplaintDetails", "Public", new {id});
            }

            var includeDeleted = User.IsInRole(CtsRole.DivisionManager.ToString());

            var model = await _dal.GetComplaintDetailsAsync(id.Value, includeDeleted);

            if (model == null)
            {
                return NotFound();
            }

            model.ComplaintTransitions = await _dal.GetComplaintTransitions(id.Value).ToListAsync();
            model.ComplaintActions = await _dal.GetComplaintActionsByComplaintId(id.Value).ToListAsync();
            model.Attachments = await _dal.GetAttachmentsByComplaintId(id.Value).ToListAsync();


            var officeMasterId = (await _context.LookupOffices.AsNoTracking()
                    .Where(e => e.Id == model.CurrentOffice.Id)
                    .SingleOrDefaultAsync())?
                .MasterUserId;

            // Control properties
            model.UserCanEdit = User.IsInRole(CtsRole.DivisionManager.ToString()) // Division Managers can edit all
                || User.IsInRole(CtsRole.Manager.ToString()) &&
                currentUser.OfficeId == model.CurrentOffice.Id // Managers can edit within their office
                || model.CurrentOwner != null && currentUser.Id == model.CurrentOwner.Id; // Users can edit their own
            model.UserCanEditDetails =
                User.IsInRole(CtsRole.DivisionManager.ToString()) // Division Managers can edit all
                || User.IsInRole(CtsRole.Manager.ToString()) &&
                currentUser.OfficeId == model.CurrentOffice.Id // Managers can edit within their office
                || model.CurrentOwner != null && currentUser.Id == model.CurrentOwner.Id // Users can edit their own
                || model.EnteredBy != null && currentUser.Id == model.EnteredBy.Id &&
                model.DateEntered.AddHours(1) > DateTime.Now; // Reporter can edit for 1 hour                    
            model.UserCanAssign = User.IsInRole(CtsRole.DivisionManager.ToString()) // Division Managers can edit all
                || User.IsInRole(CtsRole.Manager.ToString()) &&
                currentUser.OfficeId == model.CurrentOffice.Id // Managers can edit within their office
                || model.CurrentOwner == null && officeMasterId != null &&
                currentUser.Id == officeMasterId // Masters can reassign if within their office
                || model.CurrentOwner != null && currentUser.Id == model.CurrentOwner.Id; // Users can edit their own
            model.UserCanDelete = User.IsInRole(CtsRole.DivisionManager.ToString());
            model.ReviewRequested = model.Status == ComplaintStatus.ReviewPending;
            model.UserCanReopen = User.IsInRole(CtsRole.DivisionManager.ToString()); // Division Managers can reopen
            model.UserCanReview = User.IsInRole(CtsRole.DivisionManager.ToString()) // Division Managers can review all
                || User.IsInRole(CtsRole.Manager.ToString()) &&
                currentUser.OfficeId == model.CurrentOffice.Id; // Managers can review within their office
            model.MustAccept = currentUser.Id == model.CurrentOwner?.Id && model.DateCurrentOwnerAccepted == null
                && model.Status != ComplaintStatus.ReviewPending;
            model.UserIsOwner = currentUser.Id == model.CurrentOwner?.Id;
            model.IsAssigned = model.CurrentOwner != null;

            return View(model);
        }

        public async Task<IActionResult> PublicDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _dal.GetComplaintPublicDetailsAsync(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            if (model.ComplaintClosed)
            {
                return RedirectToAction("ComplaintDetails", "Public", new {id});
            }

            model.Attachments = await _dal.GetAttachmentsByComplaintId(id.Value).ToListAsync();

            return View(model);
        }
    }
}
