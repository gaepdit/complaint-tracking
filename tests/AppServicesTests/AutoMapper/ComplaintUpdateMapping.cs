using Cts.AppServices.Complaints.CommandDto;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class ComplaintUpdateMapping
{
    [Test]
    public void ComplaintUpdateMapping_ReturnsCorrectReceivedDateAndTime()
    {
        var item = new Complaint(0)
        {
            ReceivedDate = new DateTimeOffset(2000, 01, 01, 12, 30, 15, TimeSpan.Zero).ToLocalTime(),
        };

        var result = AppServicesTestsSetup.Mapper!.Map<ComplaintUpdateDto>(item);

        using var scope = new AssertionScope();
        result.ReceivedDate.Should().Be(DateOnly.FromDateTime(item.ReceivedDate.Date));
        result.ReceivedTime.Should().Be(TimeOnly.FromTimeSpan(item.ReceivedDate.TimeOfDay));
    }

    [Test]
    public void ComplaintUpdateMapping_IncludesCorrectAuthHandlerProperties()
    {
        // Arrange
        var expectedCurrentOwnerId = Guid.NewGuid().ToString();
        var expectedCurrentOfficeId = Guid.NewGuid();
        var expectedEnteredById = Guid.NewGuid().ToString();
        var expectedEnteredDate = new DateTimeOffset(2000, 01, 01, 12, 30, 15, TimeSpan.Zero).ToLocalTime();
        var expectedCurrentOwnerAcceptedDate =
            new DateTimeOffset(2000, 02, 02, 12, 30, 15, TimeSpan.Zero).ToLocalTime();

        var item = new Complaint(0)
        {
            ComplaintClosed = true,
            CurrentOwner = new ApplicationUser { Id = expectedCurrentOwnerId },
            CurrentOffice = new Office(expectedCurrentOfficeId, TextData.ValidName),
            EnteredBy = new ApplicationUser { Id = expectedEnteredById },
            EnteredDate = expectedEnteredDate,
            CurrentOwnerAcceptedDate = expectedCurrentOwnerAcceptedDate,
            Status = ComplaintStatus.Closed,
        };

        item.SetDeleted(null);

        // Act
        var result = AppServicesTestsSetup.Mapper!.Map<ComplaintUpdateDto>(item);

        // Assert
        using var scope = new AssertionScope();
        result.ComplaintClosed.Should().BeTrue();
        result.IsDeleted.Should().BeTrue();
        result.CurrentOwnerId.Should().Be(expectedCurrentOwnerId);
        result.CurrentOfficeId.Should().Be(expectedCurrentOfficeId);
        result.EnteredById.Should().Be(expectedEnteredById);
        result.EnteredDate.Should().Be(expectedEnteredDate);
        result.CurrentOwnerAcceptedDate.Should().Be(expectedCurrentOwnerAcceptedDate);
        result.Status.Should().Be(ComplaintStatus.Closed);
    }
}
