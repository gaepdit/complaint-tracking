using Cts.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cts.Domain.Repositories;

public interface IActionTypeRepository : IDisposable
{
    Task<ActionTypeView> GetActionTypeAsync(Guid id);
    Task<IReadOnlyList<ActionTypeView>> ListActionTypesAsync();
    Task<Guid> CreateActionTypeAsync(ActionTypeCreate resource);
    Task UpdateActionTypeAsync(ActionTypeEdit resource);
    Task<bool> ActionTypeNameExistsAsync(string name, Guid? ignoreId = null);
}

public class ActionTypeView
{
    public ActionTypeView(ActionType item)
    {
        Id = item.Id;
        Name = item.Name;
        Active = item.Active;
    }

    public Guid Id { get; }
    public string Name { get; }
    public bool Active { get; }
}

public class ActionTypeCreate
{
    public ActionTypeCreate() { }

    public ActionTypeCreate(ActionTypeView item)
    {
        Name = item.Name;
    }

    public string Name { get; set; } = string.Empty;
    public void TrimAll() => Name = Name.Trim();
}

public class ActionTypeEdit : ActionTypeCreate
{
    public ActionTypeEdit() { }

    public ActionTypeEdit(ActionTypeView item) : base(item)
    {
        Id = item.Id;
        Active = item.Active;
    }

    [HiddenInput]
    public Guid Id { get; }

    public bool Active { get; } = true;
}

public class ActionTypeValidator : AbstractValidator<ActionTypeCreate>
{
    private readonly IActionTypeRepository _repository;

    public ActionTypeValidator(IActionTypeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .MustAsync(async (name, _) => await NotDuplicateName(name))
            .WithMessage(_ => $"The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string authorityName, Guid? ignoreId = null) =>
        !await _repository.ActionTypeNameExistsAsync(authorityName, ignoreId);
}
