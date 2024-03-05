using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Cts.Domain.ValueObjects;

[Owned]
public record PhoneNumber : ValueObject
{
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string? Number { get; [UsedImplicitly] init; }

    [Display(Name = "Phone type")]
    [Column(TypeName = "nvarchar(25)")]
    public PhoneType? Type { get; [UsedImplicitly] init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number ?? string.Empty;
        // ReSharper disable once HeapView.BoxingAllocation
        yield return Type ?? PhoneType.Unknown;
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PhoneType
{
    [UsedImplicitly] Cell = 0,
    [UsedImplicitly] Fax = 1,
    [UsedImplicitly] Home = 2,
    [UsedImplicitly] Office = 3,
    [UsedImplicitly] Unknown = 4,
}
