using Cts.Domain.Entities;
using Cts.Domain.Offices;
using Cts.TestData.Offices;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalOfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Name == name));

    public async Task<List<ApplicationUser>> GetActiveUsersListAsync(Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).Users.Where(e => e.Active).ToList();
}
