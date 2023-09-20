using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Models;

public class PaginationNavModel
{
    public PaginationNavModel(IPaginatedResult paging, IDictionary<string, string?> routeValues)
    {
        Paging = paging;
        RouteValues = routeValues;
    }

    public IPaginatedResult Paging { get; init; }
    public IDictionary<string, string?> RouteValues { get; init; }
}
