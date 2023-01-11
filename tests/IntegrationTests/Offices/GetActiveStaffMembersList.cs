using Cts.Domain.Offices;
using Cts.TestData;
using GaEpd.AppLibrary.Domain.Repositories;

namespace IntegrationTests.Offices;

public class GetActiveStaffMembersList
{
    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.Last(e => e.Active);
        var result = await repository.GetActiveStaffMembersListAsync(item.Id);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_Throws()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var id = Guid.Empty;
        var action = async () => await repository.GetActiveStaffMembersListAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {id}");
    }
}
