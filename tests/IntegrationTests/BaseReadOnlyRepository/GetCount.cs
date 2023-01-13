using Cts.Domain.Offices;
using Cts.TestData;

namespace IntegrationTests.BaseReadOnlyRepository;

public class GetCount
{
    [Test]
    public async Task WhenItemsExist_ReturnsCount()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.First();
        var result = await repository.CountAsync(e => e.Id == item.Id);
        result.Should().Be(1);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsZero()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var result = await repository.CountAsync(e => e.Id == Guid.Empty);
        result.Should().Be(0);
    }
}
