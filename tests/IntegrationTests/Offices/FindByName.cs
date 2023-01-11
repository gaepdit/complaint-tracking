using Cts.Domain.Offices;
using Cts.TestData;
using Cts.TestData.Constants;

namespace IntegrationTests.Offices;

public class FindByName
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item, opts =>
            opts.Excluding(e => e.StaffMembers));
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var result = await repository.FindByNameAsync(TestConstants.NonExistentName);
        result.Should().BeNull();
    }
}
