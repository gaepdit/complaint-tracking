using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.Complaints;

public class GetComplaintTransitionsList
{
    private LocalComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var complaint = _repository.Items.First();

        var result = await _repository.GetComplaintTransitionsListAsync(complaint.Id);

        result.Should().BeEquivalentTo(_repository.ComplaintTransitionItems);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetComplaintTransitionsListAsync(0);
        result.Should().BeEmpty();
    }
}
