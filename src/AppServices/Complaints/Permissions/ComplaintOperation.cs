using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Cts.AppServices.Complaints.Permissions;

public static class ComplaintOperationNames
{
    public const string ManageDeletions = nameof(ManageDeletions);
    public const string ViewAsOwner = nameof(ViewAsOwner);
    public const string Accept = nameof(Accept);
    public const string Edit = nameof(Edit);
    public const string EditAsRecentReporter = nameof(EditAsRecentReporter);
    public const string Review = nameof(Review);
    public const string Assign = nameof(Assign);
    public const string RequestReview = nameof(RequestReview);
    public const string Reassign = nameof(Reassign);
    public const string Reopen = nameof(Reopen);
    public const string EditAttachments = nameof(EditAttachments);
}

public class ComplaintOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private ComplaintOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<ComplaintOperation> AllOperations { get; } = new();

    public static readonly ComplaintOperation ManageDeletions = new(ComplaintOperationNames.ManageDeletions);
    public static readonly ComplaintOperation ViewAsOwner = new(ComplaintOperationNames.ViewAsOwner);
    public static readonly ComplaintOperation Accept = new(ComplaintOperationNames.Accept);
    public static readonly ComplaintOperation Edit = new(ComplaintOperationNames.Edit);
    public static readonly ComplaintOperation EditAsRecentReporter = new(ComplaintOperationNames.EditAsRecentReporter);
    public static readonly ComplaintOperation Review = new(ComplaintOperationNames.Review);
    public static readonly ComplaintOperation Assign = new(ComplaintOperationNames.Assign);
    public static readonly ComplaintOperation RequestReview = new(ComplaintOperationNames.RequestReview);
    public static readonly ComplaintOperation Reassign = new(ComplaintOperationNames.Reassign);
    public static readonly ComplaintOperation Reopen = new(ComplaintOperationNames.Reopen);
    public static readonly ComplaintOperation EditAttachments = new(ComplaintOperationNames.EditAttachments);
}
