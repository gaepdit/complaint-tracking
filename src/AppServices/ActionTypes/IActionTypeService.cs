﻿using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.ActionTypes;

public interface IActionTypeService : IDisposable, IAsyncDisposable
{
    Task<ActionTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(string name, CancellationToken token = default);
    Task UpdateAsync(Guid id, ActionTypeUpdateDto resource, CancellationToken token = default);
}
