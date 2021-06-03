using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static ComplaintTracking.Caching;

namespace ComplaintTracking.Controllers
{
    public class OfficesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly DAL _dal;
        private const string objectDisplayName = "Office";

        public OfficesController(
            ApplicationDbContext context,
            IMemoryCache memoryCache,
            DAL dal)
        {
            _context = context;
            _cache = memoryCache;
            _dal = dal;
        }

        public async Task<IActionResult> Index()
        {
            var model = new OfficeIndexViewModel()
            {
                Offices = await _context.LookupOffices.AsNoTracking()
                    .Include(e => e.MasterUser)
                    .OrderBy(e => e.Name)
                    .Select(e => new OfficeViewModel(e))
                    .ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _context.LookupOffices.AsNoTracking()
                .Include(e => e.MasterUser)
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            var model = new OfficeViewModel(item);

            return View(model);
        }

        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Create()
        {
            var model = new CreateOfficeViewModel()
            {
                UsersSelectList = await _dal.GetAllUsersSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Create(CreateOfficeViewModel model)
        {
            if (await _dal.OfficeNameExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "The name already exists.");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var item = new Office()
                {
                    Name = model.Name,
                    MasterUserId = model.MasterUserId
                };

                _cache.Remove(CacheKeys.OfficesSelectList);
                _cache.Remove(CacheKeys.OfficesSelectListRequireMaster);

                try
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();

                    msg = $"The {objectDisplayName} has been created.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new {id = item.Id});
                }
                catch
                {
                    msg = $"There was an error saving the {objectDisplayName}. Please try again or contact support.";
                }
            }
            else
            {
                msg = $"The {objectDisplayName} was not created. Please fix the errors shown below.";
            }

            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            model.UsersSelectList = await _dal.GetAllUsersSelectListAsync();
            return View(model);
        }

        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _context.LookupOffices.AsNoTracking()
                .Include(e => e.MasterUser)
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            var model = new EditOfficeViewModel(item)
            {
                UsersSelectList = await _dal.GetAllUsersSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Edit(Guid id, EditOfficeViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (await _dal.OfficeNameExistsAsync(model.Name, id))
            {
                ModelState.AddModelError("Name", "The name already exists.");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var item = new Office()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Active = model.Active,
                    MasterUserId = model.MasterUserId
                };

                _cache.Remove(CacheKeys.OfficesSelectList);
                _cache.Remove(CacheKeys.OfficesSelectListRequireMaster);

                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _dal.OfficeExists(model.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                msg = $"The {objectDisplayName} was updated.";
                TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                return RedirectToAction("Details", new {id = model.Id});
            }

            msg = $"The {objectDisplayName} was not updated. Please fix the errors shown below.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            model.UsersSelectList = await _dal.GetAllUsersSelectListAsync();
            return View(model);
        }
    }
}
