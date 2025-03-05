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
            .ByPublicStatus(spec.Status)
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
            .HasAttachments(spec.Attachments)
            .FromDate(spec.ReceivedFrom)
            .ToDate(spec.ReceivedTo)
            .ReceivedBy(spec.ReceivedBy)
            .ContainsCaller(spec.CallerName)
            .ContainsRepresents(spec.Represents)
            .ContainsDetails(spec.Description)
            .ContainsComplaintCity(spec.ComplaintCity)
            .InCounty(spec.County)
            .IsConcernType(spec.Concern)
            .ContainsSourceName(spec.Source)
            .ContainsFacilityId(spec.FacilityIdNumber)
            .ContainsSourceContact(spec.Contact)
            .ContainsStreet(spec.Street)
            .ContainsCity(spec.City)
            .ContainsState(spec.State)
            .ContainsPostalCode(spec.PostalCode)
            .AssignedToOffice(spec.Office)
            .AssignedToAssociate(spec.Assigned, spec.OnlyUnassigned)
            .ReviewRequestedFrom(spec.Reviewer)
            .OnlyUnassigned(spec.OnlyUnassigned)
            .ContainsAnyText(spec.Text);

    private static Expression<Func<Complaint, bool>> IsPublic(this Expression<Func<Complaint, bool>> predicate) =>
        predicate.ExcludeDeleted();

    // Comprises both "Closed" and "AdministrativelyClosed" statuses.
    private static Expression<Func<Complaint, bool>> IsClosed(this Expression<Func<Complaint, bool>> predicate) =>
        predicate.And(complaint => complaint.ComplaintClosed);

    private static Expression<Func<Complaint, bool>> IsOpen(this Expression<Func<Complaint, bool>> predicate) =>
        predicate.And(complaint => !complaint.ComplaintClosed);

    private static Expression<Func<Complaint, bool>> ByPublicStatus(this Expression<Func<Complaint, bool>> predicate,
        PublicSearchStatus? input) => input switch
    {
        PublicSearchStatus.Open => predicate.IsOpen(),
        PublicSearchStatus.Closed => predicate.IsClosed(),
        _ => predicate,
    };

    private static Expression<Func<Complaint, bool>> ByStatus(this Expression<Func<Complaint, bool>> predicate,
        SearchComplaintStatus? input) => input switch
    {
        SearchComplaintStatus.AllOpen => predicate.IsOpen(),
        SearchComplaintStatus.AllClosed => predicate.IsClosed(),
        SearchComplaintStatus.New => predicate.And(complaint => complaint.Status == ComplaintStatus.New),
        SearchComplaintStatus.UnderInvestigation => predicate.And(complaint =>
            complaint.Status == ComplaintStatus.UnderInvestigation),
        SearchComplaintStatus.ReviewPending => predicate.And(complaint =>
            complaint.Status == ComplaintStatus.ReviewPending),
        SearchComplaintStatus.NotAccepted => predicate.IsOpen().And(complaint =>
            complaint.CurrentOwner != null && complaint.CurrentOwnerAcceptedDate == null),
        SearchComplaintStatus.NotAssigned => predicate.IsOpen().And(complaint => complaint.CurrentOwner == null),
        SearchComplaintStatus.Closed => predicate.And(complaint => complaint.Status == ComplaintStatus.Closed),
        SearchComplaintStatus.AdministrativelyClosed => predicate.And(complaint =>
            complaint.Status == ComplaintStatus.AdministrativelyClosed),
        _ => predicate,
    };

    private static Expression<Func<Complaint, bool>> ByDeletedStatus(this Expression<Func<Complaint, bool>> predicate,
        SearchDeleteStatus? input) => input switch
    {
        SearchDeleteStatus.All => predicate,
        SearchDeleteStatus.Deleted => predicate.And(complaint => complaint.IsDeleted),
        _ => predicate.And(complaint => !complaint.IsDeleted),
    };

    private static Expression<Func<Complaint, bool>> FromDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(complaint => complaint.ReceivedDate.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> ToDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(complaint => complaint.ReceivedDate.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> FromClosedDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(complaint =>
                complaint.ComplaintClosed &&
                complaint.ComplaintClosedDate != null &&
                complaint.ComplaintClosedDate.Value.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> ToClosedDate(this Expression<Func<Complaint, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(complaint =>
                complaint.ComplaintClosed &&
                complaint.ComplaintClosedDate != null &&
                complaint.ComplaintClosedDate.Value.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Complaint, bool>> HasAttachments(
        this Expression<Func<Complaint, bool>> predicate,
        YesNoAny? input) => input switch
    {
        YesNoAny.Yes => predicate.And(complaint => complaint.Attachments.Any(attachment => !attachment.IsDeleted)),
#pragma warning disable S6603
        // 'The collection-specific "TrueForAll" method should be used instead of the "All" extension'
        // "TrueForAll" is incompatible with EF Core.
        YesNoAny.No => predicate.And(complaint => complaint.Attachments.All(attachment => attachment.IsDeleted)),
#pragma warning restore S6603
        _ => predicate,
    };

    private static Expression<Func<Complaint, bool>> ReceivedBy(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(complaint => complaint.ReceivedBy != null && complaint.ReceivedBy.Id == input);

    private static Expression<Func<Complaint, bool>> ContainsCaller(this Expression<Func<Complaint, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(CallerNameExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsRepresents(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(CallerRepresentsExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsNature(this Expression<Func<Complaint, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(NatureExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsDetails(this Expression<Func<Complaint, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(DetailsExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsComplaintCity(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(ComplaintCityExpr(input));

    private static Expression<Func<Complaint, bool>> IsConcernType(this Expression<Func<Complaint, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(complaint =>
                complaint.PrimaryConcern.Id == input ||
                (complaint.SecondaryConcern != null && complaint.SecondaryConcern.Id == input));

    private static Expression<Func<Complaint, bool>> ContainsSourceName(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(FacilityNameExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsFacilityId(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(FacilityIdExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsSourceContact(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(ContactNameExpr(input));

    private static Expression<Func<Complaint, bool>> InCounty(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(complaint => complaint.ComplaintCounty == input);

    private static Expression<Func<Complaint, bool>> ContainsStreet(this Expression<Func<Complaint, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(SourceStreetExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsCity(this Expression<Func<Complaint, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(SourceCityExpr(input));

    private static Expression<Func<Complaint, bool>> ContainsState(this Expression<Func<Complaint, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(complaint =>
                complaint.SourceAddress != null && complaint.SourceAddress.State != null &&
                complaint.SourceAddress.State.Contains(input));

    private static Expression<Func<Complaint, bool>> ContainsPostalCode(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(SourcePostalCodeExpr(input));

    private static Expression<Func<Complaint, bool>> AssignedToOffice(this Expression<Func<Complaint, bool>> predicate,
        Guid? input) =>
        input is null ? predicate : predicate.And(complaint => complaint.CurrentOffice.Id == input);

    private static Expression<Func<Complaint, bool>> AssignedToAssociate(
        this Expression<Func<Complaint, bool>> predicate, string? input, bool onlyUnassigned) =>
        input is null || onlyUnassigned
            ? predicate
            : predicate.And(complaint => complaint.CurrentOwner != null && complaint.CurrentOwner.Id == input);

    private static Expression<Func<Complaint, bool>> ReviewRequestedFrom(
        this Expression<Func<Complaint, bool>> predicate, string? input) =>
        input is null
            ? predicate
            : predicate.And(complaint => complaint.ReviewedBy != null && complaint.ReviewedBy.Id == input);

    private static Expression<Func<Complaint, bool>> OnlyUnassigned(this Expression<Func<Complaint, bool>> predicate,
        bool onlyUnassigned) =>
        onlyUnassigned ? predicate.And(complaint => complaint.CurrentOwner == null) : predicate;

    private static Expression<Func<Complaint, bool>> ContainsAnyText(this Expression<Func<Complaint, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input)
        ? predicate
        : input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Aggregate(predicate, (current, term) =>
            current.And(CallerNameExpr(term).Or(CallerRepresentsExpr(term)).Or(NatureExpr(term)).Or(DetailsExpr(term))
                .Or(ComplaintCityExpr(term)).Or(FacilityNameExpr(term)).Or(FacilityIdExpr(term))
                .Or(ContactNameExpr(term)).Or(CountyExpr(term)).Or(SourceStreetExpr(term)).Or(SourceCityExpr(term))
                .Or(SourcePostalCodeExpr(term)).Or(CallerStreetExpr(term)).Or(CallerCityExpr(term))
                .Or(CallerPostalCodeExpr(term))));

    // Text search expressions

    private static Expression<Func<Complaint, bool>> CallerNameExpr(string input) =>
        complaint => complaint.CallerName != null && complaint.CallerName.Contains(input);

    private static Expression<Func<Complaint, bool>> CallerRepresentsExpr(string input) =>
        complaint => complaint.CallerRepresents != null && complaint.CallerRepresents.Contains(input);

    private static Expression<Func<Complaint, bool>> NatureExpr(string input) =>
        complaint => complaint.ComplaintNature != null && complaint.ComplaintNature.Contains(input);

    private static Expression<Func<Complaint, bool>> DetailsExpr(string input) =>
        complaint =>
            (complaint.ComplaintNature != null && complaint.ComplaintNature.Contains(input)) ||
            (complaint.ComplaintLocation != null && complaint.ComplaintLocation.Contains(input)) ||
            (complaint.ComplaintDirections != null && complaint.ComplaintDirections.Contains(input));

    private static Expression<Func<Complaint, bool>> ComplaintCityExpr(string input) => complaint =>
        complaint.ComplaintCity != null && complaint.ComplaintCity.Contains(input);

    private static Expression<Func<Complaint, bool>> FacilityNameExpr(string input) =>
        complaint => complaint.SourceFacilityName != null && complaint.SourceFacilityName.Contains(input);

    private static Expression<Func<Complaint, bool>> FacilityIdExpr(string input) =>
        complaint => complaint.SourceFacilityIdNumber != null && complaint.SourceFacilityIdNumber.Contains(input);

    private static Expression<Func<Complaint, bool>> ContactNameExpr(string input) =>
        complaint => complaint.SourceContactName != null && complaint.SourceContactName.Contains(input);

    private static Expression<Func<Complaint, bool>> CountyExpr(string input) =>
        complaint => complaint.ComplaintCounty != null && complaint.ComplaintCounty.Contains(input);

    private static Expression<Func<Complaint, bool>> SourceStreetExpr(string input) => complaint =>
        complaint.SourceAddress != null &&
        ((complaint.SourceAddress.Street != null && complaint.SourceAddress.Street.Contains(input)) ||
         (complaint.SourceAddress.Street2 != null && complaint.SourceAddress.Street2.Contains(input)));

    private static Expression<Func<Complaint, bool>> SourceCityExpr(string input) => complaint =>
        complaint.SourceAddress != null && complaint.SourceAddress.City != null &&
        complaint.SourceAddress.City.Contains(input);

    private static Expression<Func<Complaint, bool>> SourcePostalCodeExpr(string input) => complaint =>
        complaint.SourceAddress != null && complaint.SourceAddress.PostalCode != null &&
        complaint.SourceAddress.PostalCode.Contains(input);

    private static Expression<Func<Complaint, bool>> CallerStreetExpr(string input) => complaint =>
        complaint.CallerAddress != null &&
        ((complaint.CallerAddress.Street != null && complaint.CallerAddress.Street.Contains(input)) ||
         (complaint.CallerAddress.Street2 != null && complaint.CallerAddress.Street2.Contains(input)));

    private static Expression<Func<Complaint, bool>> CallerCityExpr(string input) => complaint =>
        complaint.CallerAddress != null && complaint.CallerAddress.City != null &&
        complaint.CallerAddress.City.Contains(input);

    private static Expression<Func<Complaint, bool>> CallerPostalCodeExpr(string input) => complaint =>
        complaint.CallerAddress != null && complaint.CallerAddress.PostalCode != null &&
        complaint.CallerAddress.PostalCode.Contains(input);
}
