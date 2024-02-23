﻿using Cts.Domain.Entities.ComplaintActions;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace Cts.AppServices.ComplaintActions;

internal static class ComplaintActionFilters
{
    public static Expression<Func<ComplaintAction, bool>> SearchPredicate(ComplaintActionSearchDto spec) =>
        PredicateBuilder.True<ComplaintAction>()
            .IsActionType(spec.ActionType)
            .ByComplaintDeletedStatus(spec.DeletedStatus)
            .ByDeletedStatus(spec.DeletedStatus)
            .FromDate(spec.DateFrom)
            .ToDate(spec.DateTo)
            .EnteredBy(spec.EnteredBy)
            .EnteredFromDate(spec.EnteredFrom)
            .EnteredToDate(spec.EnteredTo)
            .ContainsInvestigator(spec.Investigator)
            .ContainsComment(spec.Comments)
            .IsConcernType(spec.Concern);

    private static Expression<Func<ComplaintAction, bool>> IsActionType(
        this Expression<Func<ComplaintAction, bool>> predicate, Guid? input) =>
        input is null ? predicate : predicate.And(action => action.ActionType.Id == input);

    private static Expression<Func<ComplaintAction, bool>> ByComplaintDeletedStatus(
        this Expression<Func<ComplaintAction, bool>> predicate, SearchDeleteStatus? input) =>
        input switch
        {
            SearchDeleteStatus.All => predicate,
            SearchDeleteStatus.Deleted => predicate,
            _ => predicate.And(action => !action.Complaint.IsDeleted),
        };

    private static Expression<Func<ComplaintAction, bool>> ByDeletedStatus(
        this Expression<Func<ComplaintAction, bool>> predicate, SearchDeleteStatus? input) =>
        input switch
        {
            SearchDeleteStatus.All => predicate,
            SearchDeleteStatus.Deleted => predicate.And(action => action.IsDeleted),
            _ => predicate.And(action => !action.IsDeleted),
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

    private static Expression<Func<ComplaintAction, bool>> EnteredFromDate(
        this Expression<Func<ComplaintAction, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(action => action.EnteredDate.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<ComplaintAction, bool>> EnteredToDate(
        this Expression<Func<ComplaintAction, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(action => action.EnteredDate.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

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
