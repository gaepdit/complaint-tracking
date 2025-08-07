using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.Domain.Entities.Complaints;
using Cts.TestData;

namespace EfRepositoryTests.Complaints;

[Platform("Win")]
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
        // Arrange
        var spec = new ComplaintPublicSearchDto();
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints.Where(e => !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.ComplaintClosed })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task DateSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto
        {
            DateFrom = DateOnly.FromDateTime(_referenceItem.ReceivedDate.Date),
            DateTo = DateOnly.FromDateTime(_referenceItem.ReceivedDate.Date),
        };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.ReceivedDate == _referenceItem.ReceivedDate && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.ReceivedDate })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task NatureSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { Description = _referenceItem.ComplaintNature };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);
        var expected = ComplaintData.GetComplaints
            .Where(e => e.ComplaintNature == _referenceItem.ComplaintNature && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.SourceAddress })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task ConcernSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { Concern = _referenceItem.PrimaryConcern.Id };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.PrimaryConcern.Id == _referenceItem.PrimaryConcern.Id && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.PrimaryConcern })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task SourceNameSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { SourceName = _referenceItem.SourceFacilityName };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceFacilityName == _referenceItem.SourceFacilityName && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Arrange
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.SourceFacilityName })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task CountySpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { County = _referenceItem.ComplaintCounty };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.ComplaintCounty == _referenceItem.ComplaintCounty && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.ComplaintCounty })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task StreetSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { Street = _referenceItem.SourceAddress!.Street };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.Street == _referenceItem.SourceAddress.Street
                        && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.SourceAddress })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task Street2Spec_ReturnsFilteredList()
    {
        // Act

        // "Street" spec filter matches either Street OR Street2 from address
        var spec = new ComplaintPublicSearchDto { Street = _referenceItem.SourceAddress!.Street2 };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.Street2 == _referenceItem.SourceAddress.Street2
                        && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.SourceAddress })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task CitySpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { City = _referenceItem.SourceAddress!.City };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);
        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.City == _referenceItem.SourceAddress.City
                        && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.SourceAddress })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task StateSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { State = _referenceItem.SourceAddress!.State };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.State == _referenceItem.SourceAddress.State
                        && !e.IsDeleted).ToList();

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.SourceAddress })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task PostalCodeSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { PostalCode = _referenceItem.SourceAddress!.PostalCode };
        var predicate = ComplaintFilters.PublicSearchPredicate(spec);
        await using var repository = RepositoryHelper.CreateRepositoryHelper().GetComplaintRepository();

        var expected = ComplaintData.GetComplaints
            .Where(e => e.SourceAddress != null
                        && e.SourceAddress.PostalCode == _referenceItem.SourceAddress.PostalCode
                        && !e.IsDeleted).ToList();

        // Act
        var results = await repository.GetListAsync(predicate);

        // Act
        results.Should().HaveSameCount(expected);
        results.Select(e => new { e.Id, e.SourceAddress })
            .Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
    }
}
