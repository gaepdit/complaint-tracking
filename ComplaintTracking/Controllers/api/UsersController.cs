using ComplaintTracking.Data;
using ComplaintTracking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            if (!string.IsNullOrEmpty(id))
            {
                if (Guid.TryParse(id, out Guid officeGuid))
                {
                    if (officeGuid != null && officeGuid != default)
                    {
                        var user = await GetCurrentUserAsync();
                        if (user != null)
                        {
                            string officeMasterId = (await _context.LookupOffices.AsNoTracking()
                                .Where(e => e.Id == officeGuid)
                                .SingleOrDefaultAsync())?
                                .MasterUserId;

                            bool currentUserIsMaster = officeMasterId != null
                                && user.Id == officeMasterId;

                            if (user.OfficeId == officeGuid
                                || User.IsInRole(CtsRole.DivisionManager.ToString())
                                || currentUserIsMaster)
                            {
                                return Json(await _dal.GetUsersSelectListAsync(officeGuid));
                            }
                        }
                    }
                }
            }
            return Json(null);
        }

        [HttpGet("GetAll")]
        // public async Task<JsonResult> GetAll()
        public JsonResult GetAll()
        {
            //return Json(await _dal.GetAllUsersSelectListAsync(true));
            return Json(null);
        }

        [HttpGet("GetAll/{id}")]
        public async Task<JsonResult> GetAll(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (Guid.TryParse(id, out Guid officeGuid))
                {
                    if (officeGuid != null && officeGuid != default)
                    {
                        return Json(await _dal.GetUsersSelectListAsync(officeGuid, true));
                    }
                }
            }
            return Json(null);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
