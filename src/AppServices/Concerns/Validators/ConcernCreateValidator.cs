using Cts.Domain;
using Cts.Domain.Entities.Concerns;
using FluentValidation;

namespace Cts.AppServices.Concerns.Validators;

public class ConcernCreateValidator : AbstractValidator<ConcernCreateDto>
{
    private readonly IConcernRepository _repository;

    public ConcernCreateValidator(IConcernRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token: token).ConfigureAwait(false) is null;
}
