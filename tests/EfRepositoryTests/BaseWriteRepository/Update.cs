using Cts.Domain.Entities.Concerns;
using Cts.TestData;
using Cts.TestData.Constants;
using GaEpd.AppLibrary.Domain.Repositories;

namespace EfRepositoryTests.BaseWriteRepository;

public class Update
{
    private RepositoryHelper _repositoryHelper = default!;
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        _repository = _repositoryHelper.GetConcernRepository();
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
        var item = ConcernData.GetConcerns.First(e => e.Active);
        item.ChangeName(TestConstants.ValidName);
        item.Active = !item.Active;

        await _repository.UpdateAsync(item);
        _repositoryHelper.ClearChangeTracker();

        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenAutoSaveIsFalse_UpdateIsNotCommitted()
    {
        var item = ConcernData.GetConcerns.First(e => e.Active);
        var originalItem = new Concern(item.Id, item.Name);

        item.ChangeName(TestConstants.ValidName);
        item.Active = !item.Active;

        await _repository.UpdateAsync(item, false);
        _repositoryHelper.ClearChangeTracker();

        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(originalItem);
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new Concern(Guid.Empty, TestConstants.ValidName);
        var action = async () => await _repository.UpdateAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Concern).FullName}, id: {item.Id}");
    }
}
