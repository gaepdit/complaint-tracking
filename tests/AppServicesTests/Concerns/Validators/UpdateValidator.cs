using Cts.AppServices.Concerns;
using Cts.AppServices.Concerns.Validators;
using Cts.Domain.Entities.Concerns;
using Cts.TestData.Constants;
using FluentValidation.TestHelper;

namespace AppServicesTests.Concerns.Validators;

public class UpdateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var model = new ConcernUpdateDto
        {
            Id = Guid.Empty,
            Name = TestConstants.ValidName,
            Active = true,
        };

        var validator = new ConcernUpdateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Concern(Guid.NewGuid(), TestConstants.ValidName));
        var model = new ConcernUpdateDto
        {
            Id = Guid.Empty,
            Name = TestConstants.ValidName,
            Active = true,
        };

        var validator = new ConcernUpdateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Concern(Guid.Empty, TestConstants.ValidName));
        var model = new ConcernUpdateDto
        {
            Id = Guid.Empty,
            Name = TestConstants.ValidName,
            Active = true,
        };

        var validator = new ConcernUpdateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IConcernRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Concern?)null);
        var model = new ConcernUpdateDto() { Name = TestConstants.ShortName };

        var validator = new ConcernUpdateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
