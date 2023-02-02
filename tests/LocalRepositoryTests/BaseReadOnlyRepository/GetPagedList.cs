using Cts.LocalRepository.Repositories;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;
using System.Globalization;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class GetPagedList
{
    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        using var repository = new LocalOfficeRepository();
        var itemsCount = repository.Items.Count;
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(repository.Items);
        }
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        using var repository = new LocalOfficeRepository();
        repository.Items.Clear();
        var paging = new PaginatedRequest(1, 100);

        var result = await repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        using var repository = new LocalOfficeRepository();
        var paging = new PaginatedRequest(2, repository.Items.Count);
        var result = await repository.GetPagedListAsync(paging);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        using var repository = new LocalOfficeRepository();
        var itemsCount = repository.Items.Count;
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(repository.Items);
            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            result.Should().BeInDescendingOrder(e => e.Name, comparer);
        }
    }
}
