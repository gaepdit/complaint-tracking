using Cts.AppServices.Complaints.Dto;
using Cts.LocalRepository.Repositories;
using Cts.WebApp.Platform.PageDisplayHelpers;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;

namespace LocalRepositoryTests.Complaints;

public class GetPagedListByPredicate
{
    private LocalComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var itemsCount = _repository.Items.Count;
        var sorting = SortBy.IdDesc.GetDescription();
        var paging = new PaginatedRequest(1, itemsCount, sorting);

        var result = await _repository.GetPagedListAsync(e => e.DateReceived >= DateTime.MinValue, paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(_repository.Items);
            result.Should().BeInDescendingOrder(e => e.Id);
        }
    }
}
