using Cts.Domain.Entities.Concerns;
using Cts.TestData;
using Cts.TestData.Constants;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;
using System.Globalization;

namespace EfRepositoryTests.BaseReadRepository;

public class GetPagedListByPredicate
{
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        var itemsCount = ConcernData.GetConcerns.Count();
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await _repository.GetPagedListAsync(e => e.Name.Length > 0, paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(ConcernData.GetConcerns);
        }
    }

    [Test]
    public async Task WhenOneItemMatches_ReturnsListOfOne()
    {
        var itemsCount = ConcernData.GetConcerns.Count();
        var item = ConcernData.GetConcerns.First();
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await _repository.GetPagedListAsync(e => e.Id == item.Id, paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(1);
            result.First().Should().BeEquivalentTo(item);
        }
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var itemsCount = ConcernData.GetConcerns.Count();
        var paging = new PaginatedRequest(1, itemsCount);
        var result = await _repository.GetPagedListAsync(e => e.Name == TestConstants.NonExistentName, paging);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var itemsCount = ConcernData.GetConcerns.Count();
        var paging = new PaginatedRequest(2, itemsCount);
        var result = await _repository.GetPagedListAsync(e => e.Name.Length > 0, paging);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_SqliteDatabaseIsCaseSensitive_ReturnsSortedList()
    {
        var itemsCount = ConcernData.GetConcerns.Count();
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await _repository.GetPagedListAsync(e => e.Name.Length > 0, paging);

        using (new AssertionScope { FormattingOptions = { MaxLines = 1000 } })
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

        var itemsCount = ConcernData.GetConcerns.Count();
        var paging = new PaginatedRequest(1, itemsCount, "Name desc");

        var result = await repository.GetPagedListAsync(e => e.Name.Length > 0, paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(ConcernData.GetConcerns);
            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            result.Should().BeInDescendingOrder(e => e.Name, comparer);
        }
    }
}
