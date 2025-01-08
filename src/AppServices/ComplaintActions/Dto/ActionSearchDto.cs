using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.ComplaintActions.Dto;

public record ActionSearchDto
{
    // Sorting

    public SortBy Sort { get; init; } = SortBy.DateDesc;

    // Spec

    [Display(Name = "Action Type")]
    public Guid? ActionType { get; init; }

    [Display(Name = "Deletion Status")]
    public SearchDeleteStatus? DeletedStatus { get; set; }

    [Display(Name = "Action Date From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateTo { get; init; }

    [Display(Name = "Entered By")]
    public string? EnteredBy { get; init; }

    [Display(Name = "Complaint assigned office")]
    public Guid? Office { get; init; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? EnteredFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? EnteredTo { get; init; }

    [Display(Name = "Investigator")]
    public string? Investigator { get; init; }

    [Display(Name = "Comments")]
    public string? Comments { get; init; }

    [Display(Name = "Environmental Concern")]
    public Guid? Concern { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(ActionType), ActionType?.ToString() },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
        { nameof(DateFrom), DateFrom?.ToString("d") },
        { nameof(DateTo), DateTo?.ToString("d") },
        { nameof(EnteredBy), EnteredBy },
        { nameof(Office), Office?.ToString() },
        { nameof(EnteredFrom), EnteredFrom?.ToString("d") },
        { nameof(EnteredTo), EnteredTo?.ToString("d") },
        { nameof(Investigator), Investigator },
        { nameof(Comments), Comments },
        { nameof(Concern), Concern?.ToString() },
    };

    public ActionSearchDto TrimAll() => this with
    {
        Investigator = Investigator?.Trim(),
        Comments = Comments?.Trim(),
    };
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortBy
{
    [Description("Complaint.Id, ActionDate")]
    IdAsc,

    [Description("Complaint.Id desc, ActionDate desc")]
    IdDesc,

    [Description("ActionType.Name, Complaint.Id, ActionDate")]
    TypeAsc,

    [Description("ActionType.Name desc, Complaint.Id desc, ActionDate desc")]
    TypeDesc,

    [Description("ActionDate, Complaint.Id")]
    DateAsc,

    [Description("ActionDate desc, Complaint.Id desc")]
    DateDesc,
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
