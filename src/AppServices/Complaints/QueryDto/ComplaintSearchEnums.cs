using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Complaints.QueryDto;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("ReceivedDate, Id")] ReceivedDateAsc,
    [Description("ReceivedDate desc, Id")] ReceivedDateDesc,
    [Description("Status, ComplaintClosedDate, Id")] StatusAsc,

    [Description("Status desc, ComplaintClosedDate desc, Id")]
    StatusDesc,
}

// The order of the values in this enum is intentional:
// The listed order of the items determines the order they appear in the search form dropdown,
// and the corresponding integer values ensure that previous bookmarks don't break.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchComplaintStatus
{
    [Display(Name = "All Open")] AllOpen = 0,
    [Display(Name = "All Closed")] AllClosed = 1,
    [Display(Name = "New")] New = 2,
    [Display(Name = "Under Investigation")] UnderInvestigation = 3,
    [Display(Name = "Review Pending")] ReviewPending = 4,
    [Display(Name = "Assigned But Not Accepted")] NotAccepted = 7,
    [Display(Name = "Not Assigned")] NotAssigned = 8,
    [Display(Name = "Approved/Closed")] Closed = 5,
    [Display(Name = "Administratively Closed")] AdministrativelyClosed = 6,
}

// "(Any)" (null) = no filtering
// "Closed" = only closed complaints
// "Open" = only open complaints
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PublicSearchStatus
{
    Closed = 0,
    Open = 1,
}

// "Not Deleted" is included as an additional Delete Status option in the UI representing the null default state.
// "Deleted" = only deleted complaints
// "All" = all complaints
// "Not Deleted" (null) = only non-deleted complaints
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchDeleteStatus
{
    Deleted = 0,
    All = 1,
}

// "(Any)" (null) = no filtering
// "Yes" = only if value is true
// "No" = only if value is false
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum YesNoAny
{
    Yes = 1,
    No = 0,
}
