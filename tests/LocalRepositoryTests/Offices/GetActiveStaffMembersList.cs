using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.Offices;

public class GetActiveStaffMembersList
{
    private LocalOfficeRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        // Arrange
        var officeId = _repository.Items.First(e => e.Active).Id;
        var expected = _repository.Staff.Users
            .Where(e => e.Office != null && e.Office.Id == officeId);

        // Act
        var result = await _repository.GetStaffMembersListAsync(officeId, true);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        var item = _repository.Items.Last();
        var result = await _repository.GetStaffMembersListAsync(item.Id, false);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_ReturnsEmptyList()
    {
        var id = Guid.Empty;
        var result = await _repository.GetStaffMembersListAsync(id, false);
        result.Should().BeEmpty();
    }
}
