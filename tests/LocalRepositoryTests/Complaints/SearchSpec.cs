using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.Complaints;

public class SearchSpec
{
    private LocalComplaintRepository _repository;

    [SetUp]
    public void SetUp() => _repository = new LocalComplaintRepository(Substitute.For<IAttachmentRepository>(),
        Substitute.For<IActionRepository>(), Substitute.For<IComplaintTransitionRepository>());

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task DefaultSpec_ReturnsAllNonDeleted()
    {
        var spec = new ComplaintSearchDto();
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: false });
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ClosedStatusSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintSearchDto { Status = SearchComplaintStatus.AllClosed };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: false, ComplaintClosed: true });
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task NotAcceptedSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintSearchDto { Status = SearchComplaintStatus.NotAccepted };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is
            { IsDeleted: false, ComplaintClosed: false, CurrentOwner: not null, CurrentOwnerAcceptedDate: null });
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DeletedSpec_ReturnsFilteredList()
    {
        var spec = new ComplaintSearchDto { DeletedStatus = SearchDeleteStatus.Deleted };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: true });
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task NeutralDeletedSpec_ReturnsAll()
    {
        var spec = new ComplaintSearchDto { DeletedStatus = SearchDeleteStatus.All };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items;
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ClosedDateSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First(e => e.ComplaintClosed);

        var spec = new ComplaintSearchDto
        {
            ClosedFrom = DateOnly.FromDateTime(referenceItem.ComplaintClosedDate!.Value.Date),
            ClosedTo = DateOnly.FromDateTime(referenceItem.ComplaintClosedDate!.Value.Date),
        };

        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items
            .Where(e => e is { IsDeleted: false, ComplaintClosed: true }
                        && e.ComplaintClosedDate == referenceItem.ComplaintClosedDate);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ReceivedDateSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();

        var spec = new ComplaintSearchDto
        {
            ReceivedFrom = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
            ReceivedTo = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
        };

        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: false }
                                                    && e.ReceivedDate == referenceItem.ReceivedDate);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ReceivedBySpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { ReceivedBy = referenceItem.ReceivedBy!.Id };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: false }
                                                    && e.ReceivedBy == referenceItem.ReceivedBy);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task CallerSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { CallerName = referenceItem.CallerName };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: false }
                                                    && e.CallerName == referenceItem.CallerName);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task RepresentsSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Represents = referenceItem.CallerRepresents };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.CallerRepresents == referenceItem.CallerRepresents);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task TextNatureSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Description = referenceItem.ComplaintNature };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.ComplaintNature == referenceItem.ComplaintNature);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task TextLocationSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Description = referenceItem.ComplaintLocation };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.ComplaintLocation == referenceItem.ComplaintLocation);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task TextDirectionSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Description = referenceItem.ComplaintDirections };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.ComplaintDirections == referenceItem.ComplaintDirections);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ComplaintCitySpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { ComplaintCity = referenceItem.ComplaintCity };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.ComplaintCity == referenceItem.ComplaintCity);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task CountySpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { County = referenceItem.ComplaintCounty };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.ComplaintCounty == referenceItem.ComplaintCounty);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ConcernSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Concern = referenceItem.PrimaryConcern.Id };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } &&
            (e.PrimaryConcern.Id == referenceItem.PrimaryConcern.Id ||
             (e.SecondaryConcern != null && e.SecondaryConcern.Id == referenceItem.PrimaryConcern.Id)));
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task SourceNameSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Source = referenceItem.SourceFacilityName };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.SourceFacilityName == referenceItem.SourceFacilityName);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task StreetSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Street = referenceItem.SourceAddress!.Street };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.SourceAddress != null &&
            e.SourceAddress.Street == referenceItem.SourceAddress.Street);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Street2Spec_ReturnsFilteredList()
    {
        // "Street" spec filter matches either Street OR Street2 from address
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Street = referenceItem.SourceAddress!.Street2 };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.SourceAddress != null &&
            e.SourceAddress.Street2 == referenceItem.SourceAddress.Street2);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task CitySpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { City = referenceItem.SourceAddress!.City };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.SourceAddress != null &&
            e.SourceAddress.City == referenceItem.SourceAddress.City);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task StateSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { State = referenceItem.SourceAddress!.State };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.SourceAddress != null &&
            e.SourceAddress.State == referenceItem.SourceAddress.State);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task PostalCodeSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { PostalCode = referenceItem.SourceAddress!.PostalCode };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e =>
            e is { IsDeleted: false } && e.SourceAddress != null &&
            e.SourceAddress.PostalCode == referenceItem.SourceAddress.PostalCode);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AssignedOfficeSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First();
        var spec = new ComplaintSearchDto { Office = referenceItem.CurrentOffice.Id };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: false }
                                                    && e.CurrentOffice == referenceItem.CurrentOffice);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AssignedAssociateSpec_ReturnsFilteredList()
    {
        var referenceItem = _repository.Items.First(e => e.CurrentOwner != null);
        var spec = new ComplaintSearchDto { Assigned = referenceItem.CurrentOwner!.Id };
        var predicate = ComplaintFilters.SearchPredicate(spec);

        var results = await _repository.GetListAsync(predicate);

        var expected = _repository.Items.Where(e => e is { IsDeleted: false }
                                                    && e.CurrentOwner == referenceItem.CurrentOwner);
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Attachments_Yes()
    {
        // Arrange
        var spec = new ComplaintSearchDto { Attachments = YesNoAny.Yes };
        var predicate = ComplaintFilters.SearchPredicate(spec);
        var expected = _repository.Items.Where(complaint =>
            !complaint.IsDeleted && complaint.Attachments.Exists(attachment => !attachment.IsDeleted));

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Attachments_No()
    {
        // Arrange
        var spec = new ComplaintSearchDto { Attachments = YesNoAny.No };
        var predicate = ComplaintFilters.SearchPredicate(spec);
        var expected = _repository.Items.Where(complaint =>
            !complaint.IsDeleted && !complaint.Attachments.Exists(attachment => !attachment.IsDeleted));

        // Act
        var results = await _repository.GetListAsync(predicate);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }
}
