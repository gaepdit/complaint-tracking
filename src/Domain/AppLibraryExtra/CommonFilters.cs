using System.Linq.Expressions;

namespace Cts.Domain.AppLibraryExtra;

public static class CommonFilters
{
    public static Expression<Func<TEntity, bool>> WithId<TEntity, TKey>(
        this Expression<Func<TEntity, bool>> predicate,
        TKey id)
        where TEntity : IEntity<TKey> where TKey : IEquatable<TKey> =>
        predicate.And(e => e.Id.Equals(id));

    public static Expression<Func<TEntity, bool>> ExcludeDeleted<TEntity>(
        this Expression<Func<TEntity, bool>> predicate)
        where TEntity : ISoftDelete =>
        predicate.And(e => !e.IsDeleted);
}
