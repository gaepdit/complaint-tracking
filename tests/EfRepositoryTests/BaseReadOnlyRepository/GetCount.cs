using Cts.Domain.Offices;
using Cts.TestData;

namespace EfRepositoryTests.BaseReadOnlyRepository;

public class GetCount
{
    private IOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsCount()
    {
        var item = OfficeData.GetOffices.First();
        var result = await _repository.CountAsync(e => e.Id == item.Id);
        result.Should().Be(1);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsZero()
    {
        var result = await _repository.CountAsync(e => e.Id == Guid.Empty);
        result.Should().Be(0);
    }
}
