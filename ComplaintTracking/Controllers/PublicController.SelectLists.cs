using System.Linq;
using System.Threading.Tasks;
using ComplaintTracking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Controllers
{
    public partial class PublicController
    {
        private async Task<SelectList> GetCountiesSelectListAsync()
        {
            var items = await _context.LookupCounties.AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync();

            return new SelectList(items, nameof(County.Id), nameof(County.Name));
        }

        private async Task<SelectList> GetStatesSelectListAsync()
        {
            var items = await _context.LookupStates.AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync();

            return new SelectList(items, nameof(State.Id), nameof(State.Name));
        }

        private async Task<SelectList> GetAreasOfConcernSelectListAsync()
        {
            var items = await _context.LookupConcerns.AsNoTracking()
                .Where(t => t.Active)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return new SelectList(items, nameof(Concern.Id), nameof(Concern.Name));
        }
    }
}
