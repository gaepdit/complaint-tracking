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
        public static readonly DateTime OracleMigrationDate = new(2017, 9, 30, 0, 0, 0, DateTimeKind.Local);

        // Default pagination size for search results, etc.
        public const int PageSize = 25;

        // Image thumbnail size
        public const int ThumbnailSize = 90;
    }
}
