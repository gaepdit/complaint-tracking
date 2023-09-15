using Cts.LocalRepository.Repositories;
using Cts.TestData.Constants;

namespace LocalRepositoryTests.Offices;

public class FindByName
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync(TextData.NonExistentName);
        result.Should().BeNull();
    }
}
