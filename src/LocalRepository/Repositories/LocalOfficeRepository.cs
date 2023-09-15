using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalOfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(Guid id, CancellationToken token = default)
    {
        var office = await FindAsync(id, token);
        return office is null
            ? new List<ApplicationUser>()
            : office.StaffMembers
                .Where(e => e.Active)
                .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();
    }

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        FindAsync(id, token);

    public Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        GetListAsync(token);
}
