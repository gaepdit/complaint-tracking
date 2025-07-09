using AutoMapper;
using System.Linq.Expressions;

namespace Cts.Domain.Entities.ComplaintActions;

public interface IActionRepository : IRepository<ComplaintAction, Guid>
{
    Task<TDestination?> FindAsync<TDestination>(
        Expression<Func<ComplaintAction, bool>> predicate,
        IMapper mapper,
        CancellationToken token = default);
}
