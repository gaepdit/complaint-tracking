using GaEpd.AppLibrary.Pagination;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Offices;

namespace IntegrationTests.BaseReadOnlyRepository;

public class GetPagedList
{
    private RepositoryHelper _helper = default!;
    private IOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = RepositoryHelper.CreateRepositoryHelper();
        _repository = _helper.GetOfficeRepository();
    }

    [TearDown]
    public void TearDown()
    {
        _repository.Dispose();
        _helper.Dispose();
    }

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var itemsCount = OfficeData.GetOffices.Count();
        var paging = new PaginatedRequest(1, itemsCount);

        var result = await _repository.GetPagedListAsync(paging);

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(itemsCount);
            result.Should().BeEquivalentTo(OfficeData.GetOffices);
        });
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await _helper.ClearTableAsync<Office>();
        var paging = new PaginatedRequest(1, 100);

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var itemsCount = OfficeData.GetOffices.Count();
        var paging = new PaginatedRequest(2, itemsCount);
        var result = await _repository.GetPagedListAsync(paging);
        result.Should().BeEmpty();
    }
}
