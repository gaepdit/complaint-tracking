using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;
using Cts.TestData.Constants;
using GaEpd.AppLibrary.Domain.Repositories;

namespace IntegrationTests.BaseRepository;

public class Update
{
    private RepositoryHelper _repositoryHelper = default!;
    private IActionTypeRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        _repository = _repositoryHelper.GetActionTypeRepository();
    }

    [TearDown]
    public void TearDown()
    {
        _repository.Dispose();
        _repositoryHelper.Dispose();
    }

    [Test]
    public async Task WhenItemIsValid_UpdatesItem()
    {
        var item = ActionTypeData.GetActionTypes.First(e => e.Active);
        item.ChangeName(TestConstants.ValidName);
        item.Active = !item.Active;

        await _repository.UpdateAsync(item, true);
        _repositoryHelper.ClearChangeTracker();

        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new ActionType(Guid.Empty, TestConstants.ValidName);
        var action = async () => await _repository.UpdateAsync(item, true);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(ActionType).FullName}, id: {item.Id}");
    }
}
