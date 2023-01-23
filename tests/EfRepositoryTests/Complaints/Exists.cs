using Cts.Domain.Complaints;
using Cts.TestData;

namespace EfRepositoryTests.Complaints;

public class Exists
{
    [Test]
    public async Task WhenItemsExist_ReturnsTrue()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var item = ComplaintData.GetComplaints.First();
        var result = await repository.ExistsAsync(e => e.Id == item.Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var result = await repository.ExistsAsync(e => e.Id == -1);
        result.Should().BeFalse();
    }
}
