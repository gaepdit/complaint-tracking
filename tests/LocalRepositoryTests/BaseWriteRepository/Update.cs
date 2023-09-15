using Cts.Domain.Entities.Concerns;
using Cts.LocalRepository.Repositories;
using Cts.TestData.Constants;
using GaEpd.AppLibrary.Domain.Repositories;

namespace LocalRepositoryTests.BaseWriteRepository;

public class Update
{
    private LocalConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemIsValid_UpdatesItem()
    {
        var item = _repository.Items.First();
        item.ChangeName(TextData.ValidName);
        item.Active = !item.Active;

        await _repository.UpdateAsync(item);

        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new Concern(Guid.Empty, TextData.ValidName);
        var action = async () => await _repository.UpdateAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Concern).FullName}, id: {item.Id}");
    }
}
