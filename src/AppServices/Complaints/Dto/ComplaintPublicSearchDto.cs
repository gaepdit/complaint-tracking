using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.Dto;

public class ComplaintPublicSearchDto
{
    // Sorting

    public SortBy Sort { get; set; } = SortBy.Id;

    // Dates

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    public DateTime? DateFrom { get; set; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    public DateTime? DateTo { get; set; }

    // Complaint

    [Display(Name = "Description of Complaint")]
    [StringLength(100)]
    public string? Nature { get; set; }

    [Display(Name = "Type of Complaint")]
    public Guid? Type { get; set; }

    // Source

    [Display(Name = "Source (the entity associated with the incident)")]
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
    public string? State { get; set; }

    [Display(Name = "Postal Code")]
    [StringLength(10)]
    public string? PostalCode { get; set; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues =>
        new Dictionary<string, string?>
        {
            { nameof(Sort), Sort.ToString() },
            { nameof(DateFrom), DateFrom?.ToString("d") },
            { nameof(DateTo), DateTo?.ToString("d") },
            { nameof(Nature), Nature },
            { nameof(Type), Type?.ToString() },
            { nameof(SourceName), SourceName },
            { nameof(County), County },
            { nameof(Street), Street },
            { nameof(City), City },
            { nameof(State), State },
            { nameof(PostalCode), PostalCode },
        };
}

public enum SortBy
{
    [Description("Id")] Id,
    [Description("Id desc")] IdDesc,
    [Description("DateReceived, Id")] ReceivedDate,
    [Description("DateReceived desc, Id")] ReceivedDateDesc,
}
