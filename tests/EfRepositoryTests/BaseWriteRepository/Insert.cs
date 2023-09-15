using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;
using GaEpd.AppLibrary.Domain.Repositories;

namespace EfRepositoryTests.BaseWriteRepository;

public class Insert
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
    public async Task WhenItemIsValid_InsertsItem()
    {
        var item = new Concern(Guid.NewGuid(), TextData.ValidName);

        await _repository.InsertAsync(item);
        _repositoryHelper.ClearChangeTracker();

        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenAutoSaveIsFalse_NothingIsInserted()
    {
        var item = new Concern(Guid.NewGuid(), TextData.ValidName);

        await _repository.InsertAsync(item, false);
        _repositoryHelper.ClearChangeTracker();

        var action = async () => await _repository.GetAsync(item.Id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Concern).FullName}, id: {item.Id}");
    }
}
