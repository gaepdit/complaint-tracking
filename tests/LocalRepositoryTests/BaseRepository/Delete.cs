using GaEpd.AppLibrary.Domain.Repositories;
using MyAppRoot.Domain.Offices;
using MyAppRoot.LocalRepository.Repositories;
using MyAppRoot.TestData.Constants;

namespace LocalRepositoryTests.BaseRepository;

public class Delete
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_DeletesItem()
    {
        var initialCount = _repository.Items.Count;
        var item = _repository.Items.First();

        await _repository.DeleteAsync(item);
        var result = await _repository.FindAsync(item.Id);

        Assert.Multiple(() =>
        {
            _repository.Items.Count.Should().Be(initialCount - 1);
            result.Should().BeNull();
        });
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new Office(Guid.Empty, TestConstants.ValidName);
        var action = async () => await _repository.DeleteAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {item.Id}");
    }
}
