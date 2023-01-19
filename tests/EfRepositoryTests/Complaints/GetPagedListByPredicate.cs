using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Complaints;
using Cts.TestData;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.Pagination;

namespace EfRepositoryTests.Complaints;

public class GetPagedListByPredicate
{
    private IComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var itemsCount = ComplaintData.GetComplaints.Count();
        var sorting = SortBy.IdDesc.GetDescription();
        var paging = new PaginatedRequest(1, itemsCount, sorting);

        var result = await _repository.GetPagedListAsync(e => e.DateReceived >= DateTime.MinValue, paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeInDescendingOrder(e => e.Id);
        }
    }
}
