using GaEpd.AppLibrary.Pagination;
using MyAppRoot.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class GetPagedList
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var itemsCount = _repository.Items.Count;
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await _repository.GetPagedListAsync(paging);

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(_repository.Items);
        });
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        _repository.Items.Clear();
        var paging = new PaginatedRequest(1, 100);

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var paging = new PaginatedRequest(2, _repository.Items.Count);
        var result = await _repository.GetPagedListAsync(paging);
        result.Should().BeEmpty();
    }
}
