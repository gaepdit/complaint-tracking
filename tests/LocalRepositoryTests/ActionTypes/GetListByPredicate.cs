using Cts.LocalRepository;
using Cts.TestData.ActionTypes;

namespace LocalRepositoryTests.ActionTypes;

public class GetListByPredicate
{
    private LocalActionTypeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalActionTypeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var item = _repository.Items.First();

        var result = await _repository.GetListAsync(e => e.Name == item.Name);

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(1);
            result[0].Should().BeEquivalentTo(item);
        });
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetListAsync(e => e.Name == ActionTypeConstants.NonExistentName);
        result.Should().BeEmpty();
    }
}
