using ClosedXML.Attributes;
using ComplaintTracking.Models;

namespace ComplaintTracking.ViewModels
{
    public class SearchResultsExportViewModel
    {
        public SearchResultsExportViewModel(Complaint e)
        {
            ComplaintId = e.Id;
            DateReceived = e.DateReceived;
            ReceivedByName = e.ReceivedBy?.SortableFullName;
            SourceFacilityId = e.SourceFacilityId;
            SourceFacilityName = e.SourceFacilityName;
            SourceLocation = StringFunctions.ConcatNonEmptyStrings(new[] { e.SourceCity, e.SourceState?.Name }, ", ");
            Status = e.Status.GetDisplayName();
            CurrentOfficeName = e.CurrentOffice?.Name;
            CurrentOwnerName = e.CurrentOwner?.SortableFullName;
            ReviewComments = e.ReviewComments;
            PrimaryConcern = e.PrimaryConcern.Name;
            ComplaintNature = e.ComplaintNature;
            DateComplaintClosed = e.DateComplaintClosed;

            var firstComplaintAction = e.ComplaintActions.FirstOrDefault();
            ActionDate = firstComplaintAction?.ActionDate;
            ActionType = firstComplaintAction?.ActionType.Name;
            ActionComments = firstComplaintAction?.Comments;
        }

        [XLColumn(Header = "Complaint ID")]
        public int ComplaintId { get; set; }

        [XLColumn(Header = "Received By")]
        public string ReceivedByName { get; set; }

        [XLColumn(Header = "Date Received")]
        public DateTime DateReceived { get; set; }

        [XLColumn(Header = "Status")]
        public string Status { get; set; }

        [XLColumn(Header = "Review Comments")]
        public string ReviewComments { get; set; }

        [XLColumn(Header = "Source Name")]
        public string SourceFacilityName { get; set; }

        [XLColumn(Header = "Source Location")]
        public string SourceLocation { get; set; }

        [XLColumn(Header = "Facility ID")]
        public string SourceFacilityId { get; set; }

        [XLColumn(Header = "Current Assignment")]
        public string CurrentOwnerName { get; set; }

        [XLColumn(Header = "EPD Office")]
        public string CurrentOfficeName { get; set; }

        [XLColumn(Header = "Primary Area of Concern")]
        public string PrimaryConcern { get; set; }

        [XLColumn(Header = "Nature of Complaint")]
        public string ComplaintNature { get; set; }

        [XLColumn(Header = "Most Recent Action")]
        public DateTime? ActionDate { get; set; }

        [XLColumn(Header = "Action Type")]
        public string ActionType { get; set; }

        [XLColumn(Header = "Action Comments")]
        public string ActionComments { get; set; }

        [XLColumn(Header = "Date Complaint Closed")]
        public DateTime? DateComplaintClosed { get; set; }
    }
}
