﻿using AutoMapper;
using Cts.Domain.Entities;
using Cts.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.Users;

public interface IUserService
{
    public Task<UserViewDto?> GetCurrentUserAsync();
    public Task<IList<string>> GetCurrentUserRolesAsync();
    public Task<List<UserViewDto>> FindUsersAsync(string? nameFilter, string? emailFilter, string? role);
    public Task<UserViewDto?> GetUserByIdAsync(string id);
    public Task<IList<string>> GetUserRolesAsync(string id);
    public Task<IdentityResult> UpdateUserRolesAsync(string id, Dictionary<string, bool> roleUpdates);
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

public class UsersMappingProfile : Profile
{
    public UsersMappingProfile() => CreateMap<ApplicationUser, UserViewDto>();
}