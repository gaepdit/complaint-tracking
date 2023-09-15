using Cts.Domain.Entities.Concerns;
using Cts.LocalRepository.Repositories;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace LocalRepositoryTests.BaseWriteRepository;

public class Insert
{
    private LocalConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemIsValid_InsertsItem()
    {
        var initialCount = _repository.Items.Count;
        var newItem = new Concern(Guid.NewGuid(), TextData.ValidName);

        await _repository.InsertAsync(newItem);

        var getResult = await _repository.GetAsync(newItem.Id);
        using (new AssertionScope())
        {
            getResult.Should().BeEquivalentTo(newItem);
            _repository.Items.Count.Should().Be(initialCount + 1);
        }
    }
}
