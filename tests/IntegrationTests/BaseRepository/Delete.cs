using GaEpd.AppLibrary.Domain.Repositories;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;

namespace IntegrationTests.BaseRepository;

public class Delete
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
    public async Task WhenItemExists_DeletesItem()
    {
        // Arrange
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);
        await _repository.InsertAsync(item);
        _repositoryHelper.ClearChangeTracker();

        // (Still part of arrange...)
        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);

        // Act
        await _repository.DeleteAsync(item);
        _repositoryHelper.ClearChangeTracker();

        // Assert
        var result = await _repository.FindAsync(item.Id);
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenAutoSaveIsFalse_NothingIsDeleted()
    {
        // Arrange
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);
        await _repository.InsertAsync(item);
        _repositoryHelper.ClearChangeTracker();

        // Act
        await _repository.DeleteAsync(item, false);
        _repositoryHelper.ClearChangeTracker();

        // Assert
        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var action = async () => await _repository.DeleteAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
