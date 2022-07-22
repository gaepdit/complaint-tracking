using ComplaintTracking.Entities.ActionTypes;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ComplaintTracking.Entities.SeedData;

public class ActionTypeSeedData : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<ActionType, Guid> _repository;

    public ActionTypeSeedData(IRepository<ActionType, Guid> repository) => _repository = repository;

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _repository.GetCountAsync() <= 0) await _repository.InsertManyAsync(ActionTypeData(), autoSave: true);
    }

    private static IEnumerable<ActionType> ActionTypeData()
    {
        string[] items =
        {
                "Initial investigation",
                "Follow-up investigation",
                "Referred to",
                "Notice of violation",
                "Initial investigation report",
                "Follow-up investigation report",
                "Status letter",
                "Other",
                "Consent/administrative order",
        };

        return items.Select(i => new ActionType { Name = i, Active = true });
    }
}
