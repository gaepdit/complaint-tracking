using Cts.Domain;
using Cts.Domain.Entities.Concerns;
using FluentValidation;

namespace Cts.AppServices.Concerns.Validators;

public class ConcernUpdateValidator : AbstractValidator<ConcernUpdateDto>
{
    private readonly IConcernRepository _repository;

    public ConcernUpdateValidator(IConcernRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (e, _, token) => await NotDuplicateName(e, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(ConcernUpdateDto item, CancellationToken token = default)
    {
        var existing = await _repository.FindByNameAsync(item.Name, token);
        return existing is null || existing.Id == item.Id;
    }
}
