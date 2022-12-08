using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Users;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(staffView);
            pageModel.Roles.Should().BeEmpty();
            pageModel.Message.Should().BeNull();
        }
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

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).Value.Should().Be("ID not found.");
        }
    }
}
