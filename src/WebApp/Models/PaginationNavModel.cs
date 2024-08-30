using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Models;

public record PaginationNavModel(IPaginatedResult Paging, IDictionary<string, string?> RouteValues);
