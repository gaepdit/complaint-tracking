using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Generic;
using ComplaintTracking.Models;
using ComplaintTracking.Services;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static ComplaintTracking.Caching;
using static ComplaintTracking.ViewModels.UserIndexViewModel;

namespace ComplaintTracking.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly DAL _dal;

        public UsersController(
            ApplicationDbContext context,
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ILoggerFactory loggerFactory,
            DAL dal)
        {
            _context = context;
            _cache = memoryCache;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _dal = dal;
        }

        // GET: Users
        public async Task<IActionResult> Index(
            int page = 1,
            SortBy sort = SortBy.NameAsc,
            string name = null,
            Guid? office = null,
            UserStatus? status = null,
            CtsRole? role = null
        )
        {
            // Filters
            var users = _context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                users = users.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }

            if (office.HasValue)
            {
                users = users.Where(e => e.OfficeId == office.Value);
            }

            users = status switch
            {
                null => users.Where(e => e.Active),
                UserStatus.Inactive => users.Where(e => !e.Active),
                _ => users
            };

            if (role is not null)
            {
                var identityRole = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == role.ToString());
                if (identityRole is not null)
                {
                    users = users.Where(e => e.Roles.Any(r => r.RoleId == identityRole.Id));
                }
            }

            // ViewModel
            var model = new UserIndexViewModel()
            {
                CurrentUserId = (await GetCurrentUserAsync()).Id,
                Name = name,
                Office = office,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                Status = status,
                Sort = sort,
                Role = role,
            };

            // Sort
            switch (sort)
            {
                case SortBy.NameAsc:
                    users = users.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
                    model.NameSortAction = SortBy.NameDesc;
                    break;
                case SortBy.NameDesc:
                    users = users.OrderByDescending(e => e.LastName).ThenByDescending(e => e.FirstName);
                    break;
                case SortBy.OfficeAsc:
                    users = users.OrderBy(e => e.Office.Name).ThenBy(e => e.LastName).ThenBy(e => e.FirstName);
                    model.OfficeSortAction = SortBy.OfficeDesc;
                    break;
                case SortBy.OfficeDesc:
                    users = users.OrderByDescending(e => e.Office.Name).ThenBy(e => e.LastName)
                        .ThenBy(e => e.FirstName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sort), sort, null);
            }

            // Count
            var count = await users.CountAsync().ConfigureAwait(true);

            // Include
            users = users
                .Include(e => e.Office);

            // Paging
            users = users
                .Skip((page - 1) * CTS.PageSize)
                .Take(CTS.PageSize);

            // Select
            var items = await users
                .Select(e => new UserViewModel(e))
                .ToListAsync().ConfigureAwait(true);

            model.Users = new PaginatedList<UserViewModel>(items, count, page);

            return View(model);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _context.Users.AsNoTracking()
                .Include(e => e.Office)
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            if ((await GetCurrentUserAsync()).Id == user.Id)
            {
                return RedirectToAction(nameof(AccountController.Index), "Account");
            }

            var model = new UserViewModel(user);
            model.SetRoles(await _userManager.GetRolesAsync(user));

            return View(model);
        }

        // GET: /Users/Register
        [HttpGet]
        [Authorize(Roles = nameof(CtsRole.DivisionManager) + "," + nameof(CtsRole.UserAdmin))]
        public async Task<IActionResult> Register()
        {
            var currentUser = await GetCurrentUserAsync();

            var model = new RegisterUserViewModel()
            {
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                CurrentUserIsDivisionManager =
                    await _userManager.IsInRoleAsync(currentUser, nameof(CtsRole.DivisionManager))
            };

            return View(model);
        }

        // POST: /Users/Register
        [HttpPost]
        [Authorize(Roles = nameof(CtsRole.DivisionManager) + "," + nameof(CtsRole.UserAdmin))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            string msg;

            var currentUser = await GetCurrentUserAsync();
            bool currentUserIsDivisionManager =
                await _userManager.IsInRoleAsync(currentUser, nameof(CtsRole.DivisionManager));

            if (await _dal.EmailAlreadyUsedAsync(model.Email))
            {
                ModelState.AddModelError("Email", "The email address is already in use.");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Phone = model.Phone,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    OfficeId = model.OfficeId
                };

                var pwd = GenerateNewPassword();

                _cache.Remove(CacheKeys.UsersSelectList);
                _cache.Remove(CacheKeys.UsersIncludeInactiveSelectList);

                var result = await _userManager.CreateAsync(user, pwd);
                if (result.Succeeded)
                {
                    if (currentUserIsDivisionManager) // Only Division Manager can create new Division Managers
                    {
                        if (model.IsDivisionManager)
                        {
                            await _userManager.AddToRoleAsync(user, nameof(CtsRole.DivisionManager));
                        }
                        else
                        {
                            await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.DivisionManager));
                        }
                    }

                    if (model.IsDataExporter)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.DataExport));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.DataExport));
                    }

                    if (model.IsAttachmentEditor)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.AttachmentsEditor));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.AttachmentsEditor));
                    }

                    if (model.IsManager)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.Manager));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.Manager));
                    }

                    if (model.IsUserAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.UserAdmin));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.UserAdmin));
                    }

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(
                        model.Email,
                        EmailTemplates.ConfirmNewAccount.Subject,
                        string.Format(EmailTemplates.ConfirmNewAccount.PlainBody, model.Email, callbackUrl),
                        string.Format(EmailTemplates.ConfirmNewAccount.HtmlBody, model.Email, callbackUrl),
                        replyTo: currentUser.Email);

                    _logger.LogInformation(3, "User created a new account with password");

                    msg =
                        "The new user account has been created, and a confirmation email has been sent to the email provided.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            msg = "The user account was not created. Please fix the errors shown below.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            model.OfficeSelectList = await _dal.GetOfficesSelectListAsync();
            return View(model);
        }

        // GET: `Users/Edit/{id}`
        [HttpGet]
        [Authorize(Roles = nameof(CtsRole.DivisionManager) + "," + nameof(CtsRole.UserAdmin))]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser.Id == user.Id)
            {
                return RedirectToAction(nameof(AccountController.Edit), "Account");
            }

            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Active = user.Active,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                OfficeId = user.OfficeId,
                IsDivisionManager = await _userManager.IsInRoleAsync(user, nameof(CtsRole.DivisionManager)),
                IsManager = await _userManager.IsInRoleAsync(user, nameof(CtsRole.Manager)),
                IsUserAdmin = await _userManager.IsInRoleAsync(user, nameof(CtsRole.UserAdmin)),
                IsDataExporter = await _userManager.IsInRoleAsync(user, nameof(CtsRole.DataExport)),
                IsAttachmentEditor = await _userManager.IsInRoleAsync(user, nameof(CtsRole.AttachmentsEditor)),
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
                CurrentUserIsDivisionManager =
                    await _userManager.IsInRoleAsync(currentUser, nameof(CtsRole.DivisionManager)),
            };

            return View(model);
        }

        // POST: /Users/Edit
        [HttpPost]
        [Authorize(Roles = nameof(CtsRole.DivisionManager) + "," + nameof(CtsRole.UserAdmin))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            string msg;

            if (id != model.Id)
            {
                return NotFound();
            }

            var currentUser = await GetCurrentUserAsync();
            var currentUserIsDivisionManager =
                await _userManager.IsInRoleAsync(currentUser, nameof(CtsRole.DivisionManager));

            if (await _dal.EmailAlreadyUsedAsync(model.Email, id))
            {
                ModelState.AddModelError("Email", "The email address is already in use.");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                var oldEmail = user.Email.Trim();

                user.Active = model.Active;
                user.Email = model.Email.Trim();
                user.Phone = model.Phone;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Email;
                user.OfficeId = model.OfficeId;

                _cache.Remove(CacheKeys.UsersSelectList);
                _cache.Remove(CacheKeys.UsersIncludeInactiveSelectList);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (currentUserIsDivisionManager) // Only Division Manager can add or remove Division Manager role
                    {
                        if (model.IsDivisionManager)
                        {
                            await _userManager.AddToRoleAsync(user, nameof(CtsRole.DivisionManager));
                        }
                        else
                        {
                            await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.DivisionManager));
                        }
                    }

                    if (model.IsDataExporter)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.DataExport));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.DataExport));
                    }

                    if (model.IsAttachmentEditor)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.AttachmentsEditor));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.AttachmentsEditor));
                    }

                    if (model.IsManager)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.Manager));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.Manager));
                    }

                    if (model.IsUserAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, nameof(CtsRole.UserAdmin));
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, nameof(CtsRole.UserAdmin));
                    }

                    // check if the email address was changed; send notification emails to both if changed
                    if (oldEmail.ToUpper() != user.Email.ToUpper() && user.Active)
                    {
                        await _emailSender.SendEmailAsync(
                            oldEmail,
                            EmailTemplates.NotifyEmailChange.Subject,
                            string.Format(EmailTemplates.NotifyEmailChange.PlainBody, oldEmail, user.Email,
                                CTS.AdminEmail),
                            string.Format(EmailTemplates.NotifyEmailChange.HtmlBody, oldEmail, user.Email,
                                CTS.AdminEmail),
                            replyTo: currentUser.Email);
                        await _emailSender.SendEmailAsync(
                            user.Email,
                            EmailTemplates.NotifyEmailChange.Subject,
                            string.Format(EmailTemplates.NotifyEmailChange.PlainBody, oldEmail, user.Email,
                                CTS.AdminEmail),
                            string.Format(EmailTemplates.NotifyEmailChange.HtmlBody, oldEmail, user.Email,
                                CTS.AdminEmail),
                            replyTo: currentUser.Email);
                    }

                    _logger.LogInformation(3, "User updated");

                    msg = "The user profile was updated.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            msg = "The user profile was not updated. Please fix the errors shown below.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            model.OfficeSelectList = await _dal.GetOfficesSelectListAsync();
            return View(model);
        }

        // Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private static string GenerateNewPassword()
        {
            var data = new byte[16];
            RandomNumberGenerator.Create().GetBytes(data);
            return BitConverter.ToString(data);
        }

        public IActionResult Roles()
        {
            return View();
        }
    }
}
