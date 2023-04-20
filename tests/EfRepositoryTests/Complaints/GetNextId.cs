using Cts.Domain.Entities.Complaints;

namespace EfRepositoryTests.Complaints;

public class GetNextId
{
    private IComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenEF_ReturnsNull()
    {
        (await _repository.GetNextIdAsync()).Should().BeNull();
    }
}
