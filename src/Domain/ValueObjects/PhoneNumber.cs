using GaEpd.AppLibrary.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Cts.Domain.ValueObjects;

[Owned]
public record PhoneNumber : ValueObject
{
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string? Number { get; [UsedImplicitly] init; } = string.Empty;

    [Display(Name = "Phone type")]
    public PhoneType? Type { get; [UsedImplicitly] init; } = PhoneType.Unknown;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number ?? string.Empty;
        yield return Type ?? PhoneType.Unknown;
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PhoneType
{
    Cell = 0,
    Fax = 1,
    Home = 2,
    Office = 3,
    Unknown = 4,
}
