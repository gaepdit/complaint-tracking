using Microsoft.AspNetCore.Mvc;

namespace ComplaintTracking.Controllers
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index() => View();
    }
}
