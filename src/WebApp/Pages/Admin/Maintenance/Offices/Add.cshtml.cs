﻿using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class AddModel : PageModel
{
    private readonly IOfficeAppService _officeService;
    private readonly IStaffAppService _staffService;
    private readonly IValidator<OfficeCreateDto> _validator;

    public AddModel(
        IOfficeAppService officeService,
        IStaffAppService staffService,
        IValidator<OfficeCreateDto> validator)
    {
        _officeService = officeService;
        _staffService = staffService;
        _validator = validator;
    }

    [BindProperty]
    public OfficeCreateDto Item { get; set; } = default!;

    public SelectList ActiveStaffMembers { get; private set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Office;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validationResult = await _validator.ValidateAsync(Item);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(Item));

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        var id = await _officeService.CreateAsync(Item);

        HighlightId = id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"\"{Item.Name}\" successfully added.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        ActiveStaffMembers = (await _staffService.GetActiveStaffMembersAsync()).ToSelectList();
}
