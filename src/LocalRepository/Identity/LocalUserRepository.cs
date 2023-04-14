using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.LocalRepository.Repositories;
using Cts.TestData.Identity;

namespace Cts.LocalRepository.Identity;

public sealed class LocalUserRepository : BaseReadRepository<ApplicationUser, string>, IUserRepository
{
    public LocalUserRepository() : base(UserData.GetUsers) { }
}
