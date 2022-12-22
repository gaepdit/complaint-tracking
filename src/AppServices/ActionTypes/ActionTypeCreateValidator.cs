using Cts.Domain.ActionTypes;
using Cts.Domain.BaseEntities;
using FluentValidation;

namespace Cts.AppServices.ActionTypes;

public class ActionTypeCreateValidator : AbstractValidator<ActionTypeCreateDto>
{
    private readonly IActionTypeRepository _repository;

    public ActionTypeCreateValidator(IActionTypeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Length(SimpleNamedEntity.MinNameLength, SimpleNamedEntity.MaxNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token) is null;
}
