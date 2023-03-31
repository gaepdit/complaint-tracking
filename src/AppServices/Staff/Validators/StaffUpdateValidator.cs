using Cts.Domain.Identity;
using FluentValidation;

namespace Cts.AppServices.Staff.Validators;

public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(e => e.Phone).MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
