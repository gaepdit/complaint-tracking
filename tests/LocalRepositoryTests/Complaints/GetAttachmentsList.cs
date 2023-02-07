using Cts.AppServices.Attachments;
using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.Complaints;

public class GetAttachmentsList
{
    private LocalComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsTrue()
    {
        var complaint = _repository.Items.First();
        var items = _repository.AttachmentItems.Where(e => e.ComplaintId == complaint.Id && !e.IsDeleted);

        var result = await _repository.GetAttachmentsListAsync(AttachmentFilters.PublicIdPredicate(complaint.Id));

        result.Should().BeEquivalentTo(items);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetAttachmentsListAsync(e => e.Id == Guid.Empty);
        result.Should().BeEmpty();
    }
}
