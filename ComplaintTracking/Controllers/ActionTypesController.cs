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
    public class ActionTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string _objectDisplayName = "Complaint Action Type";

        public ActionTypesController(
            ApplicationDbContext context,
            IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        // GET: ActionTypes
        public async Task<IActionResult> Index()
        {
            var model = new ActionTypeIndexViewModel()
            {
                ActionTypes = await _context.LookupActionTypes.AsNoTracking()
                .OrderBy(e => e.Name)
                .Select(e => new ActionTypeViewModel(e))
                .ToListAsync()
            };

            return View(model);
        }

        // GET: ActionTypes/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.LookupActionTypes.AsNoTracking()
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            var model = new ActionTypeViewModel(item);

            return View(model);
        }

        // GET: ActionTypes/Create
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActionTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Create(CreateActionTypeViewModel model)
        {
            if (await ActionTypeNameExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "The name already exists.");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var item = new ActionType()
                {
                    Name = model.Name
                };

                try
                {
                    _cache.Remove(CacheKeys.ActionTypesSelectList);

                    _context.Add(item);
                    await _context.SaveChangesAsync();

                    msg = string.Format("The {0} has been created.", _objectDisplayName);
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction("Details", new { id = item.Id });
                }
                catch
                {
                    msg = string.Format("There was an error saving the {0}. Please try again or contact support.", _objectDisplayName);
                }
            }
            else
            {
                msg = string.Format("The {0} was not created. Please fix the errors shown below.", _objectDisplayName);
            }

            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        // GET: ActionTypes/Edit/5
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.LookupActionTypes.AsNoTracking()
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            var model = new EditActionTypeViewModel(item);

            return View(model);
        }

        // POST: ActionTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(CtsRole.DivisionManager))]
        public async Task<IActionResult> Edit(Guid id, EditActionTypeViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (await ActionTypeNameExistsAsync(model.Name, id))
            {
                ModelState.AddModelError("Name", "The name already exists.");
            }

            string msg;

            if (ModelState.IsValid)
            {
                var item = new ActionType()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Active = model.Active
                };

                try
                {
                    _cache.Remove(CacheKeys.ActionTypesSelectList);

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ActionTypeExists(model.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                msg = string.Format("The {0} was updated.", _objectDisplayName);
                TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                return RedirectToAction("Details", new { id = model.Id });
            }

            msg = string.Format("The {0} was not updated. Please fix the errors shown below.", _objectDisplayName);
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

            return View(model);
        }

        // Local functions

        private Task<bool> ActionTypeExists(Guid id)
        {
            return _context.LookupActionTypes.AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }

        private Task<bool> ActionTypeNameExistsAsync(string name, Guid? ignoreId = null)
        {
            if (ignoreId.HasValue)
            {
                return _context.LookupActionTypes.AsNoTracking()
                    .AnyAsync(e => e.Name == name & e.Id != ignoreId.Value);
            }
            else
            {
                return _context.LookupActionTypes.AsNoTracking()
                    .AnyAsync(e => e.Name == name);
            }
        }
    }
}
