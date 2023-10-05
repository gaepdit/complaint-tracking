using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using FluentValidation;

namespace Cts.AppServices.Staff.Validators;

[UsedImplicitly]
public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(dto => dto.Phone)
            .MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
