using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.Domain.Identity;
using MyAppRoot.TestData.Constants;
using MyAppRoot.WebApp.Pages.Admin.Users;

namespace WebAppTests.Pages.Admin.Users;

public class DetailsTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffView = new StaffViewDto
        {
            Id = Guid.Empty,
            Email = TestConstants.ValidEmail,
            FirstName = TestConstants.ValidName,
            LastName = TestConstants.ValidName,
        };
        var service = new Mock<IStaffAppService>();
        service.Setup(l => l.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(staffView);
        service.Setup(l => l.GetAppRolesAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<AppRole>());
        var pageModel = new DetailsModel { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(service.Object, staffView.Id);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(staffView);
            pageModel.Roles.Should().BeEmpty();
            pageModel.Message.Should().BeNull();
        });
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var service = new Mock<IStaffAppService>();
        var pageModel = new DetailsModel { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(service.Object, null);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var service = new Mock<IStaffAppService>();
        service.Setup(l => l.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((StaffViewDto?)null);
        var pageModel = new DetailsModel { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(service.Object, Guid.Empty);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).Value.Should().Be("ID not found.");
        });
    }
}
