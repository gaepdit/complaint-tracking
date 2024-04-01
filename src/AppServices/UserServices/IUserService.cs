using Cts.Domain.Identity;
using System.Security.Claims;

namespace Cts.AppServices.UserServices;

public interface IUserService
{
    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<ApplicationUser> GetUserAsync(string id);
    public Task<ApplicationUser?> FindUserAsync(string id);
    public ClaimsPrincipal? GetCurrentPrincipal();
}
