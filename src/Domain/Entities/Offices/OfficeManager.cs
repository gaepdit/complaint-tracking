namespace Cts.Domain.Entities.Offices;

public class OfficeManager : NamedEntityManager<Office, IOfficeRepository>, IOfficeManager
{
    public OfficeManager(IOfficeRepository repository) : base(repository) { }
}
