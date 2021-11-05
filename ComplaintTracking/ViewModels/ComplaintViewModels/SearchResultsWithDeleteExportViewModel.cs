using ClosedXML.Attributes;
using ComplaintTracking.Models;

namespace ComplaintTracking.ViewModels
{
    public class SearchResultsWithDeleteExportViewModel : SearchResultsExportViewModel
    {
        public SearchResultsWithDeleteExportViewModel(Complaint e) : base(e) =>
            Deleted = e.Deleted ? "Deleted" : "No";

        [XLColumn(Header = "Deleted?")]
        public string Deleted { get; set; }
    }
}
