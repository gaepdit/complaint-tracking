using Cts.Domain.Entities;
using Cts.LocalRepository.Repositories;
using GaEpd.Library.Domain.Repositories;

namespace LocalRepositoryTests.Offices;

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
        var itemId = _repository.Items.First().Id;

        await _repository.DeleteAsync(itemId);
        var result = await _repository.FindAsync(itemId);

        Assert.Multiple(() =>
        {
            _repository.Items.Count.Should().Be(initialCount - 1);
            result.Should().BeNull();
        });
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var action = async () => await _repository.DeleteAsync(Guid.Empty);

        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {Guid.Empty}");
    }
}
