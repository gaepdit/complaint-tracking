using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;
using GaEpd.AppLibrary.Domain.Repositories;

namespace IntegrationTests.BaseReadOnlyRepository;

public class Get
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
        var result = await _repository.GetAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var action = async () => await _repository.GetAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(ActionType).FullName}, id: {id}");
    }
}
