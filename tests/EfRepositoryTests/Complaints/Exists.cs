using Cts.Domain.Complaints;
using Cts.TestData;

namespace EfRepositoryTests.Complaints;

public class Exists
{
    private IComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsTrue()
    {
        var item = ComplaintData.GetComplaints.First();
        var result = await _repository.ExistsAsync(e => e.Id == item.Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.ExistsAsync(e => e.Id == -1);
        result.Should().BeFalse();
    }
}
