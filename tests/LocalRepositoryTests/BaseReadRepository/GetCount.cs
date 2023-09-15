using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadRepository;

public class GetCount
{
    private LocalConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsCount()
    {
        var item = _repository.Items.First();
        var result = await _repository.CountAsync(e => e.Id == item.Id);
        result.Should().Be(1);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsZero()
    {
        _repository.Items.Clear();
        var result = await _repository.CountAsync(e => e.Id == Guid.Empty);
        result.Should().Be(0);
    }
}
