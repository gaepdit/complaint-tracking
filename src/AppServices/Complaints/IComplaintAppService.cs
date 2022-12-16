namespace Cts.AppServices.Complaints;

public interface IComplaintAppService : IDisposable
{
    Task<ComplaintPublicViewDto?> GetPublicViewAsync(int id, CancellationToken token = default);
}