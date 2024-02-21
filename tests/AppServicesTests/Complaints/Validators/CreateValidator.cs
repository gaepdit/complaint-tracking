using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Validators;
using FluentValidation.TestHelper;

namespace AppServicesTests.Complaints.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = new ComplaintCreateDto
        {
            ReceivedById = Guid.NewGuid().ToString(),
            ComplaintNature = Guid.NewGuid().ToString(),
            OfficeId = Guid.NewGuid(),
        };
        var validator = new ComplaintCreateValidator();

        var result = await validator.TestValidateAsync(model);

        using var scope = new AssertionScope();
        result.ShouldNotHaveValidationErrorFor(e => e.ReceivedById);
        result.ShouldNotHaveValidationErrorFor(e => e.ComplaintNature);
        result.ShouldNotHaveValidationErrorFor(e => e.OfficeId);
    }

    [Test]
    public async Task MissingCurrentOffice_ReturnsAsInvalid()
    {
        var model = new ComplaintCreateDto();
        var validator = new ComplaintCreateValidator();

        var result = await validator.TestValidateAsync(model);

        using var scope = new AssertionScope();
        result.ShouldHaveValidationErrorFor(e => e.ReceivedById)
            .WithErrorMessage("'Received By Id' must not be empty.");
        result.ShouldHaveValidationErrorFor(e => e.ComplaintNature)
            .WithErrorMessage("'Complaint Nature' must not be empty.");
        result.ShouldHaveValidationErrorFor(e => e.OfficeId)
            .WithErrorMessage("'Office Id' must not be empty.");
    }
}
