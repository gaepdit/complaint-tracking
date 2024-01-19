using ComplaintTracking.AlertMessages;
using ComplaintTracking.Models;
using ComplaintTracking.Services;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Controllers
{
    public partial class ComplaintsController
    {
        // GET: Complaints/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await GetCurrentUserAsync();

            var model = new CreateComplaintViewModel()
            {
                SelectLists = await _dal.GetCommonSelectListsAsync(currentUser.OfficeId),
                ReceivedById = currentUser.Id,
                DateReceivedDate = DateTime.Now,
                CurrentOfficeId = currentUser.OfficeId,
                DisableCurrentOwnerSelect = false,
            };
            return View(model);
        }

        // POST: Complaints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateComplaintViewModel model)
        {
            var currentUser = await GetCurrentUserAsync();

            string msg = null;

            if (!ModelState.IsValid)
            {
                msg = "The Complaint was not created. Please fix the errors shown below.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

                // Populate the select lists before returning the model
                model.SelectLists = await _dal.GetCommonSelectListsAsync(model.CurrentOfficeId);
                model.DisableCurrentOwnerSelect = !(model.CurrentOfficeId == currentUser.OfficeId
                    || User.IsInRole(CtsRole.DivisionManager.ToString()));

                return View(model);
            }

            if (!User.IsInRole(CtsRole.DivisionManager.ToString())
                && model.CurrentOfficeId != currentUser.OfficeId
                || model.CurrentOwnerId == CTS.SelectUserMasterText)
            {
                model.CurrentOwnerId = null;
            }

            var complaint = new Complaint(model)
            {
                Status = ComplaintStatus.New,
                DateEntered = DateTime.Now,
                EnteredBy = currentUser,
                DateCurrentOwnerAssigned = (model.CurrentOwnerId != null) ? DateTime.Now : null,
                DateCurrentOwnerAccepted = (model.CurrentOwnerId != null && model.CurrentOwnerId == currentUser.Id)
                    ? DateTime.Now
                    : null,
            };

            // Save main complaint details
            try
            {
                _context.Add(complaint);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var customData = new Dictionary<string, object>
                {
                    {"Action", "Saving Complaint"}, {"ViewModel", model}
                };
                await _errorLogger.LogErrorAsync(ex, "POST: Complaints/Create add complaint", customData);

                msg = "There was an error saving the complaint. Please try again or contact support.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

                // Populate the select lists before returning the model
                model.SelectLists = await _dal.GetCommonSelectListsAsync(model.CurrentOfficeId);
                model.DisableCurrentOwnerSelect = !(model.CurrentOfficeId == currentUser.OfficeId
                    || User.IsInRole(CtsRole.DivisionManager.ToString()));
                return View(model);
            }

            // The complaint was successfully saved. Now start tracking any subsequent errors.
            var saveStatus = AlertStatus.Success;

            // Save initial complaint transitions
            var transitionSaveError = false;
            var transitionId = Guid.Empty;
            try
            {
                transitionId = await AddComplaintTransition(new ComplaintTransition()
                {
                    ComplaintId = complaint.Id,
                    TransferredByUserId = currentUser.Id,
                    TransferredToOfficeId = model.CurrentOfficeId,
                    TransitionType = TransitionType.New,
                });

                if (model.CurrentOwnerId != null)
                {
                    complaint.CurrentAssignmentTransitionId = await AddComplaintTransition(new ComplaintTransition()
                    {
                        ComplaintId = complaint.Id,
                        TransferredByUserId = currentUser.Id,
                        TransferredToUserId = model.CurrentOwnerId,
                        TransferredToOfficeId = model.CurrentOfficeId,
                        DateAccepted = (currentUser.Id == model.CurrentOwnerId) ? DateTime.Now : null,
                        TransitionType = TransitionType.Assigned,
                    });

                    if (model.CurrentOwnerId == currentUser.Id)
                    {
                        complaint.Status = ComplaintStatus.UnderInvestigation;
                    }

                    _context.Update(complaint);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var customData = new Dictionary<string, object>
                {
                    {"Action", "Saving Transitions"},
                    {"Complaint ID", complaint.Id},
                    {"ViewModel", model},
                    {"Complaint Model", complaint},
                    {"Transition ID", transitionId}
                };
                await _errorLogger.LogErrorAsync(ex, "POST: Complaints/Create add transitions", customData);

                saveStatus = AlertStatus.Warning;
                transitionSaveError = true;
            }

            // Email appropriate recipients
            var emailError = false;
            try
            {
                var complaintUrl = Url.Action("Details", "Complaints", new { id = complaint.Id },
                    protocol: HttpContext.Request.Scheme);
                if (complaint.CurrentOwnerId == null)
                {
                    // Email Master of current Office
                    var currentOffice = await _context.LookupOffices.FindAsync(complaint.CurrentOfficeId);

                    var officeMaster = await _userManager.FindByIdAsync(currentOffice.MasterUserId);
                    var masterEmail = await _userManager.GetEmailAsync(officeMaster);
                    await _emailSender.SendEmailAsync(
                        masterEmail,
                        string.Format(EmailTemplates.ComplaintOpenedToMaster.Subject, complaint.Id),
                        string.Format(EmailTemplates.ComplaintOpenedToMaster.PlainBody, complaint.Id, complaintUrl,
                            currentOffice.Name),
                        string.Format(EmailTemplates.ComplaintOpenedToMaster.HtmlBody, complaint.Id, complaintUrl,
                            currentOffice.Name),
                        !officeMaster.Active || !officeMaster.EmailConfirmed,
                        currentUser.Email);
                }
                else
                {
                    // Email new owner
                    var currentOwner = await _userManager.FindByIdAsync(complaint.CurrentOwnerId);
                    var currentOwnerEmail = await _userManager.GetEmailAsync(currentOwner);
                    var isValidUser = currentOwner.Active && currentOwner.EmailConfirmed;
                    await _emailSender.SendEmailAsync(
                        currentOwnerEmail,
                        string.Format(EmailTemplates.ComplaintAssigned.Subject, complaint.Id),
                        string.Format(EmailTemplates.ComplaintAssigned.PlainBody, complaint.Id, complaintUrl),
                        string.Format(EmailTemplates.ComplaintAssigned.HtmlBody, complaint.Id, complaintUrl),
                        !isValidUser,
                        currentUser.Email);
                }
            }
            catch (Exception ex)
            {
                var customData = new Dictionary<string, object>
                {
                    {"Action", "Emailing recipients"},
                    {"Complaint ID", complaint.Id},
                    {"ViewModel", model},
                    {"Complaint Model", complaint}
                };
                await _errorLogger.LogErrorAsync(ex, "POST: Complaints/Create send email", customData);

                saveStatus = AlertStatus.Warning;
                emailError = true;
            }

            // Save attachments
            var attachmentsError = false;
            var fileCount = 0;
            string attachmentErrorMessage = null;
            if (model.Attachments is { Count: > 0 })
            {
                switch (_fileService.ValidateUploadedFiles(model.Attachments))
                {
                    case FilesValidationResult.TooMany:
                        saveStatus = AlertStatus.Warning;
                        attachmentsError = true;
                        attachmentErrorMessage = "No more than 10 files can be uploaded at a time. ";
                        break;

                    case FilesValidationResult.WrongType:
                        saveStatus = AlertStatus.Warning;
                        attachmentsError = true;
                        attachmentErrorMessage =
                            "An invalid file type was selected. (Supported file types are images, documents, and spreadsheets.) ";
                        break;

                    case FilesValidationResult.Valid:
                        var savedFileList = new List<Attachment>();

                        try
                        {
                            foreach (var file in model.Attachments)
                            {
                                var attachment = await _fileService.SaveAttachmentAsync(file);
                                if (attachment == null) continue;

                                attachment.ComplaintId = complaint.Id;
                                attachment.UploadedById = currentUser.Id;
                                _context.Add(attachment);
                                savedFileList.Add(attachment);
                                fileCount++;
                            }

                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            var customData = new Dictionary<string, object>
                            {
                                {"Action", "Saving Attachments"},
                                {"Complaint ID", complaint.Id},
                                {"ViewModel", model},
                                {"Complaint Model", complaint}
                            };
                            await _errorLogger.LogErrorAsync(ex, "POST: Complaints/Create save attachments",
                                customData);

                            foreach (var attachment in savedFileList)
                            {
                                await _fileService.TryDeleteFileAsync(attachment.FileId, FilePaths.AttachmentsFolder);
                                if (attachment.IsImage)
                                {
                                    await _fileService.TryDeleteFileAsync(attachment.FileId, FilePaths.ThumbnailsFolder);
                                }
                            }

                            fileCount = 0;
                            saveStatus = AlertStatus.Warning;
                            attachmentsError = true;
                            attachmentErrorMessage = "An unknown database error occurred. ";
                        }

                        break;
                }
            }

            msg = fileCount switch
            {
                // Compile response message
                0 => "The Complaint has been created. ",
                1 => "The Complaint has been created and one file was attached. ",
                > 1 => $"The Complaint has been created and {fileCount} files were attached. ",
                _ => msg
            };

            if (saveStatus != AlertStatus.Success)
            {
                if (transitionSaveError)
                {
                    msg += "There were errors saving some of the complaint details. Please contact support. ";
                }

                if (emailError)
                {
                    msg += "There was an error emailing the recipients. Please contact support. ";
                }

                if (attachmentsError)
                {
                    msg += "There was a problem saving the attachments: " + attachmentErrorMessage +
                        "No files were saved. ";
                }

                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Warning");
            }

            TempData.SaveAlertForSession(msg, saveStatus, saveStatus.GetDisplayName());
            return RedirectToAction("Details", new { id = complaint.Id });
        }

        // GET: Complaints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();

            var model = await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EditComplaintViewModel(e))
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
                && !(User.IsInRole(CtsRole.DivisionManager.ToString()))
                && !(model.EnteredById == currentUser.Id && model.DateEntered.AddHours(1) > DateTime.Now))
            {
                msg = "You do not have permission to edit this Complaint.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            if (currentUser.Id == model.CurrentOwnerId && model.DateCurrentOwnerAccepted == null)
            {
                msg = "You must accept this Complaint before you can edit it.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            if (model.ComplaintIsClosed)
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            model.SelectLists = await _dal.GetCommonSelectListsAsync(currentUser.OfficeId);
            return View(model);
        }

        // POST: Complaints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditComplaintViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();

            string msg;

            if (!ModelState.IsValid)
            {
                msg = $"The {ObjectDisplayName} was not updated. Please fix the errors shown below.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

                // Populate the select lists before returning the model
                model.SelectLists = await _dal.GetCommonSelectListsAsync(currentUser.OfficeId);
                return View(model);
            }

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

            if (currentUser.Id != complaint.CurrentOwnerId
                && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                && !(User.IsInRole(CtsRole.DivisionManager.ToString()))
                && !(complaint.EnteredById == currentUser.Id && complaint.DateEntered.AddHours(1) > DateTime.Now))
            {
                msg = "You do not have permission to edit this Complaint.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id });
            }

            if (currentUser.Id == complaint.CurrentOwnerId && complaint.DateCurrentOwnerAccepted == null)
            {
                msg = "You must accept this Complaint before you can edit it.";
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
            if (model.DateReceivedDate != null) complaint.DateReceived = model.DateReceivedDate.Value.Date;
            if (model.DateReceivedTime.HasValue)
            {
                complaint.DateReceived = complaint.DateReceived.Add(model.DateReceivedTime.Value.TimeOfDay);
            }

            complaint.ReceivedById = model.ReceivedById;
            complaint.CallerName = model.CallerName;
            complaint.CallerRepresents = model.CallerRepresents;
            complaint.CallerStreet = model.CallerStreet;
            complaint.CallerStreet2 = model.CallerStreet2;
            complaint.CallerCity = model.CallerCity;
            complaint.CallerStateId = model.CallerStateId;
            complaint.CallerPostalCode = model.CallerPostalCode;
            complaint.CallerPhoneNumber = model.CallerPhoneNumber;
            complaint.CallerPhoneType = model.CallerPhoneType;
            complaint.CallerSecondaryPhoneNumber = model.CallerSecondaryPhoneNumber;
            complaint.CallerSecondaryPhoneType = model.CallerSecondaryPhoneType;
            complaint.CallerTertiaryPhoneNumber = model.CallerTertiaryPhoneNumber;
            complaint.CallerTertiaryPhoneType = model.CallerTertiaryPhoneType;
            complaint.CallerEmail = model.CallerEmail;
            complaint.ComplaintNature = model.ComplaintNature;
            complaint.ComplaintLocation = model.ComplaintLocation;
            complaint.ComplaintDirections = model.ComplaintDirections;
            complaint.ComplaintCity = model.ComplaintCity;
            complaint.ComplaintCountyId = model.ComplaintCountyId;
            complaint.PrimaryConcernId = model.PrimaryConcernId;
            complaint.SecondaryConcernId = model.SecondaryConcernId;
            complaint.SourceFacilityId = model.SourceFacilityId;
            complaint.SourceContactName = model.SourceContactName;
            complaint.SourceFacilityName = model.SourceFacilityName;
            complaint.SourceStreet = model.SourceStreet;
            complaint.SourceStreet2 = model.SourceStreet2;
            complaint.SourceCity = model.SourceCity;
            complaint.SourceStateId = model.SourceStateId;
            complaint.SourcePostalCode = model.SourcePostalCode;
            complaint.SourcePhoneNumber = model.SourcePhoneNumber;
            complaint.SourcePhoneType = model.SourcePhoneType;
            complaint.SourceSecondaryPhoneNumber = model.SourceSecondaryPhoneNumber;
            complaint.SourceSecondaryPhoneType = model.SourceSecondaryPhoneType;
            complaint.SourceTertiaryPhoneNumber = model.SourceTertiaryPhoneNumber;
            complaint.SourceTertiaryPhoneType = model.SourceTertiaryPhoneType;
            complaint.SourceEmail = model.SourceEmail;

            try
            {
                _context.Update(complaint);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _dal.ComplaintExistsAsync(model.Id)))
                {
                    return NotFound();
                }

                throw;
            }

            msg = $"The {ObjectDisplayName} was updated.";
            TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

            return RedirectToAction("Details", new { id });
        }
    }
}
