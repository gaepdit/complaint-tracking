using Cts.Domain.Complaints;
using Cts.TestData;

namespace EfRepositoryTests.Complaints;

public class GetNextId
{
    private IComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task OnSuccessfulInsert_ReturnsNextIdNumber()
    {
        var maxId = ComplaintData.GetComplaints.Max(e => e.Id);
        var result = await _repository.GetNextIdAsync();
        result.Should().Be(maxId + 1);
    }
}
