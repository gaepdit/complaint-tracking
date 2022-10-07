using MyAppRoot.Domain.Offices;

namespace DomainTests.Offices.Entity;

public class ChangeName
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var newGuid = Guid.NewGuid();
        var result = new Office(newGuid, newGuid.ToString());
        var newName = Guid.NewGuid().ToString();

        result.ChangeName(newName);

        result.Name.Should().Be(newName);
    }

    [Test]
    public void WithEmptyName_Throws()
    {
        var newGuid = Guid.NewGuid();
        var result = new Office(newGuid, newGuid.ToString());

        var action = () => result.ChangeName(string.Empty);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void WithShortName_Throws()
    {
        var newGuid = Guid.NewGuid();
        var result = new Office(newGuid, newGuid.ToString());

        var action = () => result.ChangeName("a");

        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{Office.MinNameLength}'.*");
    }
}
