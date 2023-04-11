using Cts.Domain.Entities.Complaints;
using Cts.TestData;
using EfRepositoryTests;

namespace LocalRepositoryTests.Complaints;

public class GetComplaintTransitionsList
{
    private IComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var complaint = ComplaintData.GetComplaints.First();
        var items = ComplaintTransitionData.GetComplaintTransitions.Where(e => e.Complaint.Id == complaint.Id);

        var result = await _repository.GetComplaintTransitionsListAsync(complaint.Id);

        result.Should().BeEquivalentTo(items, opts => opts
            .Excluding(e => e.CommittedByUser!.Office)
            .Excluding(e => e.TransferredFromUser!.Office)
            .Excluding(e => e.TransferredToUser!.Office)
            .Excluding(e => e.TransferredToOffice!.Assignor)
            .Excluding(e => e.TransferredToOffice!.StaffMembers)
            .Excluding(e => e.TransferredFromOffice!.Assignor)
            .Excluding(e => e.TransferredFromOffice!.StaffMembers)
            .Excluding(e => e.Complaint));
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetComplaintTransitionsListAsync(0);
        result.Should().BeEmpty();
    }
}
