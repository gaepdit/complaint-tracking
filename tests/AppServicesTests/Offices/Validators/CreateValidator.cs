using Cts.AppServices.Offices;
using Cts.AppServices.Offices.Validators;
using Cts.Domain.Entities.Offices;
using Cts.TestData.Constants;
using FluentValidation.TestHelper;

namespace AppServicesTests.Offices.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Office?)null);
        var model = new OfficeCreateDto(TextData.ValidName);

        var validator = new OfficeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.Empty, TextData.ValidName));
        var model = new OfficeCreateDto(TextData.ValidName);

        var validator = new OfficeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Office?)null);
        var model = new OfficeCreateDto(TextData.ShortName);

        var validator = new OfficeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
