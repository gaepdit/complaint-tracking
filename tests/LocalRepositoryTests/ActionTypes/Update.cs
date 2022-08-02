using Cts.Domain.ActionTypes;
using Cts.LocalRepository;
using GaEpd.Library.Domain.Repositories;

namespace LocalRepositoryTests.ActionTypes;

public class Update
{
    private ActionTypeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new ActionTypeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemIsValid_UpdatesItem()
    {
        var item = _repository.Items.First();

        item.ChangeName(Constants.ValidName);
        var updateResult = await _repository.UpdateAsync(item);
        var getResult = await _repository.GetAsync(item.Id);

        Assert.Multiple(() =>
        {
            updateResult.Should().BeEquivalentTo(item);
            getResult.Should().BeEquivalentTo(item);
            updateResult.Should().BeEquivalentTo(getResult);
        });
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new ActionType(Guid.Empty, Constants.ValidName);

        var action = async () => await _repository.UpdateAsync(item);

        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(ActionType).FullName}, id: {item.Id}");
    }
}
