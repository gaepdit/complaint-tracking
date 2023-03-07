using Cts.Domain.Concerns;
using Cts.TestData;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;
using System.Globalization;

namespace EfRepositoryTests.BaseReadRepository;

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
    public async Task WhenItemsExist_ReturnsPagedList()
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
    public async Task WhenDoesNotExist_ReturnsEmptyList()
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
    public async Task GivenSorting_SqliteDatabaseIsCaseSensitive_ReturnsSortedList()
    {
        var itemsCount = ConcernData.GetConcerns.Count;
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await _repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(ConcernData.GetConcerns);
            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.Ordinal);
            result.Should().BeInDescendingOrder(e => e.Name, comparer);
        }
    }

    [Test]
    public async Task GivenSorting_SqlServerDatabaseIsNotCaseSensitive_ReturnsSortedList()
    {
        using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        using var repository = repositoryHelper.GetConcernRepository();

        var itemsCount = ConcernData.GetConcerns.Count;
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(ConcernData.GetConcerns);
            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            result.Should().BeInDescendingOrder(e => e.Name, comparer);
        }
    }
}
