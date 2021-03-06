using ComplaintTracking.AlertMessages;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintTracking.Controllers
{
    public partial class ComplaintsController : Controller
    {
        // Review/Transitions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var complaint = await _context.Complaints
                .Where(e => e.Id == id)
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
                return RedirectToAction("Details", new { id });
            }
            if (currentUser.Id != complaint.CurrentOwner.Id)
            {
                msg = string.Format("This Complaint was not assigned to you so you cannot accept it.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }
            if (complaint.ComplaintClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            // update complaint properties
            complaint.DateCurrentOwnerAccepted = DateTime.Now;
            complaint.Status = ComplaintStatus.UnderInvestigation;

            try
            {
                _context.Update(complaint);

                if (complaint.CurrentAssignmentTransitionId.HasValue)
                {
                    var transition = await _context.ComplaintTransitions
                        .Where(e => e.Id == complaint.CurrentAssignmentTransitionId)
                        .SingleOrDefaultAsync();

                    if (transition != null)
                    {
                        transition.DateAccepted = DateTime.Now;
                        _context.Update(transition);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _dal.ComplaintExistsAsync(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            msg = "The Complaint was accepted.";
            TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Assign(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var model = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new AssignComplaintViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (model.ComplaintIsDeleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            string officeMasterId = null;
            if (model.CurrentOfficeId.HasValue)
            {
                officeMasterId = (await _context.LookupOffices.AsNoTracking()
                    .Where(e => e.Id == model.CurrentOfficeId.Value)
                    .SingleOrDefaultAsync())?
                    .MasterUserId;
            }
            bool currentUserIsMaster = model.CurrentOwnerId == null
                && officeMasterId != null
                && currentUser.Id == officeMasterId;

            if (currentUser.Id != model.CurrentOwnerId
                && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == model.CurrentOfficeId)
                && !(User.IsInRole(CtsRole.DivisionManager.ToString()))
                && !currentUserIsMaster)
            {
                msg = string.Format("You do not have permission to edit this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            if (model.ComplaintIsClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            model.OfficesSelectList = await _dal.GetOfficesSelectListAsync(true);
            model.UsersInOfficeSelectList = await _dal.GetUsersSelectListAsync(model.CurrentOfficeId);
            model.DisableCurrentOwnerSelect = !(model.CurrentOfficeId == currentUser.OfficeId
                || User.IsInRole(CtsRole.DivisionManager.ToString())
                || currentUserIsMaster);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, AssignComplaintViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaint == null)
                {
                    return NotFound();
                }

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                string officeMasterId = (await _context.LookupOffices.AsNoTracking()
                    .Where(e => e.Id == complaint.CurrentOfficeId)
                    .SingleOrDefaultAsync())
                    .MasterUserId;
                bool currentUserIsMaster = complaint.CurrentOwnerId == null
                    && officeMasterId != null
                    && currentUser.Id == officeMasterId;

                if (currentUser.Id != complaint.CurrentOwnerId
                     && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                     && !(User.IsInRole(CtsRole.DivisionManager.ToString()))
                     && !currentUserIsMaster)
                {
                    msg = string.Format("You do not have permission to edit this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                if (complaint.ComplaintClosed)
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                var fromOfficeId = complaint.CurrentOfficeId;
                var fromOwnerId = complaint.CurrentOwnerId;

                if (model.CurrentOfficeId == fromOfficeId
                    && model.CurrentOwnerId == fromOwnerId)
                {
                    msg = string.Format("The Complaint assignment has not changed.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Information);
                    return RedirectToAction("Details", new { id });
                }

                if (model.CurrentOwnerId == CTS.SelectUserMasterText
                    || !(User.IsInRole(CtsRole.DivisionManager.ToString())
                        || model.CurrentOfficeId == currentUser.OfficeId
                        || currentUserIsMaster))
                {
                    model.CurrentOwnerId = null;
                }

                // Update complaint properties
                complaint.CurrentOfficeId = model.CurrentOfficeId.Value;
                complaint.CurrentOwnerId = model.CurrentOwnerId;
                complaint.DateCurrentOwnerAssigned = (model.CurrentOwnerId != null) ? (DateTime?)DateTime.Now : null;
                complaint.DateCurrentOwnerAccepted = (model.CurrentOwnerId != null && model.CurrentOwnerId == currentUser.Id) ? (DateTime?)DateTime.Now : null;
                if (model.CurrentOwnerId == currentUser.Id)
                {
                    complaint.Status = ComplaintStatus.UnderInvestigation;
                }

                try
                {
                    complaint.CurrentAssignmentTransitionId =
                    await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransferredFromUserId = fromOwnerId,
                        TransferredFromOfficeId = fromOfficeId,
                        TransferredToUserId = model.CurrentOwnerId,
                        TransferredToOfficeId = model.CurrentOfficeId,
                        DateAccepted = (currentUser.Id == model.CurrentOwnerId) ? (DateTime?)DateTime.Now : null,
                        TransitionType = TransitionType.Assigned,
                        Comment = model.Comment,
                    });

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();

                    var complaintUrl = Url.Action("Details", "Complaints", new { id = complaint.Id }, protocol: HttpContext.Request.Scheme);
                    if (complaint.CurrentOwnerId == null)
                    {
                        // Send email to Master of current Office
                        var currentOffice = await _context.LookupOffices.FindAsync(complaint.CurrentOfficeId);
                        if (currentOffice?.MasterUserId != null)
                        {
                            var masterUser = await _userManager.FindByIdAsync(currentOffice.MasterUserId);
                            var masterEmail = await _userManager.GetEmailAsync(masterUser);
                            await _emailSender.SendEmailAsync(
                                masterEmail,
                                string.Format(EmailTemplates.ComplaintOpenedToMaster.Subject, complaint.Id),
                                string.Format(EmailTemplates.ComplaintOpenedToMaster.PlainBody, complaint.Id, complaintUrl, currentOffice.Name),
                                string.Format(EmailTemplates.ComplaintOpenedToMaster.HtmlBody, complaint.Id, complaintUrl, currentOffice.Name),
                                !masterUser.Active || !masterUser.EmailConfirmed,
                                replyTo: currentUser.Email);
                        }
                    }
                    else
                    {
                        // Send email to new owner
                        var currentOwner = await _userManager.FindByIdAsync(complaint.CurrentOwnerId);
                        var currentOwnerEmail = await _userManager.GetEmailAsync(currentOwner);
                        await _emailSender.SendEmailAsync(
                            currentOwnerEmail,
                            string.Format(EmailTemplates.ComplaintAssigned.Subject, complaint.Id),
                            string.Format(EmailTemplates.ComplaintAssigned.PlainBody, complaint.Id, complaintUrl),
                            string.Format(EmailTemplates.ComplaintAssigned.HtmlBody, complaint.Id, complaintUrl),
                            !currentOwner.Active || !currentOwner.EmailConfirmed,
                            replyTo: currentUser.Email);
                    }

                    msg = string.Format("The {0} has been assigned.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", objectDisplayName);
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }
            else
            {
                msg = string.Format("The {0} was not assigned. Please fix the errors shown below.", objectDisplayName);
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            }

            // Populate the select lists before returning the model
            model.OfficesSelectList = await _dal.GetOfficesSelectListAsync(true);
            model.UsersInOfficeSelectList = await _dal.GetUsersSelectListAsync(model.CurrentOfficeId);
            model.DisableCurrentOwnerSelect = (model.CurrentOfficeId == currentUser.OfficeId
                || User.IsInRole(CtsRole.DivisionManager.ToString()));
            return View(model);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var model = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new ApproveComplaintViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (model.ComplaintIsDeleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }
            if (!(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == model.CurrentOfficeId)
                && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
            {
                msg = string.Format("You do not have permission to review this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }
            if (model.ComplaintIsClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, ApproveComplaintViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaint == null)
                {
                    return NotFound();
                }

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }
                if (!(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                    && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
                {
                    msg = string.Format("You do not have permission to review this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }
                if (complaint.ComplaintClosed)
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                // Update complaint properties
                complaint.ReviewById = currentUser.Id;
                complaint.ReviewComments = model.Comment;
                complaint.ComplaintClosed = true;
                complaint.DateComplaintClosed = DateTime.Now;
                complaint.Status = ComplaintStatus.Closed;

                try
                {
                    await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransitionType = TransitionType.Closed,
                        Comment = model.Comment,
                    });

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();

                    string recipientId = complaint.CurrentOwnerId;
                    if (recipientId == null)
                    {
                        recipientId = (await _context.LookupOffices.AsNoTracking()
                            .Where(e => e.Id == complaint.CurrentOfficeId)
                            .SingleOrDefaultAsync())?
                            .MasterUserId;
                    }
                    if (recipientId != null)
                    {
                        var recipientUser = await _userManager.FindByIdAsync(recipientId);
                        var recipientEmail = await _userManager.GetEmailAsync(recipientUser);
                        var complaintUrl = Url.Action("Details", "Complaints", new { id = complaint.Id }, protocol: HttpContext.Request.Scheme);

                        await _emailSender.SendEmailAsync(
                            recipientEmail,
                            string.Format(EmailTemplates.ComplaintApproved.Subject, complaint.Id),
                            string.Format(EmailTemplates.ComplaintApproved.PlainBody, complaint.Id, complaintUrl, model.Comment),
                            string.Format(EmailTemplates.ComplaintApproved.HtmlBody, complaint.Id, complaintUrl,
                                (model.Comment == null) ? null : _htmlEncoder.Encode(model.Comment).Replace("&#xD;&#xA;", "<br />")),
                            !recipientUser.Active || !recipientUser.EmailConfirmed,
                            replyTo: currentUser.Email);
                    }

                    msg = string.Format("The {0} has been approved/closed.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", objectDisplayName);
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = string.Format("The {0} was not closed. Please fix the errors shown below.", objectDisplayName);
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        public async Task<IActionResult> Return(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var model = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new ReturnComplaintViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (model.ComplaintIsDeleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            if (!(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == model.CurrentOfficeId)
                && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
            {
                msg = string.Format("You do not have permission to review this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            if (model.Status != ComplaintStatus.ReviewPending)
            {
                msg = string.Format("Review was not requested for this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Information);
                return RedirectToAction("Details", new { id });
            }

            if (model.ComplaintIsClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            model.OfficesSelectList = await _dal.GetOfficesSelectListAsync(true);
            model.UsersInOfficeSelectList = await _dal.GetUsersSelectListAsync(model.CurrentOfficeId);
            model.DisableCurrentOwnerSelect = !(model.CurrentOfficeId == currentUser.OfficeId
                || User.IsInRole(CtsRole.DivisionManager.ToString()));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, ReturnComplaintViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaint == null)
                {
                    return NotFound();
                }

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                if (!(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                    && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
                {
                    msg = string.Format("You do not have permission to review this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                if (complaint.Status != ComplaintStatus.ReviewPending)
                {
                    msg = string.Format("Review was not requested for this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Information);
                    return RedirectToAction("Details", new { id });
                }

                if (complaint.ComplaintClosed)
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                var fromReviewerId = complaint.ReviewById;
                var fromOfficeId = complaint.CurrentOfficeId;
                var fromOwnerId = complaint.CurrentOwnerId;

                if (model.CurrentOwnerId == CTS.SelectUserMasterText
                    || !(User.IsInRole(CtsRole.DivisionManager.ToString())
                        || model.CurrentOfficeId == currentUser.OfficeId))
                {
                    model.CurrentOwnerId = null;
                }

                // Update complaint properties
                complaint.ReviewById = null;
                complaint.ReviewComments = null;
                complaint.Status = ComplaintStatus.UnderInvestigation;

                if (model.CurrentOwnerId != fromOwnerId)
                {
                    complaint.CurrentOfficeId = model.CurrentOfficeId.Value;
                    complaint.CurrentOwnerId = model.CurrentOwnerId;
                    complaint.DateCurrentOwnerAssigned = (model.CurrentOwnerId != null) ? (DateTime?)DateTime.Now : null;
                    complaint.DateCurrentOwnerAccepted = (model.CurrentOwnerId != null && model.CurrentOwnerId == currentUser.Id) ? (DateTime?)DateTime.Now : null;
                }

                try
                {
                    var transitionId = await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransferredFromUserId = fromReviewerId,
                        TransferredFromOfficeId = fromOfficeId,
                        TransferredToUserId = complaint.CurrentOwnerId,
                        TransferredToOfficeId = complaint.CurrentOfficeId,
                        DateAccepted = (model.CurrentOwnerId != fromOwnerId && currentUser.Id == model.CurrentOwnerId) ? (DateTime?)DateTime.Now : null,
                        TransitionType = TransitionType.ReturnedByReviewer,
                        Comment = model.Comment,
                    });

                    if (model.CurrentOwnerId != fromOwnerId)
                    {
                        complaint.CurrentAssignmentTransitionId = transitionId;
                    }

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();

                    string recipientId = complaint.CurrentOwnerId;
                    if (recipientId == null)
                    {
                        recipientId = (await _context.LookupOffices
                            .FindAsync(complaint.CurrentOfficeId))?
                            .MasterUserId;
                    }
                    if (recipientId != null)
                    {
                        var recipientUser = await _userManager.FindByIdAsync(recipientId);
                        var recipientEmail = await _userManager.GetEmailAsync(recipientUser);
                        var complaintUrl = Url.Action("Details", "Complaints", new { id = complaint.Id }, protocol: HttpContext.Request.Scheme);

                        await _emailSender.SendEmailAsync(
                            recipientEmail,
                            string.Format(EmailTemplates.ComplaintReturned.Subject, complaint.Id),
                            string.Format(EmailTemplates.ComplaintReturned.PlainBody, complaint.Id, complaintUrl, model.Comment),
                            string.Format(EmailTemplates.ComplaintReturned.HtmlBody, complaint.Id, complaintUrl,
                                (model.Comment == null) ? null : _htmlEncoder.Encode(model.Comment).Replace("&#xD;&#xA;", "<br />")),
                            !recipientUser.Active || !recipientUser.EmailConfirmed,
                            replyTo: currentUser.Email);
                    }

                    msg = string.Format("The {0} has been returned.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", objectDisplayName);
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = string.Format("The {0} was not edited. Please fix the errors shown below.", objectDisplayName);
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            // Populate the select lists before returning the model
            model.OfficesSelectList = await _dal.GetOfficesSelectListAsync(true);
            model.UsersInOfficeSelectList = await _dal.GetUsersSelectListAsync(model.CurrentOfficeId);
            model.DisableCurrentOwnerSelect = (model.CurrentOfficeId == currentUser.OfficeId
                || User.IsInRole(CtsRole.DivisionManager.ToString()));
            return View(model);
        }

        public async Task<IActionResult> RequestReview(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var model = await _context.Complaints.AsNoTracking()
                .Include(e => e.CurrentOffice)
                .Where(e => e.Id == id)
                .Select(e => new RequestReviewViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (model.ComplaintIsDeleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }
            if (currentUser.Id != model.CurrentOwnerId
                && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == model.CurrentOfficeId)
                && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
            {
                msg = string.Format("You do not have permission to edit this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }
            if (model.ComplaintIsClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            model.ManagersInOfficeSelectList = await _dal.GetUsersInRoleSelectListAsync(CtsRole.Manager, model.CurrentOfficeId);
            if (model.ManagersInOfficeSelectList == null
                || model.ManagersInOfficeSelectList.Count() == 0)
            {
                msg = $"\"{model.CurrentOfficeName}\" does not have any managers to review/approve Complaints. Please contact the Director's Office.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Warning");
                return RedirectToAction("Details", new { id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestReview(int id, RequestReviewViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            string msg;

            ApplicationUser reviewer = null;
            if (model.ReviewById != null)
            {
                reviewer = await _userManager.FindByIdAsync(model.ReviewById);
            }
            if (reviewer == null)
            {
                ModelState.AddModelError(nameof(RequestReviewViewModel.ReviewById), "Valid reviewer is required.");
            }

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaint == null)
                {
                    return NotFound();
                }

                if (model.ReviewById == complaint.ReviewById)
                {
                    msg = string.Format("The Complaint reviewer has not changed.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Information);
                    return RedirectToAction("Details", new { id });
                }

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }
                if (currentUser.Id != complaint.CurrentOwnerId
                     && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                     && !(User.IsInRole(CtsRole.DivisionManager.ToString())))
                {
                    msg = string.Format("You do not have permission to edit this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }
                if (complaint.ComplaintClosed)
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                var fromOwnerId = complaint.CurrentOwnerId;

                // Update complaint properties
                complaint.ReviewById = model.ReviewById;
                complaint.Status = ComplaintStatus.ReviewPending;

                try
                {
                    await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransferredFromUserId = fromOwnerId,
                        TransferredFromOfficeId = complaint.CurrentOfficeId,
                        TransferredToUserId = complaint.ReviewById,
                        TransferredToOfficeId = complaint.CurrentOfficeId,
                        TransitionType = TransitionType.SubmittedForReview,
                        Comment = model.Comment,
                    });

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();

                    var reviewerEmail = await _userManager.GetEmailAsync(reviewer);
                    var complaintUrl = Url.Action("Details", "Complaints", new { id = complaint.Id }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        reviewerEmail,
                        string.Format(EmailTemplates.ComplaintReviewRequested.Subject, complaint.Id),
                        string.Format(EmailTemplates.ComplaintReviewRequested.PlainBody, complaint.Id, complaintUrl, model.Comment),
                        string.Format(EmailTemplates.ComplaintReviewRequested.HtmlBody, complaint.Id, complaintUrl,
                            (model.Comment == null) ? null : _htmlEncoder.Encode(model.Comment).Replace("&#xD;&#xA;", "<br />")),
                        !reviewer.Active || !reviewer.EmailConfirmed,
                        replyTo: currentUser.Email);

                    msg = string.Format("The {0} has been submitted for review.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", objectDisplayName);
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = string.Format("The {0} was not updated. Please fix the errors shown below.", objectDisplayName);
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            // Populate the select lists before returning the model
            var officeID = (await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync())
                .CurrentOfficeId;
            model.ManagersInOfficeSelectList = await _dal.GetUsersInRoleSelectListAsync(CtsRole.Manager, officeID);
            if (model.ManagersInOfficeSelectList == null
                || model.ManagersInOfficeSelectList.Count() == 0)
            {
                msg = $"\"{model.CurrentOfficeName}\" does not have any managers to review/approve Complaints. Please contact the Director's Office.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Warning");
                return RedirectToAction("Details", new { id });
            }

            return View(model);
        }

        public async Task<IActionResult> Reopen(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var model = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new ReopenComplaintViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (model.ComplaintIsDeleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }
            if (!User.IsInRole(CtsRole.DivisionManager.ToString()))
            {
                msg = string.Format("You do not have permission to reopen this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }
            if (!model.ComplaintIsClosed)
            {
                msg = "This Complaint has not been closed so it cannot be reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Information);
                return RedirectToAction("Details", new { id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reopen(int id, ReopenComplaintViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaint == null)
                {
                    return NotFound();
                }

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }
                if (!User.IsInRole(CtsRole.DivisionManager.ToString()))
                {
                    msg = string.Format("You do not have permission to reopen this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }
                if (!complaint.ComplaintClosed)
                {
                    msg = "This Complaint has not been closed so it cannot be reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Information);
                    return RedirectToAction("Details", new { id });
                }

                // Update complaint properties
                complaint.ComplaintClosed = false;
                complaint.DateComplaintClosed = null;
                complaint.Status = ComplaintStatus.UnderInvestigation;
                complaint.ReviewById = null;

                try
                {
                    await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransferredToUserId = complaint.CurrentOwnerId,
                        TransferredToOfficeId = complaint.CurrentOfficeId,
                        TransitionType = TransitionType.Reopened,
                        Comment = model.Comment,
                    });

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();

                    string recipientId = complaint.CurrentOwnerId;
                    if (recipientId == null)
                    {
                        recipientId = (await _context.LookupOffices.AsNoTracking()
                            .Where(e => e.Id == complaint.CurrentOfficeId)
                            .SingleOrDefaultAsync())?
                            .MasterUserId;
                    }
                    if (recipientId != null)
                    {
                        var recipientUser = await _userManager.FindByIdAsync(recipientId);
                        var recipientEmail = await _userManager.GetEmailAsync(recipientUser);
                        var complaintUrl = Url.Action("Details", "Complaints", new { id = complaint.Id }, protocol: HttpContext.Request.Scheme);

                        await _emailSender.SendEmailAsync(
                            recipientEmail,
                            string.Format(EmailTemplates.ComplaintReopened.Subject, complaint.Id),
                            string.Format(EmailTemplates.ComplaintReopened.PlainBody, complaint.Id, complaintUrl, model.Comment),
                            string.Format(EmailTemplates.ComplaintReopened.HtmlBody, complaint.Id, complaintUrl,
                                (model.Comment == null) ? null : _htmlEncoder.Encode(model.Comment).Replace("&#xD;&#xA;", "<br />")),
                            !recipientUser.Active || !recipientUser.EmailConfirmed,
                            replyTo: currentUser.Email);
                    }

                    msg = string.Format("The {0} has been reopened.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", objectDisplayName);
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = string.Format("The {0} was not edited. Please fix the errors shown below.", objectDisplayName);
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var model = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new DeleteComplaintViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (model.ComplaintIsDeleted)
            {
                msg = "This Complaint has already been deleted.";
                TempData.SaveAlertForSession(msg, AlertStatus.Information);
                return RedirectToAction("Details", new { id });
            }
            if (!User.IsInRole(CtsRole.DivisionManager.ToString()))
            {
                msg = string.Format("You do not have permission to delete this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, DeleteComplaintViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaint == null)
                {
                    return NotFound();
                }

                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has already been deleted.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Information);
                    return RedirectToAction("Details", new { id });
                }
                if (!User.IsInRole(CtsRole.DivisionManager.ToString()))
                {
                    msg = string.Format("You do not have permission to delete this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                // Update complaint properties
                complaint.Deleted = true;
                complaint.DeletedById = currentUser.Id;
                complaint.DateDeleted = DateTime.Now;
                complaint.DeleteComments = model.Comment;

                try
                {
                    await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransitionType = TransitionType.Deleted,
                        Comment = model.Comment,
                    });

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();

                    msg = string.Format("The {0} has been deleted.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", objectDisplayName);
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = string.Format("The {0} was not deleted. Please fix the errors shown below.", objectDisplayName);
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        public async Task<IActionResult> Restore(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            var model = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == id)
                .Select(e => new RestoreComplaintViewModel(e))
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (!model.ComplaintIsDeleted)
            {
                msg = "This Complaint is not deleted, so it can't be restored.";
                TempData.SaveAlertForSession(msg, AlertStatus.Information);
                return RedirectToAction("Details", new { id });
            }
            if (!User.IsInRole(CtsRole.DivisionManager.ToString()))
            {
                msg = string.Format("You do not have permission to restore this Complaint.", objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id, RestoreComplaintViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new Exception("Current user not found");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (complaint == null)
                {
                    return NotFound();
                }

                // Check permissions
                if (!complaint.Deleted)
                {
                    msg = "This Complaint is not deleted, so it can't be restored.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Information);
                    return RedirectToAction("Details", new { id });
                }
                if (!User.IsInRole(CtsRole.DivisionManager.ToString()))
                {
                    msg = string.Format("You do not have permission to restore this Complaint.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                // Update complaint properties
                complaint.Deleted = false;
                complaint.DeletedById = null;
                complaint.DateDeleted = null;
                complaint.DeleteComments = null;

                try
                {
                    await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransitionType = TransitionType.Restored,
                        Comment = model.Comment,
                    });

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();

                    msg = string.Format("The {0} has been restored.", objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", objectDisplayName);
                    ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                }
            }

            msg = string.Format("The {0} was not restored. Please fix the errors shown below.", objectDisplayName);
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        private async Task<Guid> AddComplaintTransition(ComplaintTransition transition)
        {
            _context.Add(transition);
            await _context.SaveChangesAsync();
            return transition.Id;
        }
    }
}
