using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cts.Domain.Entities.ComplaintActions;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

public sealed class ActionRepository(AppDbContext context)
    : BaseRepository<ComplaintAction, Guid, AppDbContext>(context), IActionRepository
{
    public Task<TDestination?> FindAsync<TDestination>(
        Expression<Func<ComplaintAction, bool>> predicate,
        IMapper mapper,
        CancellationToken token = default) =>
        Context.ComplaintActions
            .Where(predicate)
            .ProjectTo<TDestination>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(token);
}
