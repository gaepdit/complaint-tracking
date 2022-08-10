using Cts.Domain.Users;
using Cts.LocalRepository.Identity;
using Cts.TestData.Identity;
using Microsoft.AspNetCore.Identity;

namespace LocalRepositoryTests.Identity;

public class UserStore
{
    private IUserRoleStore<ApplicationUser> _store = default!;

    [SetUp]
    public void SetUp() => _store = new LocalUserStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task GetUserId_ReturnsId()
    {
        var user = Data.GetUsers.First();
        var result = await _store.GetUserIdAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.Id);
    }

    [Test]
    public async Task GetUserName_ReturnsUserName()
    {
        var user = Data.GetUsers.First();
        var result = await _store.GetUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.UserName);
    }

    [Test]
    public async Task GetNormalizedUserName_ReturnsNormalizedUserName()
    {
        var user = Data.GetUsers.First();
        var result = await _store.GetNormalizedUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.NormalizedUserName);
    }

    [Test]
    public async Task FindById_ReturnsUser()
    {
        var user = Data.GetUsers.First();
        var result = await _store.FindByIdAsync(user.Id, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task FindByName_ReturnsUser()
    {
        var user = Data.GetUsers.First();
        var result = await _store.FindByNameAsync(user.NormalizedUserName, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }
}
