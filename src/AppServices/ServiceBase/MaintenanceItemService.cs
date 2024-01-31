using AutoMapper;
using Cts.AppServices.DtoBase;
using Cts.AppServices.UserServices;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.ServiceBase;

#pragma warning disable S2436
public abstract class MaintenanceItemService<TEntity, TViewDto, TUpdateDto>(
    IRepository<TEntity> repository,
    INamedEntityManager<TEntity> manager,
    IMapper mapper,
    IUserService userService)
    : IMaintenanceItemService<TViewDto, TUpdateDto>
    where TEntity : StandardNamedEntity
    where TUpdateDto : StandardNamedEntityUpdateDto
#pragma warning restore S2436

{
    public async Task<TUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<TUpdateDto>(await repository.FindAsync(id, token));

    public async Task<IReadOnlyList<TViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return mapper.Map<List<TViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var item = await manager.CreateAsync(name, (await userService.GetCurrentUserAsync())?.Id, token);
        await repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task UpdateAsync(Guid id, TUpdateDto resource, CancellationToken token = default)
    {
        var item = await repository.GetAsync(id, token);
        if (item.Name != resource.Name.Trim()) await manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;
        item.SetUpdater((await userService.GetCurrentUserAsync())?.Id);
        await repository.UpdateAsync(item, token: token);
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
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await repository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
