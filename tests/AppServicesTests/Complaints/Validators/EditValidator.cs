using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Validators;
using FluentValidation.TestHelper;

namespace AppServicesTests.Complaints.Validators;

public class EditValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = new ComplaintUpdateDto
        {
            ReceivedById = Guid.NewGuid().ToString(),
            ComplaintNature = Guid.NewGuid().ToString(),
        };
        var validator = new ComplaintUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        using var scope = new AssertionScope();
        result.ShouldNotHaveValidationErrorFor(e => e.ReceivedById);
        result.ShouldNotHaveValidationErrorFor(e => e.ComplaintNature);
    }

    [Test]
    public async Task MissingCurrentOffice_ReturnsAsInvalid()
    {
        var model = new ComplaintUpdateDto();
        var validator = new ComplaintUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        using var scope = new AssertionScope();
        result.ShouldHaveValidationErrorFor(e => e.ReceivedById)
            .WithErrorMessage("'Received By Id' must not be empty.");
        result.ShouldHaveValidationErrorFor(e => e.ComplaintNature)
            .WithErrorMessage("'Complaint Nature' must not be empty.");
    }
}
