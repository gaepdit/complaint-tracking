using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Cts.AppServices.Complaints.Permissions;

public class ComplaintOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private ComplaintOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<ComplaintOperation> AllOperations { get; } = [];

    public static readonly ComplaintOperation Accept = new(nameof(Accept));
    public static readonly ComplaintOperation Assign = new(nameof(Assign));
    public static readonly ComplaintOperation EditActions = new(nameof(EditActions));
    public static readonly ComplaintOperation EditAttachments = new(nameof(EditAttachments));
    public static readonly ComplaintOperation EditDetails = new(nameof(EditDetails));
    public static readonly ComplaintOperation ManageDeletions = new(nameof(ManageDeletions));
    public static readonly ComplaintOperation Reassign = new(nameof(Reassign));
    public static readonly ComplaintOperation Reopen = new(nameof(Reopen));
    public static readonly ComplaintOperation RequestReview = new(nameof(RequestReview));
    public static readonly ComplaintOperation Review = new(nameof(Review));
    public static readonly ComplaintOperation ViewAsOwner = new(nameof(ViewAsOwner));
    public static readonly ComplaintOperation ViewDeletedActions = new(nameof(ViewDeletedActions));
}
