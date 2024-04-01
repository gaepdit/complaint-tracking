using Cts.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Cts.AppServices.UserServices;

public class UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    : IUserService
{
    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var principal = GetCurrentPrincipal();
        return principal is null ? null : await userManager.GetUserAsync(principal).ConfigureAwait(false);
    }

    public async Task<ApplicationUser> GetUserAsync(string id) =>
        await FindUserAsync(id).ConfigureAwait(false)
        ?? throw new EntityNotFoundException<ApplicationUser>(id);

    public Task<ApplicationUser?> FindUserAsync(string id) =>
        userManager.FindByIdAsync(id);

    public ClaimsPrincipal? GetCurrentPrincipal() => httpContextAccessor.HttpContext?.User;
}
