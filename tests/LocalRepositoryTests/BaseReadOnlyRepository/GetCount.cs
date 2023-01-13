using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class GetCount
{
    [Test]
    public async Task WhenItemsExist_ReturnsCount()
    {
        using var repository = new LocalOfficeRepository();
        var item = repository.Items.First();
        var result = await repository.CountAsync(e => e.Id == item.Id);
        result.Should().Be(1);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsZero()
    {
        using var repository = new LocalOfficeRepository();
        repository.Items.Clear();
        var result = await repository.CountAsync(e => e.Id == Guid.Empty);
        result.Should().Be(0);
    }
}
