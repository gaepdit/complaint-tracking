namespace DomainTests.DataProcessing;

public class PersonalInformation
{
    [Test]
    public void PiiShouldBeRemoved()
    {
        const string data = "Phone: 404-555-1212; Email: test@example.net!";
        var result = Cts.Domain.DataProcessing.PersonalInformation.RedactPii(data);
        result.Should().Be("Phone: 404-[phone number removed]; Email: [email@removed.invalid]!");
    }
}
