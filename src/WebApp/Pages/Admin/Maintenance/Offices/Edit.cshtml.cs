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
public class EditModel : PageModel
{
    private readonly IOfficeAppService _officeService;
    private readonly IStaffAppService _staffService;
    private readonly IValidator<OfficeUpdateDto> _validator;

    public EditModel(
        IOfficeAppService officeService,
        IStaffAppService staffService,
        IValidator<OfficeUpdateDto> validator)
    {
        _officeService = officeService;
        _staffService = staffService;
        _validator = validator;
    }

    [BindProperty]
    public OfficeUpdateDto Item { get; set; } = default!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    public SelectList ActiveStaffMembers { get; private set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Office;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return RedirectToPage("Index");
        var item = await _officeService.FindForUpdateAsync(id.Value);
        if (item == null) return NotFound();

        Item = item;
        OriginalName = Item.Name;

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

        await _officeService.UpdateAsync(Item);

        HighlightId = Item.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"\"{Item.Name}\" successfully updated.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        ActiveStaffMembers = (await _staffService.GetActiveStaffMembersAsync()).ToSelectList();
}
