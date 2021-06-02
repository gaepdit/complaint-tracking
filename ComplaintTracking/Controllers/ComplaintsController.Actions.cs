using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.AlertMessages;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Controllers
{
    public partial class ComplaintsController
    {
        public async Task<IActionResult> Actions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();

            var model = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new ViewComplaintActionsViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (model.ComplaintDeleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id});
            }

            if (currentUser.Id != model.CurrentOwnerId
                && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == model.CurrentOfficeId)
                && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
            {
                msg = "You do not have permission to edit this Complaint.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id});
            }

            if (currentUser.Id == model.CurrentOwnerId && model.DateCurrentOwnerAccepted == null)
            {
                msg = "You must accept this Complaint before you can edit it.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id});
            }

            if (model.ComplaintClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id});
            }

            var includeDeleted = User != null
                && (User.IsInRole(CtsRole.DivisionManager.ToString())
                    || User.IsInRole(CtsRole.Manager.ToString()));

            model.ComplaintActions = await _dal
                .GetComplaintActionsByComplaintId(id.Value, SortOrder.Descending, includeDeleted).ToListAsync();
            model.ActionTypesSelectList = await _dal.GetActionTypesSelectListAsync();
            model.UserCanDelete = includeDeleted;
            model.ActionDate = DateTime.Now;
            model.Investigator = currentUser.FullName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAction(int id, AddComplaintActionViewModel model)
        {
            var currentUser = await GetCurrentUserAsync();

            string msg;

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == model.ComplaintId)
                .SingleOrDefaultAsync();

            if (complaint == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var complaintAction = new ComplaintAction(model);

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                if (currentUser.Id != complaint.CurrentOwnerId
                    && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                    && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
                {
                    msg = "You do not have permission to edit this Complaint.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                if (currentUser.Id == complaint.CurrentOwnerId && complaint.DateCurrentOwnerAccepted == null)
                {
                    msg = "You must accept this Complaint before you can edit it.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                if (complaint.ComplaintClosed)
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                // Update model
                complaintAction.EnteredById = currentUser.Id;
                complaintAction.DateEntered = DateTime.Now;

                try
                {
                    _context.Add(complaintAction);
                    await _context.SaveChangesAsync();

                    msg = "The Action has been added.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Actions", new {id = model.ComplaintId});
                }
                catch
                {
                    msg = "There was an error saving the Action. Please try again or contact support.";
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = "The Action was not created. Please fix the errors shown below.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            // Populate the view model before returning
            var vm = new ViewComplaintActionsViewModel(complaint, model);

            var includeDeleted = User != null
                && (User.IsInRole(CtsRole.DivisionManager.ToString())
                    || User.IsInRole(CtsRole.Manager.ToString()));

            vm.ComplaintActions = await _dal
                .GetComplaintActionsByComplaintId(model.ComplaintId, SortOrder.Descending, includeDeleted)
                .ToListAsync();
            vm.ActionTypesSelectList = await _dal.GetActionTypesSelectListAsync();
            vm.UserCanDelete = includeDeleted;
            return View("Actions", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAction(Guid itemId, int complaintId)
        {
            var currentUser = await GetCurrentUserAsync();

            string msg;

            var userCanDelete = User != null
                && (User.IsInRole(CtsRole.DivisionManager.ToString())
                    || User.IsInRole(CtsRole.Manager.ToString()));
            if (!userCanDelete)
            {
                msg = "You do not have permission to delete Complaint Actions.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = complaintId});
            }

            if (!ModelState.IsValid)
            {
                msg = "There was an error deleting the Action. Please try again or contact support.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                return RedirectToAction("Actions", new {id = complaintId});
            }

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == complaintId)
                .SingleOrDefaultAsync();
            var complaintAction = await _context.ComplaintActions.AsNoTracking()
                .Where(e => e.Id == itemId)
                .SingleOrDefaultAsync();

            if (complaint == null || complaintAction == null)
            {
                return NotFound();
            }

            // Check permissions
            if (complaint.Deleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = complaintId});
            }

            if (complaintAction.Deleted)
            {
                msg = "The Action has already been deleted.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Actions", new {id = itemId});
            }

            // Update complaint action
            complaintAction.Deleted = true;
            complaintAction.DateDeleted = DateTime.Now;
            complaintAction.DeletedById = currentUser.Id;

            try
            {
                _context.Update(complaintAction);
                await _context.SaveChangesAsync();

                msg = "The Action has been deleted.";
                TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                return RedirectToAction("Actions", new {id = complaintId});
            }
            catch
            {
                msg = "There was an error deleting the Action. Please try again or contact support.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                return RedirectToAction("Actions", new {id = complaintId});
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreAction(Guid itemId, int complaintId)
        {
            string msg;

            var userCanDelete = User != null
                && (User.IsInRole(CtsRole.DivisionManager.ToString())
                    || User.IsInRole(CtsRole.Manager.ToString()));
            if (!userCanDelete)
            {
                msg = "You do not have permission to restore Complaint Actions.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = complaintId});
            }

            if (!ModelState.IsValid)
            {
                msg = "There was an error restoring the Action. Please try again or contact support.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                return RedirectToAction("Actions", new {id = complaintId});
            }

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == complaintId)
                .SingleOrDefaultAsync();
            var complaintAction = await _context.ComplaintActions.AsNoTracking()
                .Where(e => e.Id == itemId)
                .SingleOrDefaultAsync();

            if (complaint == null || complaintAction == null)
            {
                return NotFound();
            }

            // Check permissions
            if (complaint.Deleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = complaintId});
            }

            if (!complaintAction.Deleted)
            {
                msg = "The Action is not deleted, so it can't be restored.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Actions", new {id = itemId});
            }

            // Update complaint action
            complaintAction.Deleted = false;
            complaintAction.DateDeleted = null;
            complaintAction.DeletedById = null;

            try
            {
                _context.Update(complaintAction);
                await _context.SaveChangesAsync();

                msg = "The Action has been restored.";
                TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                return RedirectToAction("Actions", new {id = complaintId});
            }
            catch
            {
                msg = "There was an error restoring the Action. Please try again or contact support.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                return RedirectToAction("Actions", new {id = complaintId});
            }
        }

        public async Task<IActionResult> EditAction(Guid id)
        {
            var currentUser = await GetCurrentUserAsync();

            var model = await _context.ComplaintActions.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new EditComplaintActionViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == model.ComplaintId)
                .SingleOrDefaultAsync();

            if (complaint == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (complaint.Deleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = model.ComplaintId});
            }

            if (model.Deleted)
            {
                msg = "The Action has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Actions", new {id});
            }

            if (currentUser.Id != complaint.CurrentOwnerId
                && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
            {
                msg = "You do not have permission to edit this Complaint.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = model.ComplaintId});
            }

            if (currentUser.Id == complaint.CurrentOwnerId && complaint.DateCurrentOwnerAccepted == null)
            {
                msg = "You must accept this Complaint before you can edit it.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = model.ComplaintId});
            }

            if (complaint.ComplaintClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new {id = model.ComplaintId});
            }

            model.ActionTypesSelectList = await _dal.GetActionTypesSelectListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAction(Guid id, EditComplaintActionViewModel model)
        {
            var currentUser = await GetCurrentUserAsync();

            string msg;

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == model.ComplaintId)
                .SingleOrDefaultAsync();

            if (complaint == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var complaintAction = await _context.ComplaintActions
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaintAction == null)
                {
                    return NotFound();
                }

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                if (currentUser.Id != complaint.CurrentOwnerId
                    && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                    && !User.IsInRole(CtsRole.DivisionManager.ToString()))
                {
                    msg = "You do not have permission to edit this Complaint.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                if (currentUser.Id == complaint.CurrentOwnerId && complaint.DateCurrentOwnerAccepted == null)
                {
                    msg = "You must accept this Complaint before you can edit it.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                if (complaint.ComplaintClosed)
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new {id = model.ComplaintId});
                }

                // update complaint action
                complaintAction.ActionDate = model.ActionDate ?? default;
                complaintAction.ActionTypeId = model.ActionTypeId ?? default;
                complaintAction.Investigator = model.Investigator;
                complaintAction.Comments = model.Comments;

                try
                {
                    _context.Update(complaintAction);
                    await _context.SaveChangesAsync();

                    msg = "The Action has been updated.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Actions", new {id = model.ComplaintId});
                }
                catch
                {
                    msg = "There was an error saving the Action. Please try again or contact support.";
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = "The Action was not updated. Please fix the errors shown below.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            model.ActionTypesSelectList = await _dal.GetActionTypesSelectListAsync();
            return View(model);
        }
    }
}
