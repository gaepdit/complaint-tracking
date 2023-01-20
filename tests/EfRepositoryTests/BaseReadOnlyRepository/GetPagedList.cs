using Cts.Domain.Concerns;
using Cts.TestData;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;

namespace EfRepositoryTests.BaseReadOnlyRepository;

public class GetPagedList
{
    private RepositoryHelper _helper = default!;
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = RepositoryHelper.CreateRepositoryHelper();
        _repository = _helper.GetConcernRepository();
    }

    [TearDown]
    public void TearDown()
    {
        _repository.Dispose();
        _helper.Dispose();
    }

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var itemsCount = ConcernData.GetConcerns.Count;
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await _repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(ConcernData.GetConcerns);
        }
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await _helper.ClearTableAsync<Concern>();
        var paging = new PaginatedRequest(1, 100);

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var itemsCount = ConcernData.GetConcerns.Count;
        var paging = new PaginatedRequest(2, itemsCount);
        var result = await _repository.GetPagedListAsync(paging);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var itemsCount = ConcernData.GetConcerns.Count;
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeInDescendingOrder(e => e.Name);
    }
}
