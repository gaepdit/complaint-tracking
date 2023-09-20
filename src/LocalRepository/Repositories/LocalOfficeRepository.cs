using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalOfficeRepository : NamedEntityRepository<Office>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        FindAsync(id, token);

    public Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        GetListAsync(token);
}
