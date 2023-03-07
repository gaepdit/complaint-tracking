using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadRepository;

public class Exists
{
    private LocalConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsTrue()
    {
        var item = _repository.Items.First();
        var result = await _repository.ExistsAsync(item.Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.ExistsAsync(Guid.Empty);
        result.Should().BeFalse();
    }
}
