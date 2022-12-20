using Cts.Domain.ValueObjects;

namespace Cts.TestData.Constants;

public static class ValueObjectData
{
    public static readonly PhoneNumber SampleNumber = new(TestConstants.ValidPhoneNumber, PhoneType.Office);

    public static readonly Address FullAddress = new()
    {
        Street = "123 Main St.",
        Street2 = "Box 456",
        City = "Town-ville",
        PostalCode = "98765-0000",
        State = "Georgia",
    };

    public static readonly Address LessAddress = new()
    {
        Street = "123 Main St.",
        Street2 = null,
        City = "Town-ville",
        PostalCode = "98765",
        State = "GA",
    };

    public static readonly Address IncompleteAddress = new()
    {
        Street = "123 Main St.",
        Street2 = null,
        City = "",
        PostalCode = "",
        State = "GA",
    };
}
