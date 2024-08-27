using ComplaintSortBy = Cts.AppServices.Complaints.QueryDto;
using ActionSortBy = Cts.AppServices.ComplaintActions.Dto;

namespace Cts.WebApp.Models;

public record SearchResultsColumnDisplay
{
    public SearchResultsColumnDisplay(string heading, ComplaintSortBy.SortBy ascender, ComplaintSortBy.SortBy descender,
        ComplaintSortBy.IBasicSearchDisplay spec)
    {
        Heading = heading;
        Up = ascender.ToString();
        Down = descender.ToString();
        SortByName = spec.Sort.ToString();
        RouteValues = spec.AsRouteValues();
    }

    public SearchResultsColumnDisplay(string heading, ActionSortBy.SortBy ascender, ActionSortBy.SortBy descender,
        ActionSortBy.ActionSearchDto spec)
    {
        Heading = heading;
        Up = ascender.ToString();
        Down = descender.ToString();
        SortByName = spec.Sort.ToString();
        RouteValues = spec.AsRouteValues();
    }

    public string Heading { get; init; }
    public string SortByName { get; }
    public string Up { get; }
    public string Down { get; }
    public IDictionary<string, string?> RouteValues { get; }
}
