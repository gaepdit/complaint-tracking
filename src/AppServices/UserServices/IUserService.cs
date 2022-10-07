using MyAppRoot.Domain.Identity;

namespace MyAppRoot.AppServices.UserServices;

public interface IUserService
{
    public Task<ApplicationUser?> GetCurrentUserAsync();
}
