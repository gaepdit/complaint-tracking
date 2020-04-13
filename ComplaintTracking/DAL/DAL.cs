using ComplaintTracking.Data;
using ComplaintTracking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace ComplaintTracking
{
    public partial class DAL
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly UserManager<ApplicationUser> _userManager;

        public DAL(
            ApplicationDbContext context,
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _cache = memoryCache;
            _userManager = userManager;
        }
    }
}
