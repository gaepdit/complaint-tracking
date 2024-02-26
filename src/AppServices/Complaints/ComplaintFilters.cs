using Cts.AppServices.Complaints.QueryDto;
using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Domain.Predicates;
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
            .ContainsNature(spec.Description)
            .IsConcernType(spec.Concern)
            .ContainsSourceName(spec.SourceName)
            .InCounty(spec.County)
            .ContainsStreet(spec.Street)
            .ContainsCity(spec.City)
            .ContainsState(spec.State)
            .ContainsPostalCode(spec.PostalCode);

    public static Expression<Func<Complaint, bool>> SearchPredicate(ComplaintSearchDto spec) =>
        PredicateBuilder.True<Complaint>()
            .ByStatus(spec.Status)
            .ByDeletedStatus(spec.DeletedStatus)
            .FromClosedDate(spec.ClosedFrom)
            .ToClosedDate(spec.ClosedTo)
            .FromDate(spec.ReceivedFrom)
            .ToDate(spec.ReceivedTo)
            .ReceivedBy(spec.ReceivedBy)
            .ContainsCaller(spec.CallerName)
            .ContainsRepresents(spec.Represents)
            .ContainsText(spec.Description)
            .ContainsComplaintCity(spec.ComplaintCity)
            .InCounty(spec.County)
            .IsConcernType(spec.Concern)
            .ContainsSourceName(spec.Source)
            .ContainsStreet(spec.Street)
            .ContainsCity(spec.City)
            .ContainsState(spec.State)
            .ContainsPostalCode(spec.PostalCode)
            .AssignedToOffice(spec.Office)
            .AssignedToAssociate(spec.Assigned);

    private static Expression<Func<Complaint, bool>> IsPublic(this Expression<Func<Complaint, bool>> predicate) =>
        predicate.IsClosed().ExcludeDeleted();

    private static Expression<Func<Complaint, bool>> IsClosed(this Expression<Func<Complaint, bool>> predicate) =>
        predicate.And(e => e.ComplaintClosed);

    private static Expression<Func<Complaint, bool>> ByStatus(this Expression<Func<Complaint, bool>> predicate,
        SearchComplaintStatus? input) => input switch
    {
        SearchComplaintStatus.Closed =>
            predicate.And(e => e.Status == ComplaintStatus.Closed),
        SearchComplaintStatus.AdministrativelyClosed =>
            predicate.And(e => e.Status == ComplaintStatus.AdministrativelyClosed),
        SearchComplaintStatus.AllClosed =>
            predicate.And(e => e.ComplaintClosed),
        SearchComplaintStatus.New =>
            predicate.And(e => e.Status == ComplaintStatus.New),
        SearchComplaintStatus.ReviewPending =>
            predicate.And(e => e.Status == ComplaintStatus.ReviewPending),
        SearchComplaintStatus.UnderInvestigation =>
            predicate.And(e => e.Status == ComplaintStatus.UnderInvestigation),
        SearchComplaintStatus.AllOpen =>
            predicate.And(e => !e.ComplaintClosed),
        _ => predicate,
    };

    private static Expression<Func<Complaint, bool>> ByDeletedStatus(this Expression<Func<Complaint, bool>> predicate,
        SearchDeleteStatus? input) => input switch
    {
        SearchDeleteStatus.All => predicate,
        SearchDeleteStatus.Deleted => predicate.And(e => e.IsDeleted),
        _ => predicate.And(e => !e.IsDeleted),
    };

    private static Expression<Func<Complaint, bool>> FromDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.ReceivedDate.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> ToDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.ReceivedDate.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> FromClosedDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e =>
                e.ComplaintClosed && e.ComplaintClosedDate != null &&
                e.ComplaintClosedDate.Value.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> ToClosedDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e =>
                e.ComplaintClosed && e.ComplaintClosedDate != null &&
                e.ComplaintClosedDate.Value.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> ReceivedBy(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(e => e.ReceivedBy.Id == input);

    private static Expression<Func<Complaint, bool>> ContainsCaller(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => e.CallerName != null && e.CallerName.Contains(input));

    private static Expression<Func<Complaint, bool>> ContainsRepresents(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => e.CallerRepresents != null && e.CallerRepresents.Contains(input));

    private static Expression<Func<Complaint, bool>> ContainsNature(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => e.ComplaintNature != null && e.ComplaintNature.Contains(input));

    private static Expression<Func<Complaint, bool>> ContainsText(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => (e.ComplaintNature != null && e.ComplaintNature.Contains(input))
                || (e.ComplaintLocation != null && e.ComplaintLocation.Contains(input))
                || (e.ComplaintDirections != null && e.ComplaintDirections.Contains(input)));

    private static Expression<Func<Complaint, bool>> ContainsComplaintCity(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => e.ComplaintCity != null && e.ComplaintCity.Contains(input));

    private static Expression<Func<Complaint, bool>> IsConcernType(this Expression<Func<Complaint, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(e =>
                e.PrimaryConcern.Id == input || (e.SecondaryConcern != null && e.SecondaryConcern.Id == input));

    private static Expression<Func<Complaint, bool>> ContainsSourceName(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => e.SourceFacilityName != null && e.SourceFacilityName.Contains(input));

    private static Expression<Func<Complaint, bool>> InCounty(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(e => e.ComplaintCounty == input);

    private static Expression<Func<Complaint, bool>> ContainsStreet(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => e.SourceAddress != null
                && ((e.SourceAddress.Street != null && e.SourceAddress.Street.Contains(input))
                    || (e.SourceAddress.Street2 != null && e.SourceAddress.Street2.Contains(input))));

    private static Expression<Func<Complaint, bool>> ContainsCity(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e =>
                e.SourceAddress != null && e.SourceAddress.City != null && e.SourceAddress.City.Contains(input));

    private static Expression<Func<Complaint, bool>> ContainsState(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e =>
                e.SourceAddress != null && e.SourceAddress.State != null && e.SourceAddress.State.Contains(input));

    private static Expression<Func<Complaint, bool>> ContainsPostalCode(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(e => e.SourceAddress != null
                && e.SourceAddress.PostalCode != null && e.SourceAddress.PostalCode.Contains(input));

    private static Expression<Func<Complaint, bool>> AssignedToOffice(this Expression<Func<Complaint, bool>> predicate,
        Guid? input) =>
        input is null ? predicate : predicate.And(e => e.CurrentOffice.Id == input);

    private static Expression<Func<Complaint, bool>> AssignedToAssociate(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.CurrentOwner != null && e.CurrentOwner.Id == input);
}
