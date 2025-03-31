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
            .Where(user => user.Office != null && user.Office.Id == id)
            .Where(user => includeInactive || user.Active)
            .OrderBy(user => user.FamilyName).ThenBy(user => user.GivenName).ThenBy(user => user.Id)
            .ToList());

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        FindAsync(id, token: token);

    public Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        GetOrderedListAsync(token);
}
