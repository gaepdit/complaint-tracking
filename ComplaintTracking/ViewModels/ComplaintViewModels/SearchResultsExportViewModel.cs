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
            SetDeleted(e.Deleted);
            ReceivedByName = e.ReceivedBy?.SortableFullName;
            SourceFacilityId = e.SourceFacilityId;
            SourceFacilityName = e.SourceFacilityName;
            SetSourceCity(e.SourceCity);
            SetSourceStateName(e.SourceState?.Name);
            SetStatus(e.Status);
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

        private ComplaintStatus _status;
        private void SetStatus(ComplaintStatus value) => _status = value;

        [XLColumn(Header = "Status")]
        public string Status => _status.GetDisplayName();

        [XLColumn(Header = "Source Name")]
        public string SourceFacilityName { get; set; }

        private string _sourceStateName;
        private void SetSourceStateName(string value) => _sourceStateName = value;

        private string _sourceCity;
        private void SetSourceCity(string value) => _sourceCity = value;

        [XLColumn(Header = "Source Location")]
        public string SourceLocation =>
            StringFunctions.ConcatNonEmptyStrings(new[] { _sourceCity, _sourceStateName }, ", ");

        [XLColumn(Header = "Facility ID")]
        public string SourceFacilityId { get; set; }

        [XLColumn(Header = "Current Assignment")]
        public string CurrentOwnerName { get; set; }

        [XLColumn(Header = "EPD Office")]
        public string CurrentOfficeName { get; set; }

        [XLColumn(Header = "Review Comments")]
        public string ReviewComments { get; set; }

        private bool _deleted;
        private void SetDeleted(bool value) => _deleted = value;

        [XLColumn(Header = "Deleted?")]
        public string Deleted => _deleted ? "Deleted" : "No";
    }
}
