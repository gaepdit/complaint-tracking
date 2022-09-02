using Cts.LocalRepository.Repositories;
using Cts.TestData.Constants;
using GaEpd.Library.Pagination;

namespace LocalRepositoryTests.Offices;

public class GetPagedListByPredicate
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

        var result = await _repository.GetPagedListAsync(e => e.Name.Length > 0, paging);

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(_repository.Items);
        });
    }

    [Test]
    public async Task WhenOneItemMatches_ReturnsListOfOne()
    {
        var item = _repository.Items.First();
        var paging = new PaginatedRequest(1, _repository.Items.Count);

        var result = await _repository.GetPagedListAsync(e => e.Name == item.Name, paging);

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(1);
            result[0].Should().BeEquivalentTo(item);
        });
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var paging = new PaginatedRequest(1, _repository.Items.Count);
        var result = await _repository.GetPagedListAsync(e => e.Name == TestConstants.NonExistentName, paging);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var paging = new PaginatedRequest(2, _repository.Items.Count);
        var result = await _repository.GetPagedListAsync(e => e.Name.Length > 0, paging);
        result.Should().BeEmpty();
    }
}
