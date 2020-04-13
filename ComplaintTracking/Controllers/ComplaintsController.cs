using ComplaintTracking.Data;
using ComplaintTracking.Models;
using ComplaintTracking.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ComplaintTracking.Controllers
{
    public partial class ComplaintsController : Controller
    {
        #region Constructor

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IErrorLogger _errorLogger;
        private readonly IEmailSender _emailSender;
        private readonly IFileService _fileService;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly DAL _dal;

        private const string objectDisplayName = "Complaint";

        public ComplaintsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IErrorLogger errorLogger,
            IEmailSender emailSender,
            IFileService fileService,
            HtmlEncoder htmlEncoder,
            DAL dal
            )
        {
            _context = context;
            _userManager = userManager;
            _errorLogger = errorLogger;
            _emailSender = emailSender;
            _fileService = fileService;
            _htmlEncoder = htmlEncoder;
            _dal = dal;
        }
        #endregion

        #region Helpers

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
