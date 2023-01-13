using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Complaints;
using Cts.TestData;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.Pagination;

namespace IntegrationTests.Complaints;

[NonParallelizable]
public class GetPagedList
{
    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var itemsCount = ComplaintData.GetComplaints.Count();
        var sorting = SortBy.IdDesc.GetDescription();
        var paging = new PaginatedRequest(1, itemsCount, sorting);

        var result = await repository.GetPagedListAsync(paging);

        using (new AssertionScope())
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeInDescendingOrder(e => e.Id);
        }
    }
}
