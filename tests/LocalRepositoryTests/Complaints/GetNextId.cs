using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.Complaints;

public class GetNextId
{
    [Test]
    public void GivenLocalRepository_ReturnsNextIdNumber()
    {
        var repository = new LocalComplaintRepository(Substitute.For<IAttachmentRepository>(),
            Substitute.For<IActionRepository>(), Substitute.For<IComplaintTransitionRepository>());
        var maxId = repository.Items.Max(e => e.Id);
        repository.GetNextId().Should().Be(maxId + 1);
    }
}
