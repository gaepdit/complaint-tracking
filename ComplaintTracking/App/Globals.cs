using System;

namespace ComplaintTracking
{
    public static partial class CTS
    {
        // App-wide global variables

        // Server environment (value set in Startup.Configure)
        internal static ServerEnvironment CurrentEnvironment { get; set; }

        // Date of final data migration from old DNR Oracle CTS application 
        // into new EPD application: September 30, 2017
        public static readonly DateTime OracleMigrationDate = new DateTime(2017, 9, 30);

        // Default pagination size for search results, etc.
        public const int PageSize = 25;

        // Limit on exporting search results to CSV
        public const int CsvRecordsExportLimit = 25000;

        // Image thumbnail size
        public const int ThumbnailSize = 90;
        
        // Support contact and return address on system emails
        public static string AdminEmail;

        // Developer receives error and test emails
        public static string DevEmail;

        // Support contact
        public static string SupportEmail;

        // Account administrator
        public static string AccountAdminEmail;
    }
}
