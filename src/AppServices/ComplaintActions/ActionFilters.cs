using Cts.AppServices.ComplaintActions.Dto;
using Cts.Domain.Entities.ComplaintActions;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace Cts.AppServices.ComplaintActions;

internal static class ActionFilters
{
    public static Expression<Func<ComplaintAction, bool>> SearchPredicate(ActionSearchDto spec) =>
        PredicateBuilder.True<ComplaintAction>()
            .IsActionType(spec.ActionType)
            .ByDeletedStatus(spec.DeletedStatus)
            .FromDate(spec.DateFrom)
            .ToDate(spec.DateTo)
            .EnteredBy(spec.EnteredBy)
            .AssignedTo(spec.Office)
            .EnteredFromDate(spec.EnteredFrom)
            .EnteredToDate(spec.EnteredTo)
            .ContainsInvestigator(spec.Investigator)
            .ContainsComment(spec.Comments)
            .IsConcernType(spec.Concern);

    private static Expression<Func<ComplaintAction, bool>> IsActionType(
        this Expression<Func<ComplaintAction, bool>> predicate, Guid? input) =>
        input is null ? predicate : predicate.And(action => action.ActionType.Id == input);

    private static Expression<Func<ComplaintAction, bool>> ByDeletedStatus(
        this Expression<Func<ComplaintAction, bool>> predicate, SearchDeleteStatus? input) =>
        input switch
        {
            SearchDeleteStatus.All => predicate,
            SearchDeleteStatus.Deleted => predicate.And(action => action.IsDeleted || action.Complaint.IsDeleted),
            _ => predicate.And(action => !action.IsDeleted && !action.Complaint.IsDeleted),
        };

    private static Expression<Func<ComplaintAction, bool>> FromDate(
        this Expression<Func<ComplaintAction, bool>> predicate,
        DateOnly? input) =>
        input is null ? predicate : predicate.And(action => action.ActionDate >= input);

    private static Expression<Func<ComplaintAction, bool>> ToDate(
        this Expression<Func<ComplaintAction, bool>> predicate,
        DateOnly? input) =>
        input is null ? predicate : predicate.And(action => action.ActionDate <= input);

    private static Expression<Func<ComplaintAction, bool>> EnteredBy(
        this Expression<Func<ComplaintAction, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(action => action.EnteredBy != null && action.EnteredBy.Id == input);

    private static Expression<Func<ComplaintAction, bool>> AssignedTo(
        this Expression<Func<ComplaintAction, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(action => action.Complaint.CurrentOffice.Id == input);

    private static Expression<Func<ComplaintAction, bool>> EnteredFromDate(
        this Expression<Func<ComplaintAction, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(action =>
                action.EnteredDate.HasValue &&
                action.EnteredDate.Value.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<ComplaintAction, bool>> EnteredToDate(
        this Expression<Func<ComplaintAction, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(action =>
                action.EnteredDate.HasValue &&
                action.EnteredDate.Value.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<ComplaintAction, bool>> ContainsInvestigator(
        this Expression<Func<ComplaintAction, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(action => action.Investigator.Contains(input));

    private static Expression<Func<ComplaintAction, bool>> ContainsComment(
        this Expression<Func<ComplaintAction, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(action => action.Comments.Contains(input));

    private static Expression<Func<ComplaintAction, bool>> IsConcernType(
        this Expression<Func<ComplaintAction, bool>> predicate, Guid? input) =>
        input is null
            ? predicate
            : predicate.And(action =>
                action.Complaint.PrimaryConcern.Id == input ||
                (action.Complaint.SecondaryConcern != null && action.Complaint.SecondaryConcern.Id == input));
}
