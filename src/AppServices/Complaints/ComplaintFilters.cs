using Cts.AppServices.Complaints.Dto;
using Cts.Domain.AppLibraryExtra;
using Cts.Domain.Complaints;
using System.Linq.Expressions;

namespace Cts.AppServices.Complaints;

internal static class ComplaintFilters
{
    public static Expression<Func<Complaint, bool>> PublicIdPredicate(int id) =>
        PredicateBuilder.True<Complaint>().WithId(id).IsPublic();

    public static Expression<Func<Complaint, bool>> PublicSearchPredicate(ComplaintPublicSearchDto spec) =>
        PredicateBuilder.True<Complaint>()
            .IsPublic()
            .FromDate(spec.DateFrom)
            .ToDate(spec.DateTo)
            .ContainsNature(spec.Nature)
            .IsConcernType(spec.Type)
            .ContainsSourceName(spec.SourceName)
            .InCounty(spec.County)
            .ContainsStreet(spec.Street)
            .ContainsCity(spec.City)
            .ContainsState(spec.State)
            .ContainsPostalCode(spec.PostalCode);

    private static Expression<Func<Complaint, bool>> IsClosed(this Expression<Func<Complaint, bool>> predicate) =>
        predicate.And(e => e.ComplaintClosed);

    public static Expression<Func<Complaint, bool>> IsPublic(this Expression<Func<Complaint, bool>> predicate) =>
        predicate.IsClosed().ExcludeDeleted();

    // The inverted comparison returns true if the parameter is null.
    // Since a null dateFrom value indicates no date filtering is desired, all entities pass this filter.
    private static Expression<Func<Complaint, bool>> FromDate(this Expression<Func<Complaint, bool>> predicate,
        DateTime? input) =>
        predicate.And(e => input == null || e.DateReceived >= input);

    // The inverted comparison returns true if the parameter is null.
    // Since a null dateFrom value indicates no date filtering is desired, all entities pass this filter.
    private static Expression<Func<Complaint, bool>> ToDate(this Expression<Func<Complaint, bool>> predicate,
        DateTime? input) =>
        predicate.And(e => input == null || e.DateReceived <= input);

    private static Expression<Func<Complaint, bool>> ContainsNature(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        predicate.And(e => string.IsNullOrWhiteSpace(input)
            || (e.ComplaintNature != null && e.ComplaintNature.Contains(input)));

    private static Expression<Func<Complaint, bool>> IsConcernType(this Expression<Func<Complaint, bool>> predicate,
        Guid? input) =>
        predicate.And(e => input == null
            || e.PrimaryConcern.Id == input
            || (e.SecondaryConcern != null && e.SecondaryConcern.Id == input));

    private static Expression<Func<Complaint, bool>> ContainsSourceName(
        this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        predicate.And(e => string.IsNullOrWhiteSpace(input)
            || (e.SourceFacilityName != null && e.SourceFacilityName.Contains(input)));

    private static Expression<Func<Complaint, bool>> InCounty(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        predicate.And(e => string.IsNullOrWhiteSpace(input)
            || e.ComplaintCounty == input);

    private static Expression<Func<Complaint, bool>> ContainsStreet(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        predicate.And(e => string.IsNullOrWhiteSpace(input)
            || (e.SourceAddress != null
                && (e.SourceAddress.Street.Contains(input)
                    || (e.SourceAddress.Street2 != null && e.SourceAddress.Street2.Contains(input)))));

    private static Expression<Func<Complaint, bool>> ContainsCity(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        predicate.And(e => string.IsNullOrWhiteSpace(input)
            || (e.SourceAddress != null && e.SourceAddress.City.Contains(input)));

    private static Expression<Func<Complaint, bool>> ContainsState(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        predicate.And(e => string.IsNullOrWhiteSpace(input)
            || (e.SourceAddress != null && e.SourceAddress.State.Contains(input)));

    private static Expression<Func<Complaint, bool>> ContainsPostalCode(
        this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        predicate.And(e => string.IsNullOrWhiteSpace(input)
            || (e.SourceAddress != null && e.SourceAddress.PostalCode.Contains(input)));
}
