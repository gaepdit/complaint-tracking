using System;

namespace ComplaintTracking
{
    public static class Caching
    {
        public const int LONG_CACHE_TIME = 1;
        public const int EXTRA_LONG_CACHE_TIME = 30;
        public static TimeSpan LONG_CACHE_TIMESPAN = TimeSpan.FromDays(LONG_CACHE_TIME);
        public static TimeSpan EXTRA_LONG_CACHE_TIMESPAN = TimeSpan.FromDays(EXTRA_LONG_CACHE_TIME);

        public static class CacheKeys
        {
            public const string OfficesSelectListRequireMaster = "OfficesSelectListRequireMaster";
            public const string OfficesSelectList = "OfficesSelectList";
            public const string ActionTypesSelectList = "ActionTypesSelectList";
            public const string AreasOfConcernSelectList = "AreasOfConcernSelectList";
            public const string CountiesSelectList = "CountiesSelectList";
            public const string StatesSelectList = "StatesSelectList";
            public const string UsersSelectList = "UsersSelectList";
            public const string UsersIncludeInactiveSelectList = "UsersIncludeInactiveSelectList";
            public const string DataExportDate = "DataExportDate";
        }
    }
}
