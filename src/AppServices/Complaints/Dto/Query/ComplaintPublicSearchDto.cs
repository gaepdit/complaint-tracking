using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.Dto.Query;

public record ComplaintPublicSearchDto
{
    // Sorting

    public SortBy Sort { get; init; } = SortBy.IdAsc;

    // Dates

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateTo { get; init; }

    // Complaint

    [Display(Name = "Description of Complaint")]
    [StringLength(100)]
    public string? Description { get; set; }

    [Display(Name = "Type of Complaint")]
    public Guid? Concern { get; init; }

    // Source

    [Display(Name = "Source")]
    [StringLength(100)]
    public string? SourceName { get; set; }

    // Location

    [Display(Name = "County")]
    public string? County { get; set; }

    [Display(Name = "Street")]
    [StringLength(100)]
    public string? Street { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [Display(Name = "State")]
    public string? State { get; init; }

    [Display(Name = "Postal Code")]
    [StringLength(10)]
    public string? PostalCode { get; set; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(DateFrom), DateFrom?.ToString("d") },
        { nameof(DateTo), DateTo?.ToString("d") },
        { nameof(Description), Description },
        { nameof(Concern), Concern?.ToString() },
        { nameof(SourceName), SourceName },
        { nameof(County), County },
        { nameof(Street), Street },
        { nameof(City), City },
        { nameof(State), State },
        { nameof(PostalCode), PostalCode },
    };

    public void TrimAll()
    {
        Description = Description?.Trim();
        SourceName = SourceName?.Trim();
        County = County?.Trim();
        Street = Street?.Trim();
        City = City?.Trim();
        PostalCode = PostalCode?.Trim();
    }
}
