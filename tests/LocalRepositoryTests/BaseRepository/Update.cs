using GaEpd.AppLibrary.Domain.Repositories;
using MyAppRoot.Domain.Offices;
using MyAppRoot.LocalRepository.Repositories;
using MyAppRoot.TestData.Constants;

namespace LocalRepositoryTests.BaseRepository;

public class Update
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemIsValid_UpdatesItem()
    {
        var item = _repository.Items.First();
        item.ChangeName(TestConstants.ValidName);
        item.Active = !item.Active;

        await _repository.UpdateAsync(item);

        var getResult = await _repository.GetAsync(item.Id);
        getResult.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var action = async () => await _repository.UpdateAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
