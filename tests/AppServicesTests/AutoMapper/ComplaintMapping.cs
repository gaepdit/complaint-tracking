using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Complaints;

namespace AppServicesTests.AutoMapper;

public class ComplaintMapping
{
    [Test]
    public void CreateComplaintMappingCalculatesCorrectDateTimeOffset()
    {
        var item = new ComplaintCreateDto()
        {
            ReceivedDate = new DateTime(2000, 1, 1),
            TimeReceived = new DateTime(2020, 2, 2, 1, 15, 0),
        };
        item.ReceivedDate = DateTime.SpecifyKind(item.ReceivedDate, DateTimeKind.Local);

        var result = AppServicesTestsGlobal.Mapper!.Map<Complaint>(item);

        var correct = new DateTime(2000, 1, 1, 1, 15, 0);
        var expected = DateTime.SpecifyKind(correct, DateTimeKind.Local);
        result.ReceivedDate.Should().Be(expected);
    }
}
