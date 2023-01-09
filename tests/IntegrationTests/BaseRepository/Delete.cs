using Cts.Domain.Offices;
using Cts.TestData.Constants;
using GaEpd.AppLibrary.Domain.Repositories;

namespace IntegrationTests.BaseRepository;

public class Delete
{
    [Test]
    public async Task WhenItemExists_DeletesItem()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetOfficeRepository();
        // Arrange
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);
        await repository.InsertAsync(item);
        repositoryHelper.ClearChangeTracker();

        // (Still part of arrange...)
        var getResult = await repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);

        // Act
        await repository.DeleteAsync(item);
        repositoryHelper.ClearChangeTracker();

        // Assert
        var result = await repository.FindAsync(item.Id);
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenAutoSaveIsFalse_NothingIsDeleted()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetOfficeRepository();
        
        // Arrange
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);
        await repository.InsertAsync(item);
        repositoryHelper.ClearChangeTracker();

        // Act
        await repository.DeleteAsync(item, false);
        repositoryHelper.ClearChangeTracker();

        // Assert
        var getResult = await repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var action = async () => await repository.DeleteAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
