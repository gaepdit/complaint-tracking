using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.TestData;

namespace AppServicesTests.Complaints;

public class PublicFilterTests
{
    [Test]
    public void DefaultSpec_ReturnsNotDeleted()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto();
        var expression = ComplaintFilters.PublicSearchPredicate(spec);
        var expected = ComplaintData.GetComplaints.Where(complaint => !complaint.IsDeleted);

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OpenStatus_ReturnsOpen()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { Status = PublicSearchStatus.Open };
        var expression = ComplaintFilters.PublicSearchPredicate(spec);
        var expected = ComplaintData.GetComplaints.Where(e => e is { IsDeleted: false, ComplaintClosed: false });

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedStatus_ReturnsClosed()
    {
        // Arrange
        var spec = new ComplaintPublicSearchDto { Status = PublicSearchStatus.Closed };
        var expression = ComplaintFilters.PublicSearchPredicate(spec);
        var expected = ComplaintData.GetComplaints.Where(e => e is { IsDeleted: false, ComplaintClosed: true });

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
