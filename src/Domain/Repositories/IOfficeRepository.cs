using Cts.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cts.Domain.Repositories;

public interface IOfficeRepository : IDisposable
{
    Task<OfficeView> GetOfficeAsync(Guid id);
    Task<IReadOnlyList<OfficeView>> ListOfficesAsync();
    Task<Guid> CreateOfficeAsync(OfficeCreate resource);
    Task UpdateOfficeAsync(OfficeEdit resource);
    Task UpdateOfficeStatusAsync(Guid id, bool newActiveStatus);
    Task<bool> OfficeNameExistsAsync(string name, Guid? ignoreId = null);
}

public class OfficeView
{
    public OfficeView(Office item)
    {
        Id = item.Id;
        Name = item.Name;
        Active = item.Active;
        MasterUser = item.MasterUser;
    }

    public Guid Id { get; }
    public string Name { get; }
    public bool Active { get; }
    public ApplicationUser? MasterUser { get; }
}

public class OfficeCreate
{
    public OfficeCreate() { }

    public OfficeCreate(OfficeView item)
    {
        Name = item.Name;
        MasterUser = item.MasterUser;
    }

    public string Name { get; set; } = string.Empty;
    public ApplicationUser? MasterUser { get; set; }
    
    public void TrimAll() => Name = Name.Trim();
}

public class OfficeEdit : OfficeCreate
{
    public OfficeEdit() { }

    public OfficeEdit(OfficeView item) : base(item)
    {
        Id = item.Id;
        Active = item.Active;
    }

    [HiddenInput]
    public Guid Id { get; }

    public bool Active { get; } = true;
}

public class OfficeValidator : AbstractValidator<OfficeCreate>
{
    private readonly IOfficeRepository _repository;

    public OfficeValidator(IOfficeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .MustAsync(async (name, _) => await NotDuplicateName(name))
            .WithMessage(_ => $"The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string authorityName, Guid? ignoreId = null) =>
        !await _repository.OfficeNameExistsAsync(authorityName, ignoreId);
}
