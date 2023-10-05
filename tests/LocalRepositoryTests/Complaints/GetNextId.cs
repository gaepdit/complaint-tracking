using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.Complaints;

public class GetNextId
{
    private LocalComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenLocalRepository_ReturnsNextIdNumber()
    {
        var maxId = _repository.Items.Max(e => e.Id);
        _repository.GetNextId().Should().Be(maxId + 1);
    }
}
