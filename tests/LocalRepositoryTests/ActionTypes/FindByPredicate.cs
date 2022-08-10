using Cts.LocalRepository;
using Cts.TestData.ActionTypes;

namespace LocalRepositoryTests.ActionTypes;

public class FindByPredicate
{
    private LocalActionTypeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalActionTypeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.FindAsync(e => e.Name == item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindAsync(e => e.Name == ActionTypeConstants.NonExistentName);
        result.Should().BeNull();
    }
}
