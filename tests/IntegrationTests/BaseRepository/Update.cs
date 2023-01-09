using Cts.Domain.Offices;
using Cts.TestData;
using Cts.TestData.Constants;
using GaEpd.AppLibrary.Domain.Repositories;

namespace IntegrationTests.BaseRepository;

public class Update
{
    [Test]
    public async Task WhenItemIsValid_UpdatesItem()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetOfficeRepository();
        
        var item = OfficeData.GetOffices.First(e => e.Active);
        item.ChangeName(TestConstants.ValidName);
        item.Active = !item.Active;

        await repository.UpdateAsync(item);
        repositoryHelper.ClearChangeTracker();

        var getResult = await repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }


    [Test]
    public async Task WhenAutoSaveIsFalse_UpdateIsNotCommitted()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetOfficeRepository();
        
        var item = OfficeData.GetOffices.First(e => e.Active);
        var originalItem = new Office(item.Id, item.Name);

        item.ChangeName(TestConstants.ValidName);
        item.Active = !item.Active;

        await repository.UpdateAsync(item, false);
        repositoryHelper.ClearChangeTracker();

        var getResult = await repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(originalItem);
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetOfficeRepository();
        
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var action = async () => await repository.UpdateAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
