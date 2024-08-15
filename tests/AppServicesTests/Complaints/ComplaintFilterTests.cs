using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.TestData;

namespace AppServicesTests.Complaints;

public class ComplaintFilterTests
{
    [Test]
    public void DefaultSpec_ReturnsNotDeleted()
    {
        // Arrange
        var spec = new ComplaintSearchDto();
        var expression = ComplaintFilters.SearchPredicate(spec);
        var expected = ComplaintData.GetComplaints.Where(complaint => !complaint.IsDeleted);

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_All_ReturnsAll()
    {
        // Arrange
        var spec = new ComplaintSearchDto { DeletedStatus = SearchDeleteStatus.All };
        var expression = ComplaintFilters.SearchPredicate(spec);
        var expected = ComplaintData.GetComplaints;

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_Deleted()
    {
        // Arrange
        var spec = new ComplaintSearchDto { DeletedStatus = SearchDeleteStatus.Deleted };
        var expression = ComplaintFilters.SearchPredicate(spec);
        var expected = ComplaintData.GetComplaints.Where(complaint => complaint.IsDeleted);

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ResponsibleStaff_Match()
    {
        // Arrange
        var responsibleStaffId = ComplaintData.GetComplaints.First().CurrentOwner!.Id;
        var spec = new ComplaintSearchDto { Assigned = responsibleStaffId };
        var expression = ComplaintFilters.SearchPredicate(spec);

        var expected = ComplaintData.GetComplaints.Where(complaint =>
            complaint.CurrentOwner != null && complaint.CurrentOwner.Id == responsibleStaffId);

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ResponsibleStaff_NoMatch()
    {
        // Arrange
        var spec = new ComplaintSearchDto { Assigned = Guid.Empty.ToString() };
        var expression = ComplaintFilters.SearchPredicate(spec);

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Office_Match()
    {
        // Arrange
        var officeId = ComplaintData.GetComplaints
            .First().CurrentOffice.Id;
        var spec = new ComplaintSearchDto { Office = officeId };
        var expression = ComplaintFilters.SearchPredicate(spec);

        var expected = ComplaintData.GetComplaints.Where(complaint =>
            !complaint.IsDeleted && complaint.CurrentOffice.Id == officeId);

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Office_NoMatch()
    {
        // Arrange
        var spec = new ComplaintSearchDto { Office = Guid.Empty };
        var expression = ComplaintFilters.SearchPredicate(spec);

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Attachments_Yes()
    {
        // Arrange
        var spec = new ComplaintSearchDto { Attachments = YesNoAny.Yes };
        var expression = ComplaintFilters.SearchPredicate(spec);

        var expected = ComplaintData.GetComplaints.Where(complaint =>
            !complaint.IsDeleted && complaint.Attachments.Exists(attachment => !attachment.IsDeleted));

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Attachments_No()
    {
        // Arrange
        var spec = new ComplaintSearchDto { Attachments = YesNoAny.No };
        var expression = ComplaintFilters.SearchPredicate(spec);

        var expected = ComplaintData.GetComplaints.Where(complaint =>
            !complaint.IsDeleted && !complaint.Attachments.Exists(attachment => !attachment.IsDeleted));

        // Act
        var result = ComplaintData.GetComplaints.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
