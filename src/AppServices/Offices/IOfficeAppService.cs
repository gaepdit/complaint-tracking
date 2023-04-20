﻿using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public interface IOfficeAppService : IDisposable
{
    Task<OfficeAdminViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<OfficeAdminViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);
    Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default);

    Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(Guid? id, bool activeOnly,
        CancellationToken token = default);

    Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default);
}
