using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.QueryDto;

public record ComplaintSearchDto : IBasicSearchDisplay
{
    // Sorting

    public SortBy Sort { get; init; } = SortBy.IdDesc;

    // Status

    [Display(Name = "Complaint Status")]
    public SearchComplaintStatus? Status { get; init; }

    [Display(Name = "Deletion Status")]
    public SearchDeleteStatus? DeletedStatus { get; set; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedTo { get; init; }

    // Attachments
    [Display(Name = "Has Attachments")]
    public YesNoAny? Attachments { get; init; }

    // Received

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReceivedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReceivedTo { get; init; }

    [Display(Name = "Received By")]
    public string? ReceivedBy { get; init; }

    // Caller

    [Display(Name = "Caller Name")]
    public string? CallerName { get; init; }

    [Display(Name = "Represents")]
    public string? Represents { get; init; }

    // Complaint

    [Display(Name = "Complaint Text")]
    public string? Description { get; init; }

    [Display(Name = "City of Complaint")]
    public string? ComplaintCity { get; init; }

    [Display(Name = "County of Complaint")]
    public string? County { get; init; }

    [Display(Name = "Environmental Concern")]
    public Guid? Concern { get; init; }

    // Source

    [Display(Name = "Source/Facility Name")]
    public string? Source { get; init; }

    [Display(Name = "Facility ID Number")]
    public string? FacilityIdNumber { get; init; }

    [Display(Name = "Contact Name")]
    public string? Contact { get; init; }

    [Display(Name = "Street Address")]
    public string? Street { get; init; }

    [Display(Name = "City")]
    public string? City { get; init; }

    [Display(Name = "State")]
    public string? State { get; init; }

    [Display(Name = "Postal Code")]
    public string? PostalCode { get; init; }

    [Display(Name = "Assigned office")]
    public Guid? Office { get; init; }

    [Display(Name = "Assigned associate")]
    public string? Assigned { get; init; }

    [Display(Name = "Only include unassigned complaints")]
    public bool OnlyUnassigned { get; init; }

    // Additional search terms used on Dashboard; these should not be added to UI route values.
    public string? Reviewer { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Status), Status?.ToString() },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
        { nameof(ClosedFrom), ClosedFrom?.ToString("d") },
        { nameof(ClosedTo), ClosedTo?.ToString("d") },
        { nameof(Attachments), Attachments?.ToString() },
        { nameof(ReceivedFrom), ReceivedFrom?.ToString("d") },
        { nameof(ReceivedTo), ReceivedTo?.ToString("d") },
        { nameof(ReceivedBy), ReceivedBy },
        { nameof(CallerName), CallerName },
        { nameof(Represents), Represents },
        { nameof(Description), Description },
        { nameof(ComplaintCity), ComplaintCity },
        { nameof(County), County },
        { nameof(Concern), Concern?.ToString() },
        { nameof(Source), Source },
        { nameof(FacilityIdNumber), FacilityIdNumber },
        { nameof(Contact), Contact },
        { nameof(Street), Street },
        { nameof(City), City },
        { nameof(State), State },
        { nameof(PostalCode), PostalCode },
        { nameof(Office), Office?.ToString() },
        { nameof(Assigned), Assigned },
        { nameof(OnlyUnassigned), OnlyUnassigned.ToString() },
    };

    public ComplaintSearchDto TrimAll() => this with
    {
        CallerName = CallerName?.Trim(),
        Represents = Represents?.Trim(),
        Description = Description?.Trim(),
        ComplaintCity = ComplaintCity?.Trim(),
        Source = Source?.Trim(),
        FacilityIdNumber = FacilityIdNumber?.Trim(),
        Contact = Contact?.Trim(),
        Street = Street?.Trim(),
        City = City?.Trim(),
        PostalCode = PostalCode?.Trim(),
    };
}
