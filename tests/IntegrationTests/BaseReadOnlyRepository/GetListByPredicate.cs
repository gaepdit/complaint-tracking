using Cts.TestData;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace IntegrationTests.BaseReadOnlyRepository;

public class GetListByPredicate
{
    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await repository.GetListAsync(e => e.Name == item.Name);
        using (new AssertionScope())
        {
            result.Count.Should().Be(1);
            result.First().Should().BeEquivalentTo(item);
        }
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var result = await repository.GetListAsync(e => e.Name == TestConstants.NonExistentName);
        result.Should().BeEmpty();
    }
}
