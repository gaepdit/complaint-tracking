using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public interface IOfficeService : IDisposable
{
    Task<OfficeWithAssignorDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<OfficeWithAssignorDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);
    Task UpdateAsync(Guid id, OfficeUpdateDto resource, CancellationToken token = default);

    Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);

    Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default);
}
