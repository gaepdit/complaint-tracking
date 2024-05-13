using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.QueryDto;

public record ComplaintSearchDto
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
    public string? CallerName { get; set; }

    [Display(Name = "Represents")]
    public string? Represents { get; set; }

    // Complaint

    [Display(Name = "Complaint Text")]
    public string? Description { get; set; }

    [Display(Name = "City of Complaint")]
    public string? ComplaintCity { get; set; }

    [Display(Name = "County of Complaint")]
    public string? County { get; init; }

    [Display(Name = "Environmental Concern")]
    public Guid? Concern { get; init; }

    // Source

    [Display(Name = "Source/facility name")]
    public string? Source { get; set; }

    [Display(Name = "Facility ID Number")]
    public string? FacilityIdNumber { get; set; }

    [Display(Name = "Source Contact")]
    public string? Contact { get; init; }

    [Display(Name = "Street Address")]
    public string? Street { get; set; }

    [Display(Name = "City")]
    public string? City { get; set; }

    [Display(Name = "State")]
    public string? State { get; init; }

    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    [Display(Name = "Assigned office")]
    public Guid? Office { get; init; }

    [Display(Name = "Assigned associate")]
    public string? Assigned { get; init; }

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
    };

    public ComplaintSearchDto TrimAll() => this with
    {
        CallerName = CallerName?.Trim(),
        Represents = Represents?.Trim(),
        Description = Description?.Trim(),
        ComplaintCity = ComplaintCity?.Trim(),
        Source = Source?.Trim(),
        FacilityIdNumber = FacilityIdNumber?.Trim(),
        Street = Street?.Trim(),
        City = City?.Trim(),
        PostalCode = PostalCode?.Trim(),
    };
}
