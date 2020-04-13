using ComplaintTracking.Models;
using System.Collections.Generic;

namespace ComplaintTracking.Data
{
    public partial class SeedTestData
    {
        public static ActionType[] GetActionTypes()
        {
            string[] items = {
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

            var itemList = new List<ActionType>();

            foreach (string item in items)
            {
                itemList.Add(new ActionType { Name = item });
            }

            return itemList.ToArray();
        }
    }
}