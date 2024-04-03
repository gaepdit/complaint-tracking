using GaEpd.EmailService;

namespace EmailServiceTests;

public class MessageTests
{
    [Test]
    public void Create_WithEmptySubject_Throws()
    {
        var func = () => Message.Create("", ["b"], "c",
            textBody: "d", htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithNoRecipients_Throws()
    {
        var func = () => Message.Create("a", new List<string>(), "c",
            textBody: "d", htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithEmptyRecipient_Throws()
    {
        var func = () => Message.Create("a", [""], "c",
            textBody: "d", htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithNoBody_Throws()
    {
        var func = () => Message.Create("a", ["b"], "c",
            textBody: null, htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithTextBody_DoesNotThrow()
    {
        var func = () => Message.Create("a", ["b"], "c",
            textBody: "d", htmlBody: null);
        func.Should().NotThrow();
    }

    [Test]
    public void Create_WithHtmlBody_DoesNotThrow()
    {
        var func = () => Message.Create("a", ["b"], "c",
            textBody: null, htmlBody: "d");
        func.Should().NotThrow();
    }
}
