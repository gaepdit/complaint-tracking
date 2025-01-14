using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.Domain.Entities.Complaints;
using Cts.TestData;

namespace EfRepositoryTests.Complaints;

public class PublicSearchSpec
{
    private IComplaintRepository _repository;
    private Complaint _referenceItem;

    [SetUp]
    public void SetUp()
    {
        _repository = RepositoryHelper.CreateSqlServerRepositoryHelper(this).GetComplaintRepository();
        _referenceItem = ComplaintData.GetComplaints
            .Single(e => e.ComplaintNature == "PublicSearchSpec complaint nature reference");
    }

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task DefaultSpec_ReturnsAllPublic()
    {
        var spec = new ComplaintPublicSearchDto();
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        await using var repository = RepositoryHelper.CreateSqlServerRepositoryHelper(this).GetComplaintRepository();
        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints.Where(e => !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Attachments)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
            .Excluding(e => e.ReviewedBy!.Office)
        );
    }

    [Test]
    public async Task DateSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto
        {
            DateFrom = DateOnly.FromDateTime(_referenceItem.ReceivedDate.Date),
            DateTo = DateOnly.FromDateTime(_referenceItem.ReceivedDate.Date),
        };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.ReceivedDate == _referenceItem.ReceivedDate && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }

    [Test]
    public async Task NatureSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { Description = _referenceItem.ComplaintNature };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.ComplaintNature == _referenceItem.ComplaintNature && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }

    [Test]
    public async Task ConcernSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { Concern = _referenceItem.PrimaryConcern.Id };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.PrimaryConcern.Id == _referenceItem.PrimaryConcern.Id && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
            .Excluding(e => e.Attachments)
        );
    }

    [Test]
    public async Task SourceNameSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { SourceName = _referenceItem.SourceFacilityName };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceFacilityName == _referenceItem.SourceFacilityName && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }

    [Test]
    public async Task CountySpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { County = _referenceItem.ComplaintCounty };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.ComplaintCounty == _referenceItem.ComplaintCounty && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
            .Excluding(e => e.ReviewedBy!.Office)
            .Excluding(e => e.Attachments)
        );
    }

    [Test]
    public async Task StreetSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { Street = _referenceItem.SourceAddress!.Street };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.Street == _referenceItem.SourceAddress.Street
                        && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }

    [Test]
    public async Task Street2Spec_ReturnsFilteredList()
    {
        // "Street" spec filter matches either Street OR Street2 from address
        var spec = new ComplaintPublicSearchDto { Street = _referenceItem.SourceAddress!.Street2 };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.Street2 == _referenceItem.SourceAddress.Street2
                        && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }

    [Test]
    public async Task CitySpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { City = _referenceItem.SourceAddress!.City };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.City == _referenceItem.SourceAddress.City
                        && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }

    [Test]
    public async Task StateSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { State = _referenceItem.SourceAddress!.State };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.State == _referenceItem.SourceAddress.State
                        && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }

    [Test]
    public async Task PostalCodeSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintPublicSearchDto { PostalCode = _referenceItem.SourceAddress!.PostalCode };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        await using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.PostalCode == _referenceItem.SourceAddress.PostalCode
                        && !e.IsDeleted);
        results.Should().BeEquivalentTo(expected, opts => opts
            .Excluding(e => e.CurrentOffice.StaffMembers)
            .Excluding(e => e.CurrentOffice.Assignor)
            .Excluding(e => e.Actions)
            .Excluding(e => e.ComplaintTransitions)
            .Excluding(e => e.EnteredBy!.Office)
            .Excluding(e => e.ReceivedBy!.Office)
            .Excluding(e => e.CurrentOwner!.Office)
        );
    }
}
