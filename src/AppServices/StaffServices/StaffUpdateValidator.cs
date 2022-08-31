using Cts.AppServices.StaffServices;
using Cts.Domain.Entities;
using FluentValidation;

namespace Cts.AppServices.ActionTypes;

public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(e => e.Phone)
            .MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
