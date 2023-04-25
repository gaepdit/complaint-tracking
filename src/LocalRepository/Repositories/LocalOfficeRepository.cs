using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData;
using Cts.TestData.Identity;

namespace Cts.LocalRepository.Repositories;

/// <inheritdoc cref="IOfficeRepository" />
public sealed class LocalOfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));

    public Task<List<ApplicationUser>> GetStaffMembersListAsync(
        Guid id, bool activeOnly, CancellationToken token = default) =>
        Task.FromResult(UserData.GetUsers
            .Where(e => e.Office?.Id == id)
            .Where(e => !activeOnly || e.Active)
            .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName)
            .ToList());

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        FindAsync(id, token);

    public Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        GetListAsync(token);
}
