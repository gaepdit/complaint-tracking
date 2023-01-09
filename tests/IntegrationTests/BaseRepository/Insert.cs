using Cts.Domain.Offices;
using Cts.TestData.Constants;
using GaEpd.AppLibrary.Domain.Repositories;

namespace IntegrationTests.BaseRepository;

public class Insert
{
    [Test]
    public async Task WhenItemIsValid_InsertsItem()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetOfficeRepository();
        
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);

        await repository.InsertAsync(item);
        repositoryHelper.ClearChangeTracker();

        var getResult = await repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenAutoSaveIsFalse_NothingIsInserted()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetOfficeRepository();
        
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);

        await repository.InsertAsync(item, false);
        repositoryHelper.ClearChangeTracker();

        var action = async () => await repository.GetAsync(item.Id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
