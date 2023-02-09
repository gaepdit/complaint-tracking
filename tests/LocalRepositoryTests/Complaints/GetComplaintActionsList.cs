using Cts.AppServices.ComplaintActions;
using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.Complaints;

public class GetComplaintActionsList
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
        var items = _repository.ComplaintActionItems.Where(e => e.ComplaintId == complaint.Id && !e.IsDeleted);

        var result =
            await _repository.GetComplaintActionsListAsync(ComplaintActionFilters.PublicIdPredicate(complaint.Id));

        result.Should().BeEquivalentTo(items);
    }

    [Test]
    public async Task WhenItemIsDeleted_ReturnsListWithoutItem()
    {
        var item = _repository.ComplaintActionItems.First(e => e.IsDeleted);

        var result =
            await _repository.GetComplaintActionsListAsync(ComplaintActionFilters.PublicIdPredicate(item.ComplaintId));

        result.Should().NotContain(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetComplaintActionsListAsync(e => e.Id == Guid.Empty);
        result.Should().BeEmpty();
    }
}
