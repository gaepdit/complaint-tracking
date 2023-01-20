using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Cts.TestData;
using Cts.TestData.Identity;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalOfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();

    public new Task InsertAsync(Office entity, bool autoSave = true, CancellationToken token = default)
    {
        entity.Assignor = IdentityData.GetUsers.SingleOrDefault(e => e.Id == entity.AssignorId);
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public new async Task UpdateAsync(Office entity, bool autoSave = true, CancellationToken token = default)
    {
        var item = await GetAsync(entity.Id, token);
        item.Assignor = IdentityData.GetUsers.SingleOrDefault(e => e.Id == entity.AssignorId);
        Items.Remove(item);
        Items.Add(entity);
    }
}
