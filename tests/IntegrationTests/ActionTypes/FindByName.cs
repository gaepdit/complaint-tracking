using Cts.Domain.ActionTypes;
using Cts.TestData;
using Cts.TestData.Constants;

namespace IntegrationTests.ActionTypes;

public class FindByName
{
    private IActionTypeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetActionTypeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = ActionTypeData.GetActionTypes.First(e => e.Active);
        var result = await _repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync(TestConstants.NonExistentName);
        result.Should().BeNull();
    }
}
