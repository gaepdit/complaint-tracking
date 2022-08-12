using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using FluentValidation;

namespace Cts.AppServices.ActionTypes;

public class ActionTypeUpdateValidator : AbstractValidator<ActionTypeUpdateDto>
{
    private readonly IActionTypeRepository _repository;

    public ActionTypeUpdateValidator(IActionTypeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Length(ActionType.MinNameLength, ActionType.MaxNameLength)
            .MustAsync(async (e, _, token) => await NotDuplicateName(e, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(ActionTypeUpdateDto item, CancellationToken token = default)
    {
        var existing = await _repository.FindByNameAsync(item.Name, token);
        return existing is null || existing.Id == item.Id;
    }
}
