using Cts.Domain.ValueObjects;

namespace Cts.TestData.Constants;

public static class ValueObjectData
{
    public static readonly PhoneNumber SampleNumber = new(TestConstants.ValidPhoneNumber, PhoneType.Office);
    public static readonly PhoneNumber AlternateNumber = new(TestConstants.AlternatePhoneNumber, PhoneType.Cell);

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
        Street = "456 Second St.",
        Street2 = null,
        City = "Town-ville",
        PostalCode = "98765",
        State = "GA",
    };

    public static readonly Address IncompleteAddress = new()
    {
        Street = "789 Third St.",
        Street2 = null,
        City = "",
        PostalCode = "",
        State = "GA",
    };
    public static readonly Address AlternateFullAddress = new()
    {
        Street = "2000 Alternate St.",
        Street2 = "Box 2000",
        City = "Alt-ville",
        PostalCode = "98765-2000",
        State = "Florida",
    };
}
