using Cts.Domain.Entities;

namespace Cts.AppServices.UserServices;

public interface IUserService
{
    public Task<ApplicationUser?> GetCurrentUserAsync();
}
