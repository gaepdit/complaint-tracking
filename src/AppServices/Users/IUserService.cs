﻿using Cts.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.Users;

public interface IUserService
{
    public Task<UserViewDto?> GetCurrentUserAsync(CancellationToken token = default);
    public Task<IList<string>> GetCurrentUserRolesAsync(CancellationToken token = default);
    public Task<List<UserViewDto>> FindUsersAsync(UserSearchDto filter, CancellationToken token = default);
    public Task<UserViewDto?> GetUserByIdAsync(string id, CancellationToken token = default);
    public Task<IList<string>> GetUserRolesAsync(string id, CancellationToken token = default);

    public Task<IdentityResult> UpdateUserRolesAsync(
        string id,
        Dictionary<string, bool> roleUpdates,
        CancellationToken token = default);
}

public record UserSearchDto
{
    public string? NameFilter { get; set; }
    public string? EmailFilter { get; set; }
    public string? Role { get; set; }
}

public class UserViewDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Office? Office { get; set; }
    public bool Active { get; set; }

    public string FullName =>
        string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrEmpty(s)));

    public string SortableFullName =>
        string.Join(", ", new[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));

    public string SelectableName =>
        SortableFullName + (Active ? "" : " [Inactive]");

    public string SelectableNameWithOffice =>
        SelectableName + (Office != null ? $" - {Office.Name}" : "");
}
