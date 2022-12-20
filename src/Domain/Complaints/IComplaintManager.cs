using Cts.Domain.Concerns;
using Cts.Domain.Identity;
using Cts.Domain.Offices;

namespace Cts.Domain.Complaints;

public interface IComplaintManager
{
    public Complaint Create(
        ApplicationUser enteredBy,
        DateTime dateReceived,
        ApplicationUser receivedBy,
        Concern primaryConcern,
        Office currentOffice);
}
