using Cts.Domain.Entities.Attachments;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace Cts.AppServices.Attachments;

internal static class AttachmentFilters
{
    public static Expression<Func<Attachment, bool>> IdPredicate(Guid id) =>
        PredicateBuilder.True<Attachment>().WithId(id);

    public static Expression<Func<Attachment, bool>> PublicIdPredicate(Guid id) =>
        PredicateBuilder.True<Attachment>().WithId(id).IsPublic();

    private static Expression<Func<Attachment, bool>> IsPublic(
        this Expression<Func<Attachment, bool>> predicate) =>
        predicate.ExcludeDeleted().ComplaintIsPublic();

    private static Expression<Func<Attachment, bool>> ComplaintIsPublic(
        this Expression<Func<Attachment, bool>> predicate) =>
        predicate.And(attachment => attachment.Complaint.ComplaintClosed && !attachment.Complaint.IsDeleted);
}
