using Cts.TestData;
using Cts.TestData.Constants;

namespace IntegrationTests.ActionTypes;

public class FindByName
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetActionTypeRepository();
        var item = ActionTypeData.GetActionTypes.First(e => e.Active);
        var result = await repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetActionTypeRepository();
        var result = await repository.FindByNameAsync(TestConstants.NonExistentName);
        result.Should().BeNull();
    }
}
