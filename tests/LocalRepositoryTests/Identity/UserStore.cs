using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.LocalRepository.Identity;
using Cts.TestData.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using System.Diagnostics;

namespace LocalRepositoryTests.Identity;

public class UserStore
{
    private LocalUserStore _store;

    [SetUp]
    public void SetUp() => _store = RepositoryHelper.GetLocalUserStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task GetUserId_ReturnsId()
    {
        var user = UserData.GetUsers.First();
        var result = await _store.GetUserIdAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.Id);
    }

    [Test]
    public async Task GetUserName_ReturnsUserName()
    {
        var user = _store.UserStore.First();
        var result = await _store.GetUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.UserName);
    }

    [Test]
    public async Task GetNormalizedUserName_ReturnsNormalizedUserName()
    {
        var user = _store.UserStore.First();
        var result = await _store.GetNormalizedUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.NormalizedUserName);
    }

    [Test]
    public async Task Update_WhenItemIsValid_UpdatesItem()
    {
        var store = RepositoryHelper.GetLocalUserStore();
        var user = store.UserStore.First();
        user.PhoneNumber = "1";
        user.Office = new Office(Guid.NewGuid(), "abc");

        var result = await store.UpdateAsync(user, CancellationToken.None);
        var updatedUser = await store.FindByIdAsync(user.Id, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Succeeded.Should().BeTrue();
        updatedUser.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Update_WhenItemDoesNotExist_Throws()
    {
        var user = new ApplicationUser { Id = Guid.Empty.ToString() };
        var action = async () => await _store.UpdateAsync(user, CancellationToken.None);
        await action.Should().ThrowAsync<EntityNotFoundException<ApplicationUser>>();
    }

    [Test]
    public async Task FindById_ReturnsUser()
    {
        var user = _store.UserStore.First();
        var result = await _store.FindByIdAsync(user.Id, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task FindByName_ReturnsUser()
    {
        var user = _store.UserStore.First();
        Debug.Assert(user.NormalizedUserName != null);
        var result = await _store.FindByNameAsync(user.NormalizedUserName, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }
}
