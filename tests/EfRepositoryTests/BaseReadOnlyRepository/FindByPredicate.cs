using Cts.Domain.Concerns;
using Cts.TestData;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace EfRepositoryTests.BaseReadOnlyRepository;

public class FindByPredicate
{
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = ConcernData.GetConcerns.First(e => e.Active);
        var result = await _repository.FindAsync(e => e.Name == item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindAsync(e => e.Name == TestConstants.NonExistentName);
        result.Should().BeNull();
    }

    [Test]
    public async Task SqliteDatabaseIsCaseSensitive()
    {
        var item = ConcernData.GetConcerns.First(e => e.Active);

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await _repository.FindAsync(e =>
            e.Name.ToUpper().Equals(item.Name.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await _repository.FindAsync(e =>
            e.Name.ToUpper().Equals(item.Name.ToLower()));

        using (new AssertionScope())
        {
            resultSameCase.Should().BeEquivalentTo(item);
            resultDifferentCase.Should().BeNull();
        }
    }
}
