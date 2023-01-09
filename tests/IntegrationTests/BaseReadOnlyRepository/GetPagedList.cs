using Cts.Domain.Offices;
using Cts.TestData;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;

namespace IntegrationTests.BaseReadOnlyRepository;

public class GetPagedList
{
    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var itemsCount = OfficeData.GetOffices.Count();
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(OfficeData.GetOffices);
        }
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        using var helper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = helper.GetOfficeRepository();
        await helper.ClearTableAsync<Office>();
        var paging = new PaginatedRequest(1, 100);

        var result = await repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var itemsCount = OfficeData.GetOffices.Count();
        var paging = new PaginatedRequest(2, itemsCount);
        var result = await repository.GetPagedListAsync(paging);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var itemsCount = OfficeData.GetOffices.Count(); 
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(OfficeData.GetOffices);
            result.Should().BeInDescendingOrder(e => e.Name);
        }
    }
}
