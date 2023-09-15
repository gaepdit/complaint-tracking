﻿using Cts.AppServices.Attachments;
using Cts.Domain.Entities.Complaints;
using Cts.TestData;

namespace EfRepositoryTests.Complaints;

public class GetAttachmentsList
{
    private IComplaintRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var complaint = ComplaintData.GetComplaints.First();
        var items = AttachmentData.GetAttachments.Where(e => e.Complaint.Id == complaint.Id && !e.IsDeleted);

        var result = await _repository.GetAttachmentsListAsync(AttachmentFilters.PublicIdPredicate(complaint.Id));

        result.Should().BeEquivalentTo(items, opts => opts
            .Excluding(e => e.UploadedBy!.Office)
            .Excluding(e => e.Complaint));
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetAttachmentsListAsync(e => e.Id == Guid.Empty);
        result.Should().BeEmpty();
    }
}