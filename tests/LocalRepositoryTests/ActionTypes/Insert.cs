using Cts.Domain.Entities;
using Cts.LocalRepository;
using Cts.TestData.ActionTypes;

namespace LocalRepositoryTests.ActionTypes;

public class Insert
{
    private LocalActionTypeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalActionTypeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemIsValid_InsertsItem()
    {
        var initialCount = _repository.Items.Count;
        var newItem = new ActionType(Guid.NewGuid(), ActionTypeConstants.ValidName);

        await _repository.InsertAsync(newItem);

        var getResult = await _repository.GetAsync(newItem.Id);
        Assert.Multiple(() =>
        {
            getResult.Should().BeEquivalentTo(newItem);
            _repository.Items.Count.Should().Be(initialCount + 1);
        });
    }
}
