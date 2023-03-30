using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Validators;
using FluentValidation.TestHelper;

namespace AppServicesTests.Complaints.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = new ComplaintCreateDto { CurrentOfficeId = Guid.Empty };
        var validator = new ComplaintCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.CurrentOfficeId);
    }

    [Test]
    public async Task MissingCurrentOffice_ReturnsAsInvalid()
    {
        var model = new ComplaintCreateDto();
        var validator = new ComplaintCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.CurrentOfficeId)
            .WithErrorMessage("'Current Office Id' must not be empty.");
    }
}
