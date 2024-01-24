using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ComplaintTracking.Data;
using ComplaintTracking.Models;
using ComplaintTracking.Services;
using Microsoft.AspNetCore.Identity;

namespace ComplaintTracking.Controllers
{
    public partial class ComplaintsController
    {
        // Constructor

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IErrorLogger _errorLogger;
        private readonly IEmailSender _emailSender;
        private readonly ICtsAttachmentService _attachmentService;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly DAL _dal;

        private const string ObjectDisplayName = "Complaint";

        public ComplaintsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IErrorLogger errorLogger,
            IEmailSender emailSender,
            ICtsAttachmentService attachmentService,
            HtmlEncoder htmlEncoder,
            DAL dal
        )
        {
            _context = context;
            _userManager = userManager;
            _errorLogger = errorLogger;
            _emailSender = emailSender;
            _attachmentService = attachmentService;
            _htmlEncoder = htmlEncoder;
            _dal = dal;
        }

        // Helpers

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(User);
    }
}
