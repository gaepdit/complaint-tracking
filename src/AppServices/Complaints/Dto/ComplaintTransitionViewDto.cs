﻿using Cts.AppServices.Offices;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Entities.ComplaintTransitions;

namespace Cts.AppServices.Complaints.Dto;

public class ComplaintTransitionViewDto
{
    public TransitionType TransitionType { get; init; }
    public DateTimeOffset CommittedDate { get; init; }
    public StaffViewDto? CommittedByUser { get; init; }
    public StaffViewDto? TransferredFromUser { get; init; }
    public OfficeViewDto? TransferredFromOffice { get; init; }
    public StaffViewDto? TransferredToUser { get; init; }
    public OfficeViewDto? TransferredToOffice { get; init; }
    public string? Comment { get; init; }
}
