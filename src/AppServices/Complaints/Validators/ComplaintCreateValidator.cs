using Cts.AppServices.Complaints.Dto;
using Cts.Domain.BaseEntities;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using FluentValidation;

namespace Cts.AppServices.Concerns;

public class ComplaintCreateValidator : AbstractValidator<ComplaintCreateDto>
{
    private readonly IComplaintRepository _repository;

    public ComplaintCreateValidator(IComplaintRepository repository)
    {
        _repository = repository;

        //RuleFor(e => e.Name)
        //    .Length(SimpleNamedEntity.MinNameLength, SimpleNamedEntity.MaxNameLength)
        //    .MustAsync(async (_, name, token) => await NotDuplicateName(name, token))
        //    .WithMessage("The name entered already exists.");
    }
}
