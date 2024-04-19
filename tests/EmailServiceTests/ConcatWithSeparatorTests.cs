using GaEpd.EmailService.Utilities;

namespace EmailServiceTests;

[TestFixture]
public class ConcatWithSeparatorTests
{
    private static readonly string[] Items = ["a", "b"];
    private static readonly string[] ItemsWithEmptyValues = ["a", "", "b", ""];
    private static readonly string?[] ItemsWithNullValues = ["a", null, "b", null];

    [Test]
    public void ConcatWithSeparator_WithDefaultSeparator()
    {
        Items.ConcatWithSeparator().Should().Be("a b");
    }

    [Test]
    public void ConcatWithSeparator_WithCustomSeparator()
    {
        Items.ConcatWithSeparator("X").Should().Be("aXb");
    }

    [Test]
    public void ConcatWithSeparator_WithEmptyStrings()
    {
        ItemsWithEmptyValues.ConcatWithSeparator().Should().Be("a b");
    }

    [Test]
    public void ConcatWithSeparator_WithNullValues()
    {
        ItemsWithNullValues.ConcatWithSeparator().Should().Be("a b");
    }
}
