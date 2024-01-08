﻿using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.ComplaintActions;

public class ComplaintAction : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ComplaintAction() { }

    internal ComplaintAction(Guid id, Complaint complaint, ActionType actionType) : base(id)
    {
        Complaint = complaint;
        ActionType = actionType;
    }

    // Properties

    public Complaint Complaint { get; private init; } = default!;

    public DateTimeOffset ActionDate { get; init; }

    public ActionType ActionType { get; private set; } = default!;

    [StringLength(100)]
    public string? Investigator { get; init; }

    public DateTimeOffset EnteredDate { get; init; }

    public ApplicationUser EnteredBy { get; init; } = default!;

    public string? Comments { get; init; }
}
