using Microsoft.AspNetCore.Identity;
using MyAppRoot.Domain.Identity;
using MyAppRoot.LocalRepository.Identity;
using MyAppRoot.TestData.Identity;

namespace LocalRepositoryTests.Identity;

public class UserRoleStore
{
    private IUserRoleStore<ApplicationUser> _store = default!;

    [SetUp]
    public void SetUp() => _store = new LocalUserStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task AddToRole_AddsRole()
    {
        var user = IdentityData.GetUsers.Last();
        var roleName = IdentityData.GetIdentityRoles.First().Name;
        var resultBefore = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        await _store.AddToRoleAsync(user, roleName, CancellationToken.None);
        var resultAfter = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        Assert.Multiple(() =>
        {
            resultBefore.Should().BeFalse();
            resultAfter.Should().BeTrue();
        });
    }

    [Test]
    public async Task RemoveFromRole_RemovesRole()
    {
        var user = IdentityData.GetUsers.First();
        var roleName = IdentityData.GetIdentityRoles.First().Name;
        var resultBefore = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        await _store.RemoveFromRoleAsync(user, roleName, CancellationToken.None);
        var resultAfter = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        Assert.Multiple(() =>
        {
            resultBefore.Should().BeTrue();
            resultAfter.Should().BeFalse();
        });
    }

    [Test]
    public async Task GetRoles_ReturnsListOfRoles()
    {
        var user = IdentityData.GetUsers.First();
        var result = await _store.GetRolesAsync(user, CancellationToken.None);

        Assert.Multiple(() =>
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(IdentityData.GetIdentityRoles.Count());
        });
    }

    [Test]
    public async Task GetRoles_IfNone_ReturnsEmptyList()
    {
        var user = IdentityData.GetUsers.Last();
        var result = await _store.GetRolesAsync(user, CancellationToken.None);

        Assert.Multiple(() =>
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        });
    }

    [Test]
    public async Task IsInRole_IfSo_ReturnsTrue()
    {
        var user = IdentityData.GetUsers.First();
        var roleName = IdentityData.GetIdentityRoles.First().Name;
        var result = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Test]
    public async Task IsInRole_IfNot_ReturnsFalse()
    {
        var user = IdentityData.GetUsers.Last();
        var roleName = IdentityData.GetIdentityRoles.First().Name;
        var result = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);
        result.Should().BeFalse();
    }

    [Test]
    public async Task GetUsersInRole_IfSome_ReturnsListOfUsers()
    {
        var roleName = IdentityData.GetIdentityRoles.First().Name;
        var result = await _store.GetUsersInRoleAsync(roleName, CancellationToken.None);

        Assert.Multiple(() =>
        {
            result.Should().HaveCount(1);
            result[0].Should().Be(IdentityData.GetUsers.First());
        });
    }

    [Test]
    public async Task GetUsersInRole_IfNone_ReturnsEmptyList()
    {
        var result = await _store.GetUsersInRoleAsync("None", CancellationToken.None);
        result.Should().HaveCount(0);
    }
}
