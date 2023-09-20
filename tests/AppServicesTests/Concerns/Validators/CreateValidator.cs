using Cts.AppServices.Concerns;
using Cts.AppServices.Concerns.Validators;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;
using FluentValidation.TestHelper;

namespace AppServicesTests.Concerns.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var model = new ConcernCreateDto(TextData.ValidName);

        var validator = new ConcernCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Concern(Guid.Empty, TextData.ValidName));
        var model = new ConcernCreateDto(TextData.ValidName);

        var validator = new ConcernCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var model = new ConcernCreateDto(TextData.ShortName);

        var validator = new ConcernCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
