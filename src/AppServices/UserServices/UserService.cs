using Cts.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.UserServices;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        return principal == null ? null : await _userManager.GetUserAsync(principal);
    }

    public Task<ApplicationUser> GetUserAsync(string id) =>
        _userManager.FindByIdAsync(id)
        ?? throw new EntityNotFoundException(typeof(ApplicationUser), id);

    public async Task<ApplicationUser?> FindUserAsync(string id) =>
        await _userManager.FindByIdAsync(id) ?? null as ApplicationUser;
}
