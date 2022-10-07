using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;
using MyAppRoot.TestData.Offices;

namespace IntegrationTests.BaseReadOnlyRepository;

public class GetListByPredicate
{
    private IOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await _repository.GetListAsync(e => e.Name == item.Name);
        Assert.Multiple(() =>
        {
            result.Count.Should().Be(1);
            result.First().Should().BeEquivalentTo(item);
        });
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetListAsync(e => e.Name == TestConstants.NonExistentName);
        result.Should().BeEmpty();
    }
}
