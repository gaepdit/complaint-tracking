using GaEpd.AppLibrary.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace MyAppRoot.Domain.ValueObjects;

[Owned]
public record Address : ValueObject
{
    public string Street { get; [UsedImplicitly] init; } = string.Empty;
    public string? Street2 { get; [UsedImplicitly] init; }
    public string City { get; [UsedImplicitly] init; } = string.Empty;
    public string State { get; [UsedImplicitly] init; } = string.Empty;

    [DataType(DataType.PostalCode)]
    public string PostalCode { get; [UsedImplicitly] init; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return Street2 ?? string.Empty;
        yield return City;
        yield return State;
        yield return PostalCode;
    }
}
