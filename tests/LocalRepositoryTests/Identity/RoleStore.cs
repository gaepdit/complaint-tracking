using Cts.LocalRepository.Identity;
using System.Diagnostics;

namespace LocalRepositoryTests.Identity;

public class RoleStore
{
    private LocalRoleStore _store;

    [SetUp]
    public void SetUp() => _store = new LocalRoleStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task GetRoleId_ReturnsId()
    {
        var role = _store.Roles.First();
        var result = await _store.GetRoleIdAsync(role, CancellationToken.None);
        result.Should().BeEquivalentTo(role.Id);
    }

    [Test]
    public async Task GetRoleName_ReturnsName()
    {
        var role = _store.Roles.First();
        var result = await _store.GetRoleNameAsync(role, CancellationToken.None);
        result.Should().BeEquivalentTo(role.Name);
    }

    [Test]
    public async Task GetNormalizedRoleName_ReturnsNormalizedName()
    {
        var role = _store.Roles.First();
        var result = await _store.GetNormalizedRoleNameAsync(role, CancellationToken.None);
        result.Should().BeEquivalentTo(role.NormalizedName);
    }

    [Test]
    public async Task FindById_ReturnsRole()
    {
        var role = _store.Roles.First();
        var result = await _store.FindByIdAsync(role.Id, CancellationToken.None);
        result.Should().BeEquivalentTo(role);
    }

    [Test]
    public async Task FindByName_ReturnsRole()
    {
        var role = _store.Roles.First();
        Debug.Assert(role.NormalizedName != null);
        var result = await _store.FindByNameAsync(role.NormalizedName, CancellationToken.None);
        result.Should().BeEquivalentTo(role);
    }
}
