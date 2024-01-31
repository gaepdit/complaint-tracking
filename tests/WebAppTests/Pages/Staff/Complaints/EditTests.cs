﻿using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Staff;
using Cts.WebApp.Pages.Staff.Complaints;

namespace WebAppTests.Pages.Staff.Complaints;

[TestFixture]
public class EditTests
{
    private IComplaintService _complaintService = null!;
    private IStaffService _staffService = null!;
    private IConcernService _concernService = null!;

    [SetUp]
    public void Setup()
    {
        _complaintService = Substitute.For<IComplaintService>();
        _staffService = Substitute.For<IStaffService>();
        _concernService = Substitute.For<IConcernService>();
        _concernService.GetAsListItemsAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<ListItem>());
        _staffService.GetAsListItemsAsync(Arg.Any<bool>()).Returns(new List<ListItem<string>>());
    }

    [TearDown]
    public void Teardown()
    {
        _complaintService.Dispose();
        _staffService.Dispose();
        _concernService.Dispose();
    }

    [Test]
    public async Task OnGet_ReturnsPage()
    {
        // Arrange
        const int complaintId = 0;
        var dto = new ComplaintUpdateDto();
        _complaintService.FindForUpdateAsync(complaintId).Returns(dto);
        var validator = Substitute.For<IValidator<ComplaintUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<ComplaintUpdateDto>(),
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Success());
        var page = new EditModel(_complaintService, _staffService, _concernService, validator, authorization);

        // Act
        var result = await page.OnGetAsync(complaintId);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.Item.Should().BeOfType<ComplaintUpdateDto>();
        page.Item.Should().Be(dto);
    }

    [Test]
    public async Task OnPost_ReturnsRedirectResultWhenModelIsValid()
    {
        // Arrange
        var validator = Substitute.For<IValidator<ComplaintUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_complaintService, _staffService, _concernService, validator, authorization)
        {
            Id = 1,
            Item = new ComplaintUpdateDto(),
            TempData = WebAppTestsSetup.PageTempData(),
        };
        _complaintService.FindForUpdateAsync(page.Id).Returns(page.Item);
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(),
                Arg.Any<ComplaintUpdateDto>(),
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Success());
        validator.ValidateAsync(Arg.Any<ComplaintUpdateDto>()).Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnPost_ReturnsBadRequestWhenOriginalComplaintIsNull()
    {
        // Arrange
        var validator = Substitute.For<IValidator<ComplaintUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_complaintService, _staffService, _concernService, validator, authorization)
        {
            Id = 2,
            TempData = WebAppTestsSetup.PageTempData(),
        };

        _complaintService.FindForUpdateAsync(page.Id).Returns((ComplaintUpdateDto?)null);

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_ReturnsBadRequestWhenUserCannotEdit()
    {
        // Arrange
        var validator = Substitute.For<IValidator<ComplaintUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_complaintService, _staffService, _concernService, validator, authorization)
            { Id = 3 };
        _complaintService.FindForUpdateAsync(page.Id).Returns(new ComplaintUpdateDto());
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<ComplaintUpdateDto>(),
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Failed());
        validator.ValidateAsync(Arg.Any<ComplaintUpdateDto>()).Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_ReturnsPageResultWhenModelStateIsNotValid()
    {
        // Arrange
        var validator = Substitute.For<IValidator<ComplaintUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_complaintService, _staffService, _concernService, validator, authorization)
            { Id = 4 };
        page.ModelState.AddModelError("test", "test error");
        _complaintService.FindForUpdateAsync(page.Id).Returns(new ComplaintUpdateDto());
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<ComplaintUpdateDto>(),
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Success());
        validator.ValidateAsync(Arg.Any<ComplaintUpdateDto>()).Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
