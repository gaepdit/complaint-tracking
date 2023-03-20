﻿namespace Cts.Domain.Complaints;

/// <inheritdoc />
public class ComplaintManager : IComplaintManager
{
    private IComplaintRepository _repository;

    public ComplaintManager(IComplaintRepository repository) => _repository = repository;

    public async Task SetIdAsync(Complaint complaint) =>
        complaint.SetId(await _repository.GetNextIdAsync());
}
