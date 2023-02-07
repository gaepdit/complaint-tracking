using Cts.Domain.Attachments;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace Cts.AppServices.Attachments;

internal static class AttachmentFilters
{
    public static Expression<Func<Attachment, bool>> PublicIdPredicate(int id) =>
        PredicateBuilder.True<Attachment>().ForComplaint(id).IsPublic();

    private static Expression<Func<Attachment, bool>> IsPublic(this Expression<Func<Attachment, bool>> predicate) =>
        predicate.ExcludeDeleted();

    private static Expression<Func<Attachment, bool>> ForComplaint(this Expression<Func<Attachment, bool>> predicate,
        int complaintId) =>
        predicate.And(e => e.ComplaintId == complaintId);
}
