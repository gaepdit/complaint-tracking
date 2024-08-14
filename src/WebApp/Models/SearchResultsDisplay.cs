using Cts.AppServices.Complaints.QueryDto;
using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Models;

public record SearchResultsDisplay(
    IBasicSearchDisplay Spec,
    IPaginatedResult<ComplaintSearchResultDto> SearchResults,
    PaginationNavModel Pagination,
    bool IsPublic)
{
    public string SortByName => Spec.Sort.ToString();
}
