using Cts.Domain.Users;
using GaEpd.Library.Domain.Repositories;

namespace Cts.Domain.Offices;

public interface IOfficeRepository : IRepository<Office, Guid>
{
    Task<Office> FindByNameAsync(string name);
    Task<List<ApplicationUser>> GetUsersListAsync(Guid id);
}
