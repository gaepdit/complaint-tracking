using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using static ComplaintTracking.Caching;

namespace ComplaintTracking.Controllers
{
    [Authorize(Roles = nameof(CtsRole.DataExport))]
    public class ExportController : Controller
    {
        private const int ExportLifespan = 15; // hours
        private const int ExportTimeout = 10 * 60; // Ten minutes
        private const int ExportFilesToKeep = 3;

        private readonly ApplicationDbContext _context;
        private readonly IErrorLogger _errorLogger;
        private readonly IMemoryCache _cache;

        public ExportController(
            ApplicationDbContext context,
            IErrorLogger errorLogger,
            IMemoryCache memoryCache)
        {
            _context = context;
            _errorLogger = errorLogger;
            _cache = memoryCache;

            Directory.CreateDirectory(FilePaths.ExportFolder);
        }

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

                var fileBytes = await System.IO.File.ReadAllBytesAsync(xm.FilePath);
                return File(fileBytes, FileTypes.ZipContentType, xm.FileName);
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
            if (!_cache.TryGetValue(CacheKeys.DataExportDate, out DataExportMeta xm))
            {
                xm = new DataExportMeta(DateTime.Now);
            }

            if (!System.IO.File.Exists(xm.FilePath) || new FileInfo(xm.FilePath).Length == 0)
            {
                _cache.Remove(CacheKeys.DataExportDate);
                await DeleteOldExportFilesAsync();
                xm = await CreateDataExportFileAsync();
            }

            _cache.Set(CacheKeys.DataExportDate, xm, xm.FileExpirationDate);
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

            await System.IO.File.WriteAllBytesAsync(xm.FilePath, await dataFiles.GetZipByteArrayAsync());

            return xm;
        }

        private async Task DeleteOldExportFilesAsync()
        {
            var d = new DirectoryInfo(FilePaths.ExportFolder);

            // Keep most recent files for auditing
            var fileInfos = d.GetFiles()
                .OrderByDescending(f => f.CreationTime)
                .Skip(ExportFilesToKeep);

            foreach (var fileInfo in fileInfos)
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (Exception ex)
                {
                    // Log error but take no other action if file can't be deleted
                    var customData = new Dictionary<string, object>
                    {
                        {"File", fileInfo.ToString()},
                        {"FileList", fileInfos.ToString()}
                    };
                    await _errorLogger.LogErrorAsync(ex, "DeleteOldExportFilesAsync", customData);
                }
            }
        }

        private class DataExportMeta
        {
            private DateTime ExportDate { get; }
            public DataExportMeta(DateTime dataExportDate) => ExportDate = dataExportDate;
            public string FileDateString => $"{ExportDate:yyyy-MM-dd-HH-mm-ss.FFF}";
            public string FileName => $"cts_export_{FileDateString}.zip";
            public string FilePath => Path.Combine(FilePaths.ExportFolder, FileName);
            public DateTimeOffset FileExpirationDate => new(ExportDate.AddHours(ExportLifespan));
        }

        private async Task<MemoryStream> OpenComplaintsCsvStreamAsync()
        {
            const string query = "SELECT * FROM gora.OpenComplaints ORDER BY ComplaintId";
            var result = await DataSqlHelper.ExecSQL<OpenComplaints>(query, _context, ExportTimeout);

            return await result.GetCsvMemoryStreamAsync();
        }

        private async Task<MemoryStream> ClosedComplaintsCsvStreamAsync()
        {
            const string query = "SELECT * FROM gora.ClosedComplaints ORDER BY ComplaintId";
            var result = await DataSqlHelper.ExecSQL<ClosedComplaints>(query, _context, ExportTimeout);

            return await result.GetCsvMemoryStreamAsync();
        }

        private async Task<MemoryStream> ClosedComplaintActionsCsvStreamAsync()
        {
            const string query = "SELECT * FROM gora.ClosedComplaintActions ORDER BY ComplaintId, ActionDate";
            var result = await DataSqlHelper.ExecSQL<ClosedComplaintActions>(query, _context, ExportTimeout);

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
            public DateTime DateComplaintClosed { get; set; }
            public DateTime DateCurrentOwnerAccepted { get; set; }
            public DateTime DateCurrentOwnerAssigned { get; set; }
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
            public DateTime DateEntered { get; set; }
            public string EnteredBy { get; set; }
            public string Investigator { get; set; }
        }
    }
}
