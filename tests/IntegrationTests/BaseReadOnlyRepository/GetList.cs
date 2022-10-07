using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Offices;

namespace IntegrationTests.BaseReadOnlyRepository;

public class GetList
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
        var result = await _repository.GetListAsync();
        result.Should().BeEquivalentTo(OfficeData.GetOffices);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        await _helper.ClearTableAsync<Office>();
        var result = await _repository.GetListAsync();
        result.Should().BeEmpty();
    }
}
