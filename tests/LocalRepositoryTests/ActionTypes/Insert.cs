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

        var insertResult = await _repository.InsertAsync(newItem);
        var getResult = await _repository.GetAsync(newItem.Id);

        Assert.Multiple(() =>
        {
            insertResult.Should().BeEquivalentTo(newItem);
            getResult.Should().BeEquivalentTo(newItem);
            insertResult.Should().BeEquivalentTo(getResult);
            _repository.Items.Count.Should().Be(initialCount + 1);
        });
    }
}
