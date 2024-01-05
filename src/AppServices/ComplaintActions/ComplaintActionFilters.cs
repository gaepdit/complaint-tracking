using Cts.Domain.Entities.ComplaintActions;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace Cts.AppServices.ComplaintActions;

internal static class ComplaintActionFilters
{
    public static Expression<Func<ComplaintAction, bool>> IdPredicate(int id) =>
        PredicateBuilder.True<ComplaintAction>().ForComplaint(id);

    public static Expression<Func<ComplaintAction, bool>> PublicIdPredicate(int id) =>
        PredicateBuilder.True<ComplaintAction>().ForComplaint(id).IsPublic();

    private static Expression<Func<ComplaintAction, bool>> IsPublic(
        this Expression<Func<ComplaintAction, bool>> predicate) =>
        predicate.ExcludeDeleted();

    private static Expression<Func<ComplaintAction, bool>> ForComplaint(
        this Expression<Func<ComplaintAction, bool>> predicate, int complaintId) =>
        predicate.And(action => action.Complaint.Id == complaintId);
}
