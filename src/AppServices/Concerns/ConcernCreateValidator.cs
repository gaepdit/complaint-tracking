using Cts.Domain.Concerns;
using FluentValidation;

namespace Cts.AppServices.Concerns;

public class ConcernCreateValidator : AbstractValidator<ConcernCreateDto>
{
    private readonly IConcernRepository _repository;

    public ConcernCreateValidator(IConcernRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Length(Concern.MinNameLength, Concern.MaxNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token) is null;
}
