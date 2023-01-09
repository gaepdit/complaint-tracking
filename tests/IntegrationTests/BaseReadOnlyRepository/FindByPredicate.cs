using Cts.TestData;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace IntegrationTests.BaseReadOnlyRepository;

public class FindByPredicate
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await repository.FindAsync(e => e.Name == item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var result = await repository.FindAsync(e => e.Name == TestConstants.NonExistentName);
        result.Should().BeNull();
    }

    [Test]
    public async Task SqliteDatabaseIsCaseSensitive()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();
        var item = OfficeData.GetOffices.First(e => e.Active);

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await repository.FindAsync(e =>
            e.Name.ToUpper().Equals(item.Name.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await repository.FindAsync(e =>
            e.Name.ToUpper().Equals(item.Name.ToLower()));

        using (new AssertionScope())
        {
            resultSameCase.Should().BeEquivalentTo(item);
            resultDifferentCase.Should().BeNull();
        }
    }
}
