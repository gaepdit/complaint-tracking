using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalOfficeRepository() : NamedEntityRepository<Office>(OfficeData.GetOffices), IOfficeRepository
{
    public async Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => includeInactive || e.Active)
        .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        FindAsync(id, token);

    public Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        GetListAsync(token);
}
