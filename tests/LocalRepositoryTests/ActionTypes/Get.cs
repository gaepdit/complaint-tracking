using Cts.Domain.Entities;
using Cts.LocalRepository;
using GaEpd.Library.Domain.Repositories;

namespace LocalRepositoryTests.ActionTypes;

public class Get
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
