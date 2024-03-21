using Cts.AppServices.ServiceBase;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public interface IOfficeService : IMaintenanceItemService<OfficeViewDto, OfficeUpdateDto>
{
    Task<OfficeWithAssignorDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<OfficeWithAssignorDto>> GetListIncludeAssignorAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);

    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);

    Task<bool> UserIsAssignorForOfficeAsync(Guid id, string userId, CancellationToken token = default);

    Task<IReadOnlyCollection<OfficeViewDto>> GetOfficesForAssignorAsync(string userId, Guid? ignoreOffice,
        CancellationToken token = default);
}
