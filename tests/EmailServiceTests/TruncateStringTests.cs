using GaEpd.EmailService.Utilities;

namespace EmailServiceTests;

public class TruncateStringTests
{
    private const string Value = "abc";

    [Test]
    public void Truncate_WithNegativeMaxLength_Throws()
    {
        var func = () => Value.Truncate(maxLength: -1);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Truncate_WithNullValue_ReturnsNull()
    {
        var result = ((string?)null).Truncate(maxLength: 1);
        result.Should().BeNull();
    }

    [Test]
    public void Truncate_WithEmptyValue_ReturnsEmptyString()
    {
        var result = string.Empty.Truncate(maxLength: 1);
        result.Should().Be(string.Empty);
    }

    [Test]
    public void Truncate_WithZeroMaxLength_ReturnsEmptyString()
    {
        var result = Value.Truncate(maxLength: 0);
        result.Should().Be(string.Empty);
    }

    [Test]
    public void Truncate_ValueShorterThanMaxLength_ReturnsOriginalValue()
    {
        var result = Value.Truncate(maxLength: 4);
        result.Should().Be(Value);
    }

    [Test]
    public void Truncate_ValueEqualToMaxLength_ReturnsOriginalValue()
    {
        var result = Value.Truncate(maxLength: Value.Length);
        result.Should().Be(Value);
    }

    [Test]
    public void Truncate_ValueLongerThanMaxLength_ReturnsTruncatedValue()
    {
        var result = Value.Truncate(maxLength: 2);
        result.Should().Be("a…");
    }

    [Test]
    public void Truncate_WithCustomSuffix_ReturnsTruncatedValueAndSuffix()
    {
        var result = Value.Truncate(maxLength: 2, suffix: "x");
        result.Should().Be("ax");
    }

    [Test]
    public void Truncate_WithLongerCustomSuffix_ReturnsTruncatedValueAndSuffix()
    {
        var result = "abcde".Truncate(maxLength: 4, suffix: "xy");
        result.Should().Be("abxy");
    }

    [Test]
    public void Truncate_SuffixEqualToMaxLength_ReturnsOnlySuffix()
    {
        const string suffix = "xy";
        var result = Value.Truncate(maxLength: suffix.Length, suffix: suffix);
        result.Should().Be(suffix);
    }

    [Test]
    public void Truncate_SuffixLongerThanMaxLength_ReturnsTruncatedValueWithoutSuffix()
    {
        var result = Value.Truncate(maxLength: 2, suffix: "xyz");
        result.Should().Be("ab");
    }
}
