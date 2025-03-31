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

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, context, token) => await NotDuplicateName(name, context, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, IValidationContext context,
        CancellationToken token = default)
    {
        var item = await _repository.FindByNameAsync(name, token: token).ConfigureAwait(false);
        return item is null || item.Id == (Guid)context.RootContextData["Id"];
    }
}
