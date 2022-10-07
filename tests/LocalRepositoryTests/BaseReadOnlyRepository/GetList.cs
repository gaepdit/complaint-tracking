using MyAppRoot.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class GetList
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var result = await _repository.GetListAsync();
        result.Should().BeEquivalentTo(_repository.Items);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        _repository.Items.Clear();
        var result = await _repository.GetListAsync();
        result.Should().BeEmpty();
    }
}
