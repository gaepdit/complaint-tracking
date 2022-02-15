using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Models;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ComplaintTracking.Caching;

namespace ComplaintTracking.Controllers
{
    public class ConcernsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string ObjectDisplayName = "Area of Concern";

        public ConcernsController(
            ApplicationDbContext context,
            IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        // GET: Concerns
        public async Task<IActionResult> Index()
        {
            var model = new ConcernIndexViewModel()
            {
                Concerns = await _context.LookupConcerns.AsNoTracking()
                .OrderBy(e => e.Name)
                .Select(e => new ConcernViewModel(e))
                .ToListAsync()
            };

            return View(model);
        }

        // GET: Concerns/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _context.LookupConcerns.AsNoTracking()
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            var model = new ConcernViewModel(item);

            return View(model);
        }

        // GET: Concerns/Create
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Concerns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Create(CreateConcernViewModel model)
        {
            if (await ConcernNameExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "The name already exists.");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var item = new Concern()
                {
                    Name = model.Name
                };

                try
                {
                    _cache.Remove(CacheKeys.AreasOfConcernSelectList);

                    _context.Add(item);
                    await _context.SaveChangesAsync();

                    msg = $"The {ObjectDisplayName} has been created.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id = item.Id });
                }
                catch
                {
                    msg = $"There was an error saving the {ObjectDisplayName}. Please try again or contact support.";
                }
            }
            else
            {
                msg = $"The {ObjectDisplayName} was not created. Please fix the errors shown below.";
            }

            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        // GET: Concerns/Edit/5
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _context.LookupConcerns.AsNoTracking()
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            var model = new EditConcernViewModel(item);

            return View(model);
        }

        // POST: Concerns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Edit(Guid id, EditConcernViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (await ConcernNameExistsAsync(model.Name, id))
            {
                ModelState.AddModelError("Name", "The name already exists.");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var item = new Concern()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Active = model.Active
                };

                try
                {
                    _cache.Remove(CacheKeys.AreasOfConcernSelectList);

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ConcernExists(model.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                msg = $"The {ObjectDisplayName} was updated.";
                TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                return RedirectToAction("Details", new { id = model.Id });
            }

            msg = $"The {ObjectDisplayName} was not updated. Please fix the errors shown below.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        // Local functions

        private Task<bool> ConcernExists(Guid id)
        {
            return _context.LookupConcerns.AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        private Task<bool> ConcernNameExistsAsync(string name, Guid? ignoreId = null)
        {
            if (ignoreId.HasValue)
            {
                return _context.LookupConcerns.AsNoTracking()
                    .AnyAsync(e => e.Name == name && e.Id != ignoreId.Value);
            }

            return _context.LookupConcerns.AsNoTracking()
                .AnyAsync(e => e.Name == name);
        }
    }
}
