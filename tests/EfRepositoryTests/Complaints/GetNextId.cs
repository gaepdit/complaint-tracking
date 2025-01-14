using Cts.Domain.Entities.Complaints;

namespace EfRepositoryTests.Complaints;

public class GetNextId
{
    private IComplaintRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenEF_ReturnsNull()
    {
        _repository.GetNextId().Should().BeNull();
    }
}
