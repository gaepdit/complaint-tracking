using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Complaints;
using Cts.TestData;

namespace IntegrationTests.Complaints;

[NonParallelizable]
public class PublicSearchSpec
{
    [Test]
    public async Task DefaultSpec_ReturnsAllPublic()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        
        var spec = new ComplaintPublicSearchDto();
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task DateSpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");
        
        var spec = new ComplaintPublicSearchDto
        {
            DateFrom = referenceItem.DateReceived,
            DateTo = referenceItem.DateReceived,
        };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.DateReceived == referenceItem.DateReceived
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task NatureSpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");
        
        var spec = new ComplaintPublicSearchDto { Nature = referenceItem.ComplaintNature };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.ComplaintNature == referenceItem.ComplaintNature
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task ConcernSpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");
        
        var spec = new ComplaintPublicSearchDto { Type = referenceItem.PrimaryConcern.Id };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.PrimaryConcern.Id == referenceItem.PrimaryConcern.Id
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task SourceNameSpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");

        var spec = new ComplaintPublicSearchDto { SourceName = referenceItem.SourceFacilityName };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceFacilityName == referenceItem.SourceFacilityName
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task CountySpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");

        var spec = new ComplaintPublicSearchDto { County = referenceItem.ComplaintCounty };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.ComplaintCounty == referenceItem.ComplaintCounty
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task StreetSpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");

        var spec = new ComplaintPublicSearchDto { Street = referenceItem.SourceAddress!.Street };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                && e.SourceAddress.Street == referenceItem.SourceAddress.Street
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task Street2Spec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");

        // "Street" spec filter matches either Street OR Street2 from address
        var spec = new ComplaintPublicSearchDto { Street = referenceItem.SourceAddress!.Street2 };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                && e.SourceAddress.Street2 == referenceItem.SourceAddress.Street2
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task CitySpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");

        var spec = new ComplaintPublicSearchDto { City = referenceItem.SourceAddress!.City };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                && e.SourceAddress.City == referenceItem.SourceAddress.City
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task StateSpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");

        var spec = new ComplaintPublicSearchDto { State = referenceItem.SourceAddress!.State };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                && e.SourceAddress.State == referenceItem.SourceAddress.State
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }

    [Test]
    public async Task PostalCodeSpec_ReturnsFilteredList()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();
        var referenceItem = ComplaintData.GetComplaints.Single(e => e.ComplaintNature == "PublicSearchSpec reference");

        var spec = new ComplaintPublicSearchDto { PostalCode = referenceItem.SourceAddress!.PostalCode };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var results = await repository.GetListAsync(predicate);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                && e.SourceAddress.PostalCode == referenceItem.SourceAddress.PostalCode
                && e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected, opts =>
            opts.Excluding(e => e.CurrentOffice.StaffMembers));
    }
}
