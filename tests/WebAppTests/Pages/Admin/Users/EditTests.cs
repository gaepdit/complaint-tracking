using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.TestData.Constants;
using Cts.WebApp.Models;
using Cts.WebApp.Pages.Admin.Users;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Users;

public class EditTests
{
    private static readonly OfficeViewDto OfficeViewTest = new(Guid.Empty, TextData.ValidName, true);

    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        FamilyName = TextData.ValidName,
        GivenName = TextData.ValidName,
        Email = TextData.ValidEmail,
        Office = OfficeViewTest,
        Active = true,
    };

    private static readonly StaffUpdateDto StaffUpdateTest = new()
    {
        Id = Guid.Empty.ToString(),
        Active = true,
    };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns(StaffViewTest);
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetActiveListItemsAsync(Arg.Any<CancellationToken>()).Returns(new List<ListItem>());
        var pageModel = new EditModel(staffServiceMock, officeServiceMock,
                Substitute.For<IValidator<StaffUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.UpdateStaff.Should().BeEquivalentTo(StaffViewTest.AsUpdateDto());
            pageModel.OfficeItems.Should().BeEmpty();
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var pageModel = new EditModel(Substitute.For<IStaffService>(),
                Substitute.For<IOfficeService>(), Substitute.For<IValidator<StaffUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);
        var pageModel = new EditModel(staffServiceMock,
                Substitute.For<IOfficeService>(), Substitute.For<IValidator<StaffUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateAsync(Arg.Any<StaffUpdateDto>()).Returns(IdentityResult.Success);
        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty.ToString());
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsBadRequest()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateAsync(Arg.Any<StaffUpdateDto>()).Returns(IdentityResult.Failed());
        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ReturnsPageWithInvalidModelState()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns(StaffViewTest);
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetActiveListItemsAsync(Arg.Any<CancellationToken>()).Returns(new List<ListItem>());
        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var page = new EditModel(staffServiceMock, officeServiceMock, validatorMock)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.DisplayStaff.Should().Be(StaffViewTest);
            page.UpdateStaff.Should().Be(StaffUpdateTest);
        }
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);
        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }
}
