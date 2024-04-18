using GaEpd.EmailService.Utilities;

namespace EmailServiceTests;

[TestFixture]
public class ConcatWithSeparatorTests
{
    [Test]
    public void ConcatWithSeparator_WithDefaultSeparator()
    {
        new[] { "a", "b" }.ConcatWithSeparator().Should().Be("a b");
    }

    [Test]
    public void ConcatWithSeparator_WithCustomSeparator()
    {
        new[] { "a", "b" }.ConcatWithSeparator("X").Should().Be("aXb");
    }

    [Test]
    public void ConcatWithSeparator_WithNullValues()
    {
        new[] { "a", null, "b", null }.ConcatWithSeparator().Should().Be("a b");
    }

    [Test]
    public void ConcatWithSeparator_WithEmptyStrings()
    {
        new[] { "a", "", "b", "" }.ConcatWithSeparator().Should().Be("a b");
    }
}
