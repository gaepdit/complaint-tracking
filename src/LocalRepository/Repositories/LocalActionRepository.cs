using AutoMapper;
using Cts.Domain.Entities.ComplaintActions;
using Cts.TestData;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalActionRepository()
    : BaseRepository<ComplaintAction, Guid>(ComplaintActionData.GetComplaintActions), IActionRepository
{
    public async Task<TDestination?> FindAsync<TDestination>(Expression<Func<ComplaintAction, bool>> predicate,
        IMapper mapper, CancellationToken token = default) =>
        mapper.Map<TDestination>(await FindAsync(predicate, token: token).ConfigureAwait(false));
}
