using Cts.LocalRepository.Repositories;
using FluentAssertions.Execution;

namespace LocalRepositoryTests.BaseReadRepository;

public class FindByPredicate
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.FindAsync(e => e.Id == item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindAsync(e => e.Id == Guid.Empty);
        result.Should().BeNull();
    }

    [Test]
    public async Task LocalRepositoryIsCaseSensitive()
    {
        var item = _repository.Items.First();

        var resultIgnoreCase = await _repository.FindAsync(e =>
            e.Name.ToUpperInvariant().Equals(item.Name.ToLowerInvariant(), StringComparison.CurrentCultureIgnoreCase));
        var resultCaseSensitive = await _repository.FindAsync(e =>
            e.Name.ToUpperInvariant().Equals(item.Name.ToLowerInvariant(), StringComparison.CurrentCulture));

        using (new AssertionScope())
        {
            resultIgnoreCase.Should().BeEquivalentTo(item);
            resultCaseSensitive.Should().BeNull();
        }
    }
}
