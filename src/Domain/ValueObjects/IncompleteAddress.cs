using GaEpd.AppLibrary.Domain.ValueObjects;
using GaEpd.AppLibrary.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Cts.Domain.ValueObjects;

[Owned]
public record IncompleteAddress : ValueObject
{
    [Display(Name = "Street Address")]
    public string? Street { get; [UsedImplicitly] init; }

    [Display(Name = "Apt / Suite / Other")]
    public string? Street2 { get; [UsedImplicitly] init; }

    public string? City { get; [UsedImplicitly] init; }

    public string? State { get; [UsedImplicitly] init; }

    [DataType(DataType.PostalCode)]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; [UsedImplicitly] init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street ?? string.Empty;
        yield return Street2 ?? string.Empty;
        yield return City ?? string.Empty;
        yield return State ?? string.Empty;
        yield return PostalCode ?? string.Empty;
    }

    public string OneLine => new[] { Street, Street2, City, new[] { State, PostalCode }.ConcatWithSeparator() }
        .ConcatWithSeparator(", ");

    public static IncompleteAddress EmptyAddress => new();
    public bool IsEmpty => this == EmptyAddress;
}
