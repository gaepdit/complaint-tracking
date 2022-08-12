﻿using Cts.Domain.Entities;
using Cts.Domain.Offices;
using FluentValidation;

namespace Cts.AppServices.Offices;

public class OfficeCreateValidator : AbstractValidator<OfficeCreateDto>
{
    private readonly IOfficeRepository _repository;

    public OfficeCreateValidator(IOfficeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Length(Office.MinNameLength, Office.MaxNameLength)
            .MustAsync(async (e, name, token) => await NotDuplicateName(name, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token) is null;
}
