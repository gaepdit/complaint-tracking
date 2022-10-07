using Microsoft.AspNetCore.Identity;
using MyAppRoot.LocalRepository.Identity;
using MyAppRoot.TestData.Identity;

namespace LocalRepositoryTests.Identity;

public class RoleStore
{
    private IRoleStore<IdentityRole> _store = default!;

    [SetUp]
    public void SetUp() => _store = new LocalRoleStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task GetRoleId_ReturnsId()
    {
        var role = IdentityData.GetIdentityRoles.First();
        var result = await _store.GetRoleIdAsync(role, CancellationToken.None);
        result.Should().BeEquivalentTo(role.Id);
    }

    [Test]
    public async Task GetRoleName_ReturnsName()
    {
        var role = IdentityData.GetIdentityRoles.First();
        var result = await _store.GetRoleNameAsync(role, CancellationToken.None);
        result.Should().BeEquivalentTo(role.Name);
    }

    [Test]
    public async Task GetNormalizedRoleName_ReturnsNormalizedName()
    {
        var role = IdentityData.GetIdentityRoles.First();
        var result = await _store.GetNormalizedRoleNameAsync(role, CancellationToken.None);
        result.Should().BeEquivalentTo(role.NormalizedName);
    }

    [Test]
    public async Task FindById_ReturnsRole()
    {
        var role = IdentityData.GetIdentityRoles.First();
        var result = await _store.FindByIdAsync(role.Id, CancellationToken.None);
        result.Should().BeEquivalentTo(role);
    }

    [Test]
    public async Task FindByName_ReturnsRole()
    {
        var role = IdentityData.GetIdentityRoles.First();
        var result = await _store.FindByNameAsync(role.NormalizedName, CancellationToken.None);
        result.Should().BeEquivalentTo(role);
    }
}
