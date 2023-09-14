using Cts.Domain.Entities.Offices;
using Cts.Domain.Exceptions;
using Cts.TestData.Constants;

namespace DomainTests.Offices.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Office?)null);
        var manager = new OfficeManager(repoMock);

        var newItem = await manager.CreateAsync(TextData.ValidName, null);

        newItem.Name.Should().BeEquivalentTo(TextData.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.Empty, TextData.ValidName));
        var manager = new OfficeManager(repoMock);

        var office = async () => await manager.CreateAsync(TextData.ValidName, null);

        (await office.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TextData.ValidName}");
    }
}
