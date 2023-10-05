using Cts.AppServices.Concerns;
using Cts.AppServices.Concerns.Validators;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;
using FluentValidation;
using FluentValidation.TestHelper;

namespace AppServicesTests.Concerns.Validators;

public class UpdateValidator
{
    private static ValidationContext<ConcernUpdateDto> GetContext(ConcernUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var model = new ConcernUpdateDto(TextData.ValidName, true);

        var result = await new ConcernUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Concern(Guid.NewGuid(), TextData.ValidName));
        var model = new ConcernUpdateDto(TextData.ValidName, true);

        var result = await new ConcernUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Concern(Guid.Empty, TextData.ValidName));
        var model = new ConcernUpdateDto(TextData.ValidName, true);

        var result = await new ConcernUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var model = new ConcernUpdateDto(TextData.ShortName, true);

        var result = await new ConcernUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
