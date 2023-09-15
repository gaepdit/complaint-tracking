using Cts.LocalRepository.Repositories;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;
using System.Globalization;

namespace LocalRepositoryTests.BaseReadRepository;

public class GetPagedList
{
    private LocalConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var itemsCount = _repository.Items.Count;
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await _repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(_repository.Items);
        }
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        _repository.Items.Clear();
        var paging = new PaginatedRequest(1, 100);

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var paging = new PaginatedRequest(2, _repository.Items.Count);
        var result = await _repository.GetPagedListAsync(paging);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var itemsCount = _repository.Items.Count;
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await _repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(_repository.Items);
            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            result.Should().BeInDescendingOrder(e => e.Name, comparer);
        }
    }
}
