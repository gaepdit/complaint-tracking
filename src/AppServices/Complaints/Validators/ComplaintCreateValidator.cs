using Cts.AppServices.Complaints.CommandDto;
using FluentValidation;

namespace Cts.AppServices.Complaints.Validators;

public class ComplaintCreateValidator : AbstractValidator<ComplaintCreateDto>
{
    public ComplaintCreateValidator()
    {
        RuleFor(dto => dto.ReceivedById).NotEmpty();
        RuleFor(dto => dto.ComplaintNature).NotEmpty();
        RuleFor(dto => dto.OfficeId).NotEmpty();
    }
}
