using Cts.Domain.Users;
using GaEpd.Library.Domain.Repositories;

namespace Cts.Domain.Offices;

public interface IOfficeRepository : IRepository<Office, Guid>
{
    Task<Office> FindByName(string name);
    Task<List<ApplicationUser>> GetUsersList(Guid id);
}
