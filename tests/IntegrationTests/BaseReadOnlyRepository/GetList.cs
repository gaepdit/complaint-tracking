using Cts.Domain.Offices;
using Cts.TestData;

namespace IntegrationTests.BaseReadOnlyRepository;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var result = await repository.GetListAsync();
        result.Should().BeEquivalentTo(OfficeData.GetOffices);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        using var helper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = helper.GetOfficeRepository();
        await helper.ClearTableAsync<Office>();
        var result = await repository.GetListAsync();
        result.Should().BeEmpty();
    }
}
