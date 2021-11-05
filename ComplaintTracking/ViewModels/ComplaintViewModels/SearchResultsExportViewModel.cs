using ClosedXML.Attributes;
using ComplaintTracking.Models;
using System;

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
        }

        [XLColumn(Header = "Complaint ID")]
        public int ComplaintId { get; set; }

        [XLColumn(Header = "Received By")]
        public string ReceivedByName { get; set; }

        [XLColumn(Header = "Date Received")]
        public DateTime DateReceived { get; set; }

        [XLColumn(Header = "Status")]
        public string Status { get; set; }

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

        [XLColumn(Header = "Review Comments")]
        public string ReviewComments { get; set; }
    }
}
