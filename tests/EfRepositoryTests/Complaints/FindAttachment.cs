using Cts.Domain.Complaints;
using Cts.TestData;

namespace EfRepositoryTests.Complaints;

public class FindAttachment
{
    private IComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsItem()
    {
        var item = AttachmentData.GetAttachments.First();
        var result = await _repository.FindAttachmentAsync(item.Id);
        result.Should().BeEquivalentTo(item, opts => opts
            .Excluding(e => e.UploadedBy!.Office)
            .Excluding(e => e.Complaint));
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindAttachmentAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
