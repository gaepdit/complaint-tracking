using Cts.Domain.Entities;
using Cts.Domain.Users;
using GaEpd.Library.Domain.Repositories;

namespace Cts.Domain.Offices;

public interface IOfficeRepository : IRepository<Office, Guid>
{
    Task<Office> FindByNameAsync(string name, CancellationToken token = default);
    Task<List<ApplicationUser>> GetUsersListAsync(Guid id, CancellationToken token = default);
}
