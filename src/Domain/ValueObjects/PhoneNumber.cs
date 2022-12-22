using GaEpd.AppLibrary.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Cts.Domain.ValueObjects;

[Owned]
public record PhoneNumber : ValueObject
{
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string Number { get; [UsedImplicitly] init; } = string.Empty;

    public PhoneType Type { get; [UsedImplicitly] init; } = PhoneType.Unknown;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return Type;
    }
}

public enum PhoneType
{
    Cell = 0,
    Fax = 1,
    Home = 2,
    Office = 3,
    Unknown = 4,
}
