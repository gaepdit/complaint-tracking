using GaEpd.AppLibrary.Domain.Repositories;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Offices;

namespace IntegrationTests.BaseReadOnlyRepository;

public class Get
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
        var result = await _repository.GetAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var action = async () => await _repository.GetAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {id}");
    }
}
