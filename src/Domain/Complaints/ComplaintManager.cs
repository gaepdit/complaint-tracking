using Cts.Domain.Concerns;
using Cts.Domain.Identity;
using Cts.Domain.Offices;

namespace Cts.Domain.Complaints;

/// <inheritdoc />
public class ComplaintManager : IComplaintManager
{
    public Complaint Create(
        ApplicationUser enteredBy,
        DateTime dateReceived,
        ApplicationUser receivedBy,
        Concern primaryConcern,
        Office currentOffice)
    {
        return new Complaint
        {
            EnteredBy = enteredBy,
            DateReceived = dateReceived,
            ReceivedBy = receivedBy,
            PrimaryConcern = primaryConcern,
            CurrentOffice = currentOffice,
        };
    }
}
