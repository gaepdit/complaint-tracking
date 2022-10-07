using Microsoft.AspNetCore.Identity;
using MyAppRoot.Domain.Identity;
using MyAppRoot.Domain.Offices;
using MyAppRoot.LocalRepository.Identity;
using MyAppRoot.TestData.Identity;

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
        var user = IdentityData.GetUsers.First();
        var result = await _store.GetUserIdAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.Id);
    }

    [Test]
    public async Task GetUserName_ReturnsUserName()
    {
        var user = IdentityData.GetUsers.First();
        var result = await _store.GetUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.UserName);
    }

    [Test]
    public async Task GetNormalizedUserName_ReturnsNormalizedUserName()
    {
        var user = IdentityData.GetUsers.First();
        var result = await _store.GetNormalizedUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.NormalizedUserName);
    }

    [Test]
    public async Task Update_WhenItemIsValid_UpdatesItem()
    {
        var user = IdentityData.GetUsers.First();
        user.Phone = "1";
        user.Office = new Office(Guid.NewGuid(), "abc");

        await _store.UpdateAsync(user, CancellationToken.None);

        var result = await _store.FindByIdAsync(user.Id, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Update_WhenItemDoesNotExist_Throws()
    {
        var user = new ApplicationUser { Id = Guid.Empty.ToString() };
        var action = async () => await _store.UpdateAsync(user, CancellationToken.None);
        (await action.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("Sequence contains no matching element");
    }

    [Test]
    public async Task FindById_ReturnsUser()
    {
        var user = IdentityData.GetUsers.First();
        var result = await _store.FindByIdAsync(user.Id, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task FindByName_ReturnsUser()
    {
        var user = IdentityData.GetUsers.First();
        var result = await _store.FindByNameAsync(user.NormalizedUserName, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }
}
