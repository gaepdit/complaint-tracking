using Cts.Domain.Entities.Offices;
using Cts.TestData;
using Cts.TestData.Constants;

namespace EfRepositoryTests.Offices;

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
        result.Should().BeEquivalentTo(item, opts => opts
            .Excluding(e => e.StaffMembers)
            .Excluding(e => e.Assignor)
        );
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync(TextData.NonExistentName);
        result.Should().BeNull();
    }
}