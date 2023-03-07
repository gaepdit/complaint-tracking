﻿using Cts.Domain.ActionTypes;
using Cts.Domain.Complaints;
using Cts.Domain.Identity;
using JetBrains.Annotations;

namespace Cts.Domain.ComplaintActions;

public class ComplaintAction : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ComplaintAction() { }

    internal ComplaintAction(Guid id) : base(id) { }

    // Properties

    public Complaint Complaint { get; init; } = default!;

    public DateTimeOffset ActionDate { get; init; }

    public ActionType ActionType { get; init; } = default!;

    [StringLength(100)]
    public string? Investigator { get; init; }

    public DateTimeOffset DateEntered { get; init; }

    public ApplicationUser EnteredBy { get; init; } = default!;

    public string? Comments { get; init; }
}
