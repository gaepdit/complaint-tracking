using AutoMapper;
using Cts.AppServices.DtoBase;
using Cts.AppServices.UserServices;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.ServiceBase;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters
public abstract class MaintenanceItemService<TEntity, TViewDto, TUpdateDto>(
    INamedEntityRepository<TEntity> repository,
    INamedEntityManager<TEntity> manager,
    IMapper mapper,
    IUserService userService)
    : IMaintenanceItemService<TViewDto, TUpdateDto>
    where TEntity : StandardNamedEntity
    where TUpdateDto : StandardNamedEntityUpdateDto
#pragma warning restore S2436

{
    public async Task<TUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<TUpdateDto>(await repository.FindAsync(id, token).ConfigureAwait(false));

    public async Task<IReadOnlyList<TViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListAsync(token).ConfigureAwait(false)).Order()
            .OrderBy(entity => entity.Name).ThenBy(entity => entity.Id).ToList();
        return mapper.Map<IReadOnlyList<TViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetAsListItemsAsync(bool includeInactive = false,
        CancellationToken token = default) =>
        (await repository.GetListAsync(entity => includeInactive || entity.Active, token).ConfigureAwait(false))
        .Select(entity => new ListItem(entity.Id, entity.Name))
        .OrderBy(item => item.Name).ThenBy(item => item.Id)
        .ToList();

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var entity = await manager
            .CreateAsync(name, (await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id, token)
            .ConfigureAwait(false);
        await repository.InsertAsync(entity, token: token).ConfigureAwait(false);
        return entity.Id;
    }

    public async Task UpdateAsync(Guid id, TUpdateDto resource, CancellationToken token = default)
    {
        var entity = await repository.GetAsync(id, token).ConfigureAwait(false);
        if (entity.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(entity, resource.Name, token).ConfigureAwait(false);
        entity.Active = resource.Active;
        entity.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await repository.UpdateAsync(entity, token: token).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) repository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual async ValueTask DisposeAsyncCore()
    {
        await repository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
