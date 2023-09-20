using Cts.Domain;
using GaEpd.AppLibrary.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.DtoBase;

public abstract record StandardNamedEntityViewDto
(
    Guid Id,
    string Name,
    bool Active
) : INamedEntity;

public abstract record StandardNamedEntityCreateDto
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength, MinimumLength = AppConstants.MinimumNameLength)]
    string Name
);

public abstract record StandardNamedEntityUpdateDto
(
    Guid Id,
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength, MinimumLength = AppConstants.MinimumNameLength)]
    string Name,
    bool Active
);
