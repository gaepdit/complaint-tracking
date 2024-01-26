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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFiles(int id, List<IFormFile> files, CancellationToken token)
        {
            var currentUser = await GetCurrentUserAsync();

            string msg;

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync(token);

            if (complaint == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check permissions
                if (complaint.Deleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                if (currentUser.Id != complaint.CurrentOwnerId
                    && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                    && !User.IsInRole(CtsRole.DivisionManager.ToString())
                    && !(complaint.EnteredById == currentUser.Id && complaint.DateEntered.AddHours(1) > DateTime.Now)
                    && !User.IsInRole(CtsRole.AttachmentsEditor.ToString()))
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

                if (complaint.ComplaintClosed && !User.IsInRole(CtsRole.AttachmentsEditor.ToString()))
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id });
                }

                switch (_attachmentService.ValidateUploadedFiles(files))
                {
                    case FilesValidationResult.TooMany:
                        msg = "Too many files selected. Please don't upload more than 10 files at a time.";
                        TempData.SaveAlertForSession(msg, AlertStatus.Warning, "No files saved");
                        return RedirectToAction("Details", "Complaints", new { id }, "attachments");

                    case FilesValidationResult.WrongType:
                        msg = "Invalid file type selected. No files were attached. Please try again. " +
                            "(Supported file types are images, documents, and spreadsheets.)";
                        TempData.SaveAlertForSession(msg, AlertStatus.Error, "Error");
                        return RedirectToAction("Details", "Complaints", new { id }, "attachments");
                }

                var fileCount = 0;
                var savedFileList = new List<Attachment>();

                foreach (var formFile in files)
                {
                    var attachment = await _attachmentService.SaveAttachmentAsync(formFile);
                    if (attachment == null) continue;

                    attachment.ComplaintId = complaint.Id;
                    attachment.UploadedById = currentUser.Id;
                    _context.Add(attachment);
                    savedFileList.Add(attachment);
                    fileCount++;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    foreach (var attachment in savedFileList)
                    {
                        await _attachmentService.DeleteAttachmentAsync(attachment.FileId, attachment.IsImage);
                    }

                    throw;
                }

                switch (fileCount)
                {
                    case 1:
                        msg = "One file attached.";
                        TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                        break;
                    case > 1:
                        msg = $"{fileCount} files attached.";
                        TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                        break;
                    default:
                        msg = "No files were attached. Please try again.";
                        TempData.SaveAlertForSession(msg, AlertStatus.Error, "Error");
                        break;
                }
            }
            else
            {
                msg = "An error occurred uploading the files. Please try again.";
                TempData.SaveAlertForSession(msg, AlertStatus.Error, "Error");
            }

            return RedirectToAction("Details", "Complaints", new { id }, "attachments");
        }

        [Route("/Complaints/Attachment/{attachmentId:guid}")]
        public async Task<IActionResult> Attachment(Guid attachmentId)
        {
            var fileName = await _dal.GetAttachmentFilenameByIdAsync(attachmentId);

            if (fileName == null || string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Attachment), new { attachmentId, fileName });
        }

        [Route("/Complaints/Attachment/{attachmentId:guid}/{fileName}")]
        public async Task<IActionResult> Attachment(Guid attachmentId, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            var attachment = await _dal.GetAttachmentByIdAsync(attachmentId);

            if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName))
            {
                return NotFound();
            }

            return await TryReturnAttachmentFile(attachment.FileId);
        }

        [Route("/Complaints/Thumbnail/{attachmentId:guid}")]
        public async Task<IActionResult> Thumbnail(Guid attachmentId)
        {
            var attachment = await _dal.GetAttachmentByIdAsync(attachmentId);

            if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName) || !attachment.IsImage)
            {
                return NotFound();
            }

            return await TryReturnThumbnailFile(attachment.FileId);
        }

        private Task<IActionResult> TryReturnAttachmentFile(string fileId) => TryReturnFile(fileId, false);
        private Task<IActionResult> TryReturnThumbnailFile(string fileId) => TryReturnFile(fileId, true);

        private async Task<IActionResult> TryReturnFile(string fileId, bool thumbnail)
        {
            var fileBytes = await _attachmentService.GetAttachmentAsync(fileId, thumbnail);
            return fileBytes.Length == 0 ? FileNotFound(fileId) : File(fileBytes, FileTypes.GetContentType(fileId));
        }

        private IActionResult FileNotFound(string fileName)
        {
            if (FileTypes.FilenameImpliesImage(fileName))
            {
                return Redirect("~/static/Georgia_404.svg");
            }

            return NotFound();
        }

        [HttpGet]
        [ActionName("DeleteAttachment")]
        public async Task<IActionResult> DeleteAttachmentConfirm(Guid id)
        {
            var model = await _dal.GetAttachmentByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == model.ComplaintId)
                .Select(e => new ViewAttachmentsViewModel(e))
                .SingleOrDefaultAsync();

            if (complaint == null)
            {
                return NotFound();
            }

            string msg;

            // Check permissions
            if (complaint.ComplaintDeleted)
            {
                msg = "This Complaint has been deleted and cannot be edited.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id = model.ComplaintId });
            }

            if (currentUser.Id != complaint.CurrentOwnerId
                && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                && !User.IsInRole(CtsRole.DivisionManager.ToString())
                && !(complaint.EnteredById == currentUser.Id && complaint.DateEntered.AddHours(1) > DateTime.Now)
                && !User.IsInRole(CtsRole.AttachmentsEditor.ToString()))
            {
                msg = "You do not have permission to edit this Complaint.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id = model.ComplaintId });
            }

            if (currentUser.Id == complaint.CurrentOwnerId
                && complaint.DateCurrentOwnerAccepted == null)
            {
                msg = "You must accept this Complaint before you can edit it.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id = model.ComplaintId });
            }

            if (complaint.ComplaintClosed && !User.IsInRole(CtsRole.AttachmentsEditor.ToString()))
            {
                msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                return RedirectToAction("Details", new { id = model.ComplaintId });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttachment(Guid id)
        {
            var currentUser = await GetCurrentUserAsync();

            var attachment = await _context.Attachments
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            if (attachment == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints.AsNoTracking()
                .Where(m => m.Id == attachment.ComplaintId)
                .Select(e => new ViewAttachmentsViewModel(e))
                .SingleOrDefaultAsync();

            if (complaint == null)
            {
                return NotFound();
            }

            string msg;

            if (ModelState.IsValid)
            {
                // Check permissions
                if (complaint.ComplaintDeleted)
                {
                    msg = "This Complaint has been deleted and cannot be edited.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id = attachment.ComplaintId });
                }

                if (currentUser.Id != complaint.CurrentOwnerId
                    && !(User.IsInRole(CtsRole.Manager.ToString()) && currentUser.OfficeId == complaint.CurrentOfficeId)
                    && !User.IsInRole(CtsRole.DivisionManager.ToString())
                    && !(complaint.EnteredById == currentUser.Id && complaint.DateEntered.AddHours(1) > DateTime.Now)
                    && !User.IsInRole(CtsRole.AttachmentsEditor.ToString()))
                {
                    msg = "You do not have permission to edit this Complaint.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id = attachment.ComplaintId });
                }

                if (currentUser.Id == complaint.CurrentOwnerId
                    && complaint.DateCurrentOwnerAccepted == null)
                {
                    msg = "You must accept this Complaint before you can edit it.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id = attachment.ComplaintId });
                }

                if (complaint.ComplaintClosed && !User.IsInRole(CtsRole.AttachmentsEditor.ToString()))
                {
                    msg = "This Complaint has been closed and cannot be edited unless it is reopened.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Warning, "Access Denied");
                    return RedirectToAction("Details", new { id = attachment.ComplaintId });
                }

                // Delete attachment
                attachment.Deleted = true;
                attachment.DateDeleted = DateTime.Now;
                attachment.DeletedById = currentUser.Id;

                try
                {
                    await _attachmentService.DeleteAttachmentAsync(attachment.FileId, attachment.IsImage);

                    _context.Update(attachment);
                    await _context.SaveChangesAsync();

                    msg = "The attachment has has been deleted.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                }
                catch
                {
                    msg = "An error occurred deleting the attachment. Please try again or contact support.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Error, "Error");
                }
            }
            else
            {
                msg = "An error occurred deleting the attachment. Please try again.";
                TempData.SaveAlertForSession(msg, AlertStatus.Error, "Error");
            }

            return RedirectToAction("Details", "Complaints", new { id = attachment.ComplaintId }, "attachments");
        }
    }
}
