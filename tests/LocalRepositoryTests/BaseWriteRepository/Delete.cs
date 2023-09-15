using Cts.Domain.Entities.Concerns;
using Cts.LocalRepository.Repositories;
using Cts.TestData.Constants;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Domain.Repositories;

namespace LocalRepositoryTests.BaseWriteRepository;

public class Delete
{
    private LocalConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_DeletesItem()
    {
        var initialCount = _repository.Items.Count;
        var item = _repository.Items.First();

        await _repository.DeleteAsync(item);
        var result = await _repository.FindAsync(item.Id);

        using (new AssertionScope())
        {
            _repository.Items.Count.Should().Be(initialCount - 1);
            result.Should().BeNull();
        }
    }

    [Test]
    public async Task WhenItemDoesNotExist_Throws()
    {
        var item = new Concern(Guid.Empty, TextData.ValidName);
        var action = async () => await _repository.DeleteAsync(item);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Concern).FullName}, id: {item.Id}");
    }
}
