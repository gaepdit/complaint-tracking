using FluentValidation;
using MyAppRoot.Domain.Offices;

namespace MyAppRoot.AppServices.Offices;

public class OfficeCreateValidator : AbstractValidator<OfficeCreateDto>
{
    private readonly IOfficeRepository _repository;

    public OfficeCreateValidator(IOfficeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Length(Office.MinNameLength, Office.MaxNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token) is null;
}
