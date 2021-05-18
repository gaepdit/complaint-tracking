using System;
using System.Diagnostics.CodeAnalysis;

namespace ComplaintTracking
{
    [SuppressMessage("ReSharper", "S101")]
    public static partial class CTS
    {
        // App-wide global variables

        // Server environment (value set in Startup.Configure)
        internal static ServerEnvironment CurrentEnvironment { get; set; }

        // Date of final data migration from old DNR Oracle CTS application 
        // into new EPD application: September 30, 2017
        public static readonly DateTime OracleMigrationDate = new(2017, 9, 30);

        // Default pagination size for search results, etc.
        public const int PageSize = 25;

        // Limit on exporting search results to CSV
        public const int CsvRecordsExportLimit = 25000;

        // Image thumbnail size
        public const int ThumbnailSize = 90;

        // Support contact and return address on system emails
        public static string AdminEmail { get; set; }

        // Developer receives error and test emails
        public static string DevEmail { get; set; }

        // Support contact
        public static string SupportEmail { get; set; }

        // Account administrator
        public static string AccountAdminEmail { get; set; }
    }
}
