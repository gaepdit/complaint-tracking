using Cts.AppServices.Complaints.Dto.Command;
using FluentValidation;

namespace Cts.AppServices.Complaints.Validators;

public class ComplaintUpdateValidator : AbstractValidator<ComplaintUpdateDto>
{
    public ComplaintUpdateValidator()
    {
        RuleFor(dto => dto.ReceivedById).NotEmpty();
        RuleFor(dto => dto.ComplaintNature).NotEmpty();
    }
}
