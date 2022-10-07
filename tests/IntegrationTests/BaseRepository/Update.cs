using GaEpd.AppLibrary.Domain.Repositories;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;
using MyAppRoot.TestData.Offices;

namespace IntegrationTests.BaseRepository;

public class Update
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
    public async Task WhenItemIsValid_UpdatesItem()
    {
        var item = OfficeData.GetOffices.First(e => e.Active);
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
        var item = OfficeData.GetOffices.First(e => e.Active);
        var originalItem = new Office(item.Id, item.Name);

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
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var action = async () => await _repository.UpdateAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
