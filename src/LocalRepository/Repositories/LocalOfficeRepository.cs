using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.LocalRepository.Identity;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalOfficeRepository() : NamedEntityRepository<Office>(OfficeData.GetOffices), IOfficeRepository
{
    public LocalUserStore Staff { get; } = new();

    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Task.FromResult(Staff.Users
            .Where(e => e.Office != null && e.Office.Id == id)
            .Where(e => includeInactive || e.Active)
            .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList());

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        FindAsync(id, token);

    public Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        GetListAsync(token);
}
