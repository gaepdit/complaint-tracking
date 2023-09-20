using GaEpd.AppLibrary.Extensions;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Staff.Dto;

public record StaffSearchResultDto
(
    string Id,
    string GivenName,
    string FamilyName,
    string Email,
    string? OfficeName,
    bool Active
)
{
    [JsonIgnore]
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");
}
