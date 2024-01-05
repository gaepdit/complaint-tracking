namespace Cts.Domain.Entities.Offices;

public class OfficeManager(IOfficeRepository repository)
    : NamedEntityManager<Office, IOfficeRepository>(repository), IOfficeManager;
