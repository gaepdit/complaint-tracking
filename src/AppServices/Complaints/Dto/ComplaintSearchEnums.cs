using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.Dto;

public enum SortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("DateReceived, Id")] ReceivedDateAsc,
    [Description("DateReceived desc, Id")] ReceivedDateDesc,
    [Description("Status, DateComplaintClosed, Id")] StatusAsc,
    [Description("Status desc, DateComplaintClosed desc, Id")] StatusDesc,
}

// The order of the values in this enum is intentional:
// The listed order of the items determines the order they appear in the search form dropdown,
// and the corresponding integer values ensure that previous bookmarks don't break.
public enum SearchComplaintStatus
{
    [Display(Name = "All Open")] AllOpen,
    [Display(Name = "All Closed")] AllClosed,
    [Display(Name = "New")] New,
    [Display(Name = "Under Investigation")] UnderInvestigation,
    [Display(Name = "Review Pending")] ReviewPending,
    [Display(Name = "Approved/Closed")] Closed,
    [Display(Name = "Administratively Closed")] AdministrativelyClosed,
}

// "Not Deleted" is included as an additional Delete Status option in the UI representing the null default state.
// "Deleted" = only deleted complaints
// "All" = all complaints
// "Not Deleted" (null) = only non-deleted complaints
public enum SearchDeleteStatus
{
    Deleted,
    All,
}
