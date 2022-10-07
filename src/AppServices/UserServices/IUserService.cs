using Cts.Domain.Identity;

namespace Cts.AppServices.UserServices;

public interface IUserService
{
    public Task<ApplicationUser?> GetCurrentUserAsync();
}
