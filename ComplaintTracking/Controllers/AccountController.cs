using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.AlertMessages;
using ComplaintTracking.Data;
using ComplaintTracking.Models;
using ComplaintTracking.Services;
using ComplaintTracking.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using static ComplaintTracking.Caching;

namespace ComplaintTracking.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly DAL _dal;

        public AccountController(
            ApplicationDbContext context,
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILoggerFactory loggerFactory,
            DAL dal)
        {
            _context = context;
            _cache = memoryCache;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _dal = dal;
        }

        // GET: /Account/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUserAsync();

            string officeName = null;

            if (currentUser.OfficeId != null)
            {
                officeName = await _dal.GetOfficeName(currentUser.OfficeId);
            }

            var model = new AccountIndexViewModel
            {
                Email = currentUser.Email,
                Phone = currentUser.Phone,
                FullName = currentUser.FullName,
                OfficeName = officeName,
                Roles = await _userManager.GetRolesAsync(currentUser)
            };

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            // Check if active user first
            var u = await _context.Users.AsNoTracking()
                .Where(e => e.Email == model.Email)
                .SingleOrDefaultAsync();

            string msg;
            if (u is {Active: false})
            {
                msg = "Invalid login attempt.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error);
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in");

                // TODO: Customize welcome message
                // But User name is only available after redirect:
                // https://stackoverflow.com/a/38997379/212978
                TempData.SaveAlertForSession("You have been logged in. Welcome back!", AlertStatus.Information);
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out");
                return View("Lockout");
            }

            // If we got this far, something failed, redisplay form
            msg = "Invalid login attempt.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error);
            return View(model);
        }

        // GET: /Account/Logout
        // (on the off-chance someone gets navigated here accidentally)
        [HttpGet]
        [ActionName("Logout")]
        [AllowAnonymous]
        public IActionResult Logout_Get()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out");

            TempData.SaveAlertForSession("You have been signed out.", AlertStatus.Information);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // GET: /Account/ConfirmEmail
        [HttpGet, HttpHead]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (HttpMethods.IsHead(HttpContext.Request.Method))
            {
                // This isn't a technically correct HEAD response, since the HTTP headers will not match what
                // would be returned from a valid GET request, but it's close enough since the purpose is to
                // satisfy Microsoft's Safe Links feature while avoiding a 405 Method Not Allowed crash report
                // and without validating the code.
                return Ok();
            }

            if (userId == null || code == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return View("ConfirmAccountFailed");
            }

            var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            return RedirectToAction(nameof(SetPassword), new {userId = user.Id, code = passwordResetCode});
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SetPassword(string userId, string code)
        {
            if (code == null || userId == null)
            {
                return NotFound();
            }

            var model = new SetPasswordViewModel
            {
                Code = code,
                UserId = userId
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            string msg;

            if (!ModelState.IsValid)
            {
                msg = "Your password was not set. Please fix the errors shown below.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is {Active: true})
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    msg = "Your password has been set. Please sign in.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                    return RedirectToAction(nameof(Login));
                }

                AddErrors(result);
            }

            msg = "An error occurred. Your password was not set.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            return View();
        }

        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            string msg;

            if (!ModelState.IsValid)
            {
                msg = "Your password has not been changed. Please fix the errors shown below.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation(3, "User changed their password successfully");

                    msg = "Your password has been changed.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                    return RedirectToAction(nameof(Index));
                }

                AddErrors(result);
                msg = "Your password has not been changed. Please fix the errors shown below.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");

                return View(model);
            }

            msg = "An error has occurred.";
            TempData.SaveAlertForSession(msg, AlertStatus.Error);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not {Active: true})
                {
                    // Don't reveal that the user does not exist or is not active
                    return View("ForgotPasswordConfirmation");
                }

                string code;
                string callbackUrl;

                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code},
                        HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(
                        model.Email,
                        EmailTemplates.ConfirmNewAccount.Subject,
                        string.Format(EmailTemplates.ConfirmNewAccount.PlainBody, model.Email, callbackUrl),
                        string.Format(EmailTemplates.ConfirmNewAccount.HtmlBody, model.Email, callbackUrl));
                    return View("ConfirmAccount");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                code = await _userManager.GeneratePasswordResetTokenAsync(user);
                callbackUrl = Url.Action(nameof(ResetPassword), "Account", new {userId = user.Id, code},
                    HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(
                    model.Email,
                    EmailTemplates.ResetPassword.Subject,
                    string.Format(EmailTemplates.ResetPassword.PlainBody, callbackUrl),
                    string.Format(EmailTemplates.ResetPassword.HtmlBody, callbackUrl));

                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [HttpGet, HttpHead]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                return NotFound();
            }

            return View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            string msg;

            if (!ModelState.IsValid)
            {
                msg = "Your password was not reset. Please fix the errors shown below.";
                ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is {Active: true})
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    msg = "Your password has been reset. Please sign in.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");
                    return RedirectToAction(nameof(Login));
                }

                AddErrors(result);
            }

            msg = "An error occurred. Your password was not reset.";
            ViewData["AlertMessage"] = new AlertViewModel(msg, AlertStatus.Error, "Error");
            return View();
        }

        // GET /Account/AccessDenied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: /Account/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await GetCurrentUserAsync();

            var model = new EditAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                OfficeId = user.OfficeId,
                OfficeSelectList = await _dal.GetOfficesSelectListAsync(),
            };

            return View(model);
        }

        // POST: /Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            string msg;

            var user = await GetCurrentUserAsync();

            if (await _dal.EmailAlreadyUsedAsync(model.Email, user.Id))
            {
                ModelState.AddModelError("Email", "The email address is already in use.");
            }

            if (ModelState.IsValid)
            {
                var oldEmail = user.Email.Trim();

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
                    // check if the email address was changed; send notification emails to both if changed
                    if (!string.Equals(oldEmail, user.Email, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await _emailSender.SendEmailAsync(
                            oldEmail,
                            EmailTemplates.NotifyEmailChange.Subject,
                            string.Format(EmailTemplates.NotifyEmailChange.PlainBody, oldEmail, user.Email,
                                CTS.AdminEmail),
                            string.Format(EmailTemplates.NotifyEmailChange.HtmlBody, oldEmail, user.Email,
                                CTS.AdminEmail));
                        await _emailSender.SendEmailAsync(
                            user.Email,
                            EmailTemplates.NotifyEmailChange.Subject,
                            string.Format(EmailTemplates.NotifyEmailChange.PlainBody, oldEmail, user.Email,
                                CTS.AdminEmail),
                            string.Format(EmailTemplates.NotifyEmailChange.HtmlBody, oldEmail, user.Email,
                                CTS.AdminEmail));
                    }

                    _logger.LogInformation(3, "User updated");

                    msg = "Your profile was updated.";
                    TempData.SaveAlertForSession(msg, AlertStatus.Success, "Success");

                    return RedirectToAction(nameof(Index));
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            msg = "Your profile was not updated. Please fix the errors shown below.";
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

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
