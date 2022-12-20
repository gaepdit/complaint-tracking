using System.Linq.Dynamic.Core;

namespace Cts.Domain.AppLibraryExtra;

public static class Sorting
{
    public static IOrderedQueryable<TSource> OrderByIf<TSource>(this IQueryable<TSource> source,
        string? sorting) =>
        string.IsNullOrWhiteSpace(sorting) ? (IOrderedQueryable<TSource>)source : source.OrderBy(sorting);
}
