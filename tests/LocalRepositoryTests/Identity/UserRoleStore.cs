using Cts.Domain.Identity;
using Cts.LocalRepository.Identity;
using FluentAssertions.Execution;
using System.Diagnostics;

namespace LocalRepositoryTests.Identity;

public class UserRoleStore
{
    [Test]
    public async Task AddToRole_AddsRole()
    {
        using var store = new LocalUserStore();
        var user = store.UserStore.Last();
        var roleName = store.Roles.First().Name;
        Debug.Assert(roleName != null, nameof(roleName) + " != null");
        var resultBefore = await store.IsInRoleAsync(user, roleName, CancellationToken.None);

        await store.AddToRoleAsync(user, roleName, CancellationToken.None);
        var resultAfter = await store.IsInRoleAsync(user, roleName, CancellationToken.None);

        using (new AssertionScope())
        {
            resultBefore.Should().BeFalse();
            resultAfter.Should().BeTrue();
        }
    }

    [Test]
    public async Task RemoveFromRole_RemovesRole()
    {
        using var store = new LocalUserStore();
        var user = store.UserStore.First();
        var roleName = store.Roles.First().Name;
        Debug.Assert(roleName != null, nameof(roleName) + " != null");
        var resultBefore = await store.IsInRoleAsync(user, roleName, CancellationToken.None);

        await store.RemoveFromRoleAsync(user, roleName, CancellationToken.None);
        var resultAfter = await store.IsInRoleAsync(user, roleName, CancellationToken.None);

        using (new AssertionScope())
        {
            resultBefore.Should().BeTrue();
            resultAfter.Should().BeFalse();
        }
    }

    [Test]
    public async Task GetRoles_ReturnsListOfRoles()
    {
        using var store = new LocalUserStore();
        var user = store.UserStore.First();

        var result = await store.GetRolesAsync(user, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(store.Roles.Count);
        }
    }

    [Test]
    public async Task GetRoles_IfNone_ReturnsEmptyList()
    {
        using var store = new LocalUserStore();
        var user = store.UserStore.Last();

        var result = await store.GetRolesAsync(user, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
    }

    [Test]
    public async Task IsInRole_IfSo_ReturnsTrue()
    {
        using var store = new LocalUserStore();
        var user = store.UserStore.First();
        var roleName = store.Roles.First().Name;
        Debug.Assert(roleName != null, nameof(roleName) + " != null");
        var result = await store.IsInRoleAsync(user, roleName, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Test]
    public async Task IsInRole_IfNot_ReturnsFalse()
    {
        using var store = new LocalUserStore();
        var user = store.UserStore.Last();
        var roleName = store.Roles.First().Name;
        Debug.Assert(roleName != null, nameof(roleName) + " != null");
        var result = await store.IsInRoleAsync(user, roleName, CancellationToken.None);
        result.Should().BeFalse();
    }

    [Test]
    public async Task GetUsersInRole_IfSome_ReturnsListOfUsers()
    {
        using var store = new LocalUserStore();
        var result = await store.GetUsersInRoleAsync(RoleName.DivisionManager, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().HaveCount(1);
            result[0].Should().Be(store.UserStore.First());
        }
    }

    [Test]
    public async Task GetUsersInRole_IfNone_ReturnsEmptyList()
    {
        using var store = new LocalUserStore();
        var result = await store.GetUsersInRoleAsync("None", CancellationToken.None);
        result.Should().HaveCount(0);
    }
}
