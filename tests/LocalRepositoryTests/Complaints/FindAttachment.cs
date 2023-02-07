using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class FindAttachment
{
    private LocalComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsTrue()
    {
        var item = _repository.AttachmentItems.First();
        var result = await _repository.FindAttachmentAsync(item.Id);
        result.Should().Be(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.FindAttachmentAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
