using Cts.Domain.Concerns;
using Cts.TestData;

namespace EfRepositoryTests.BaseReadOnlyRepository;

public class GetList
{
    private RepositoryHelper _helper = default!;
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = RepositoryHelper.CreateRepositoryHelper();
        _repository = _helper.GetConcernRepository();
    }

    [TearDown]
    public void TearDown()
    {
        _repository.Dispose();
        _helper.Dispose();
    }

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var result = await _repository.GetListAsync();
        result.Should().BeEquivalentTo(ConcernData.GetConcerns);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        await _helper.ClearTableAsync<Concern>();
        var result = await _repository.GetListAsync();
        result.Should().BeEmpty();
    }
}
