using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using GaEpd.FileService;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static ComplaintTracking.Caching;

namespace ComplaintTracking.Controllers
{
    [Authorize(Roles = nameof(CtsRole.DataExport))]
    public class ExportController(ApplicationDbContext context, IMemoryCache cache, IFileService fileService) : Controller
    {
        private const int ExportLifespan = 15; // hours
        private const int ExportTimeout = 600; // seconds
        private const int ExportDaysToKeep = 7; // days

        private MemoryStream CurrentFile { get; set; }

        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Download() => View("Download", Url.Action("ZipArchive"));

        [HttpGet]
        [ActionName("Download")]
        public IActionResult Download_Get() => RedirectToAction("Index");

        [HttpGet]
        [Produces(FileTypes.ZipContentType)]
        public async Task<IActionResult> ZipArchive()
        {
            try
            {
                var xm = await GetOrCreateDataExportAsync();
                if (CurrentFile == null) return BadRequest();
                return File(CurrentFile.ToArray(), FileTypes.ZipContentType, xm.FileName);
            }
            catch (SqlException)
            {
                const string msg = "A database error occurred; please try again later. " +
                    "If you continue to receive this message, please contact EPD-IT.";
                TempData.SaveAlertForSession(msg, AlertStatus.Error, "Error");
                return RedirectToAction("Index");
            }
        }

        private async Task<DataExportMeta> GetOrCreateDataExportAsync()
        {
            if (!cache.TryGetValue(CacheKeys.DataExportDate, out DataExportMeta xm))
            {
                xm = new DataExportMeta(DateTime.Now);
            }

            await using var response = await fileService.TryGetFileAsync(xm.FileName, FilePaths.ExportFolder);

            if (response.Success)
            {
                CurrentFile = response.Value as MemoryStream;
            }
            else
            {
                await DeleteOldExportFilesAsync();
                xm = await CreateDataExportFileAsync();
            }

            cache.Set(CacheKeys.DataExportDate, xm, xm.FileExpirationDate);
            return xm;
        }

        private async Task<DataExportMeta> CreateDataExportFileAsync()
        {
            var xm = new DataExportMeta(DateTime.Now);

            var dataFiles = new Dictionary<string, Task<MemoryStream>>
            {
                {$"{nameof(OpenComplaints)}_{xm.FileDateString}.csv", OpenComplaintsCsvStreamAsync()},
                {$"{nameof(ClosedComplaints)}_{xm.FileDateString}.csv", ClosedComplaintsCsvStreamAsync()},
                {$"{nameof(ClosedComplaintActions)}_{xm.FileDateString}.csv", ClosedComplaintActionsCsvStreamAsync()}
            };

            CurrentFile = await dataFiles.GetZipMemoryStreamAsync();

            await fileService.SaveFileAsync(CurrentFile, xm.FileName, FilePaths.ExportFolder);

            return xm;
        }

        private async Task DeleteOldExportFilesAsync()
        {
            var files = fileService.GetFilesAsync(FilePaths.ExportFolder);
            await foreach (var file in files)
            {
                // Keep recent files for auditing.
                if (file.CreatedOn < DateTimeOffset.UtcNow.AddDays(-ExportDaysToKeep))
                    await fileService.DeleteFileAsync(file.FullName);
            }
        }

        private sealed class DataExportMeta(DateTime ExportDate)
        {
            public string FileDateString => $"{ExportDate:yyyy-MM-dd-HH-mm-ss.FFF}";
            public string FileName => $"cts_export_{FileDateString}.zip";
            public DateTimeOffset FileExpirationDate => new(ExportDate.AddHours(ExportLifespan));
        }

        private async Task<MemoryStream> OpenComplaintsCsvStreamAsync()
        {
            const string query = "SELECT * FROM gora.OpenComplaints ORDER BY ComplaintId";
            context.Database.SetCommandTimeout(ExportTimeout);
            var result = context.Database.SqlQueryRaw<OpenComplaints>(query);
            return await result.GetCsvMemoryStreamAsync();
        }

        private async Task<MemoryStream> ClosedComplaintsCsvStreamAsync()
        {
            const string query = "SELECT * FROM gora.ClosedComplaints ORDER BY ComplaintId";
            context.Database.SetCommandTimeout(ExportTimeout);
            var result = context.Database.SqlQueryRaw<ClosedComplaints>(query);
            return await result.GetCsvMemoryStreamAsync();
        }

        private async Task<MemoryStream> ClosedComplaintActionsCsvStreamAsync()
        {
            const string query = "SELECT * FROM gora.ClosedComplaintActions ORDER BY ComplaintId, ActionDate";
            context.Database.SetCommandTimeout(ExportTimeout);
            var result = context.Database.SqlQueryRaw<ClosedComplaintActions>(query);
            return await result.GetCsvMemoryStreamAsync();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        public class OpenComplaints
        {
            public int ComplaintId { get; set; }
            public string CallerCity { get; set; }
            public string CallerName { get; set; }
            public string CallerPostalCode { get; set; }
            public string CallerRepresents { get; set; }
            public string CallerState { get; set; }
            public string CallerStreet { get; set; }
            public string CallerStreet2 { get; set; }
            public string ComplaintCity { get; set; }
            public string ComplaintCounty { get; set; }
            public string ComplaintDirections { get; set; }
            public string ComplaintLocation { get; set; }
            public string ComplaintNature { get; set; }
            public string PrimaryConcern { get; set; }
            public string SecondaryConcern { get; set; }
            public DateTime DateReceived { get; set; }
            public string ReceivedBy { get; set; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        public class ClosedComplaints
        {
            public int ComplaintId { get; set; }
            public string CallerCity { get; set; }
            public string CallerName { get; set; }
            public string CallerPostalCode { get; set; }
            public string CallerRepresents { get; set; }
            public string CallerState { get; set; }
            public string CallerStreet { get; set; }
            public string CallerStreet2 { get; set; }
            public string ComplaintCity { get; set; }
            public string ComplaintCounty { get; set; }
            public string ComplaintDirections { get; set; }
            public string ComplaintLocation { get; set; }
            public string ComplaintNature { get; set; }
            public string CurrentOffice { get; set; }
            public string CurrentOwner { get; set; }
            public DateTime? DateComplaintClosed { get; set; }
            public DateTime? DateCurrentOwnerAccepted { get; set; }
            public DateTime? DateCurrentOwnerAssigned { get; set; }
            public DateTime DateEntered { get; set; }
            public DateTime DateReceived { get; set; }
            public string EnteredBy { get; set; }
            public string PrimaryConcern { get; set; }
            public string ReceivedBy { get; set; }
            public string ReviewBy { get; set; }
            public string ReviewComments { get; set; }
            public string SecondaryConcern { get; set; }
            public string SourceCity { get; set; }
            public string SourceContactName { get; set; }
            public string SourceFacilityId { get; set; }
            public string SourceFacilityName { get; set; }
            public string SourcePostalCode { get; set; }
            public string SourceState { get; set; }
            public string SourceStreet { get; set; }
            public string SourceStreet2 { get; set; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        public class ClosedComplaintActions
        {
            public int ComplaintId { get; set; }
            public DateTime ActionDate { get; set; }
            public string ActionType { get; set; }
            public string Comments { get; set; }
            public DateTime? DateEntered { get; set; }
            public string EnteredBy { get; set; }
            public string Investigator { get; set; }
        }
    }
}
