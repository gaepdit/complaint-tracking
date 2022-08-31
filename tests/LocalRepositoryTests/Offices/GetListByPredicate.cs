using Cts.LocalRepository;
using Cts.TestData.Constants;

namespace LocalRepositoryTests.Offices;

public class GetListByPredicate
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var item = _repository.Items.First();

        var result = await _repository.GetListAsync(e => e.Name == item.Name);

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(1);
            result[0].Should().BeEquivalentTo(item);
        });
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetListAsync(e => e.Name == TestConstants.NonExistentName);
        result.Should().BeEmpty();
    }
}
