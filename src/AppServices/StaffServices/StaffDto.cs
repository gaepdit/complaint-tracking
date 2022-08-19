using Cts.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.StaffServices;

public record StaffSearchDto
{
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Role { get; init; }
    public Guid? Office { get; init; }
    public ActiveStatus Status { get; init; } = ActiveStatus.Active;

    public enum ActiveStatus
    {
        Active,
        Inactive,
        All,
    }

    public void TrimAll()
    {
        Name = Name?.Trim();
        Email = Email?.Trim();
    }
}

public class StaffViewDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public Office? Office { get; init; }

    [UIHint("BoolActive")]
    public bool Active { get; init; } = true;

    public string FullName =>
        string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrEmpty(s)));

    public string SortableFullName =>
        string.Join(", ", new[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));
}

public class StaffUpdateDto
{
    public Guid Id { get; init; }

    [StringLength(ApplicationUser.MaxPhoneLength)]
    public string? Phone { get; init; }

    public Office? Office { get; init; }
    public bool Active { get; init; }
}
