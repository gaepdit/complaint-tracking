using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Staff.Dto;

public class StaffSearchResultDto
{
    public string Id { get; init; } = string.Empty;

    [UsedImplicitly]
    public string GivenName { get; init; } = string.Empty;

    [UsedImplicitly]
    public string FamilyName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;
    
    public string? OfficeName { get; init; }

    [UIHint("BoolActive")]
    public bool Active { get; init; }

    // Read-only properties

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));
}
