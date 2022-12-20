using Cts.Domain.Offices;
using Cts.LocalRepository.Repositories;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace LocalRepositoryTests.BaseRepository;

public class Insert
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemIsValid_InsertsItem()
    {
        var initialCount = _repository.Items.Count;
        var newItem = new Office(Guid.NewGuid(), TestConstants.ValidName);

        await _repository.InsertAsync(newItem);

        var getResult = await _repository.GetAsync(newItem.Id);
        using (new AssertionScope())
        {
            getResult.Should().BeEquivalentTo(newItem);
            _repository.Items.Count.Should().Be(initialCount + 1);
        }
    }
}