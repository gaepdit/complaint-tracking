using System.Linq;
using ComplaintTracking.Models;

namespace ComplaintTracking.Data
{
    public partial class SeedTestData
    {
        public static ActionType[] GetActionTypes()
        {
            string[] items =
            {
                "Initial investigation",
                "Follow-up investigation",
                "Referred to",
                "Notice of violation",
                "Initial investigation report",
                "Follow-up investigation report",
                "Status letter",
                "Other",
                "Consent/administrative order"
            };

            return items.Select(item => new ActionType {Name = item}).ToArray();
        }
    }
}
