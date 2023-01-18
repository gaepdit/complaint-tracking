using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        using var repository = new LocalOfficeRepository();
        var result = await repository.GetListAsync();
        result.Should().BeEquivalentTo(repository.Items);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        using var repository = new LocalOfficeRepository();
        repository.Items.Clear();
        var result = await repository.GetListAsync();
        result.Should().BeEmpty();
    }
}
