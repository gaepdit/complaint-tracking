using Microsoft.EntityFrameworkCore;

namespace Cts.Domain.ValueObjects;

[Owned]
public record Address
{
    [StringLength(100)]
    public string Street { get; init; } = string.Empty;

    [StringLength(100)]
    public string? Street2 { get; init; }

    [StringLength(50)]
    public string City { get; init; } = string.Empty;

    [StringLength(30)]
    public string State { get; init; } = string.Empty;

    [StringLength(10)]
    [DataType(DataType.PostalCode)]
    public string PostalCode { get; init; } = string.Empty;
}
