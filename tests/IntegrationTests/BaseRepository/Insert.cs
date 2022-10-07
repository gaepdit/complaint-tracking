using GaEpd.AppLibrary.Domain.Repositories;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;

namespace IntegrationTests.BaseRepository;

public class Insert
{
    private RepositoryHelper _repositoryHelper = default!;
    private IOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        _repository = _repositoryHelper.GetOfficeRepository();
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
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);

        await _repository.InsertAsync(item);
        _repositoryHelper.ClearChangeTracker();

        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenAutoSaveIsFalse_NothingIsInserted()
    {
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);

        await _repository.InsertAsync(item, false);
        _repositoryHelper.ClearChangeTracker();

        var action = async () => await _repository.GetAsync(item.Id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
