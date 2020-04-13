using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComplaintTracking.ViewModels
{
    public class CommonSelectLists
    {
        public SelectList CountiesSelectList { get; set; }
        public SelectList StatesSelectList { get; set; }
        public SelectList PhoneTypesSelectList { get; set; }
        public SelectList AreasOfConcernSelectList { get; set; }
        public SelectList OfficesSelectList { get; set; }
        public SelectList AllUsersSelectList { get; set; }
        public SelectList UsersInOfficeSelectList { get; set; }
    }
}
