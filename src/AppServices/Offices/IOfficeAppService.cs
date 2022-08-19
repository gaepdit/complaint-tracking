using Cts.AppServices.StaffServices;

namespace Cts.AppServices.Offices;

public interface IOfficeAppService : IDisposable
{
    Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<OfficeViewDto>> GetListAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);
    Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default);
    Task<IReadOnlyList<StaffViewDto>> GetActiveStaffAsync(Guid id, CancellationToken token = default);
}
