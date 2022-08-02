using Cts.LocalRepository;

namespace LocalRepositoryTests.ActionTypes;

public class FindByName
{
    private ActionTypeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new ActionTypeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync(Constants.NonExistentName);
        result.Should().BeNull();
    }
}
