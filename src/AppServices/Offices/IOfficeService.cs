using Cts.AppServices.ServiceBase;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public interface IOfficeService : IMaintenanceItemService<OfficeWithAssignorDto, OfficeUpdateDto>
{
    Task<OfficeWithAssignorDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);

    Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);

    Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default);
}
