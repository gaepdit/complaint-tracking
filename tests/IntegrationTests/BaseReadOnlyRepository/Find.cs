using Cts.Domain.Offices;
using Cts.TestData;

namespace IntegrationTests.BaseReadOnlyRepository;

public class Find
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await repository.FindAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var result = await repository.FindAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
