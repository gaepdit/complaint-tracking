using Cts.Domain.Offices;
using Cts.TestData;
using GaEpd.AppLibrary.Domain.Repositories;

namespace IntegrationTests.BaseReadOnlyRepository;

public class Get
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await repository.GetAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_Throws()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var id = Guid.Empty;
        var action = async () => await repository.GetAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {id}");
    }
}
