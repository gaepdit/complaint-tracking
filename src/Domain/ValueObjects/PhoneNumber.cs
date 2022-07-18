using Microsoft.EntityFrameworkCore;

namespace Cts.Domain.ValueObjects;

[Owned]
public class PhoneNumber
{
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string CallerPhoneNumber { get; set; } = string.Empty;

    public PhoneType? CallerPhoneType { get; set; }

    public enum PhoneType
    {
        Cell = 0,
        Fax = 1,
        Home = 2,
        Office = 3,
    }
}
