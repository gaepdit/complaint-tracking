using Cts.AppServices.Offices;
using Cts.AppServices.Offices.Validators;
using Cts.Domain.Entities.Offices;
using Cts.TestData.Constants;
using FluentValidation;
using FluentValidation.TestHelper;

namespace AppServicesTests.Offices.Validators;

public class UpdateValidator
{
    private static ValidationContext<OfficeUpdateDto> GetContext(OfficeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Office?)null);
        var model = new OfficeUpdateDto(TextData.ValidName, true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.NewGuid(), TextData.ValidName));
        var model = new OfficeUpdateDto(TextData.ValidName, true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.Empty, TextData.ValidName));
        var model = new OfficeUpdateDto(TextData.ValidName, true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Office?)null);
        var model = new OfficeUpdateDto(TextData.ShortName, true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
