using GaEpd.AppLibrary.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Cts.Domain.ValueObjects;

[Owned]
public record IncompleteAddress : ValueObject
{
    [Display(Name = "Street Address")]
    public string? Street { get; [UsedImplicitly] init; }

    [Display(Name = "Apt / Suite / Other")]
    public string? Street2 { get; [UsedImplicitly] set; }

    public string? City { get; [UsedImplicitly] set; }

    public string? State { get; [UsedImplicitly] set; }

    [DataType(DataType.PostalCode)]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; [UsedImplicitly] set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street ?? string.Empty;
        yield return Street2 ?? string.Empty;
        yield return City ?? string.Empty;
        yield return State ?? string.Empty;
        yield return PostalCode ?? string.Empty;
    }
}
