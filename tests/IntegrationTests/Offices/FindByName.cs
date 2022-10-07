using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;
using MyAppRoot.TestData.Offices;

namespace IntegrationTests.Offices;

public class FindByName
{
    private IOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await _repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync(TestConstants.NonExistentName);
        result.Should().BeNull();
    }
}
