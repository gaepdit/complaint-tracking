using System;
using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.Data;
using ComplaintTracking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Controllers.Api
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DAL _dal;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            DAL dal)
        {
            _context = context;
            _userManager = userManager;
            _dal = dal;
        }

        [HttpGet("ByOffice/{id}")]
        public async Task<JsonResult> ByOffice(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var officeGuid) || officeGuid == Guid.Empty)
                return Json(null);

            var user = await GetCurrentUserAsync();
            if (user == null) return Json(null);

            var officeMasterId = (await _context.LookupOffices.AsNoTracking()
                    .Where(e => e.Id == officeGuid)
                    .SingleOrDefaultAsync())?
                .MasterUserId;

            var currentUserIsMaster = officeMasterId != null
                && user.Id == officeMasterId;

            if (user.OfficeId == officeGuid
                || User.IsInRole(CtsRole.DivisionManager.ToString())
                || currentUserIsMaster)
            {
                return Json(await _dal.GetUsersSelectListAsync(officeGuid));
            }

            return Json(null);
        }

        [HttpGet("GetAll")]
        public JsonResult GetAll()
        {
            return Json(null);
        }

        [HttpGet("GetAll/{id}")]
        public async Task<JsonResult> GetAll(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var officeGuid)) return Json(null);

            return officeGuid != Guid.Empty
                ? Json(await _dal.GetUsersSelectListAsync(officeGuid, true))
                : Json(null);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
