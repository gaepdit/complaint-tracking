using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking
{
    public enum CtsRole
    {
        [Display(Name = "Division Manager")] DivisionManager,
        [Display(Name = "Manager")] Manager,
        [Display(Name = "User Account Admin")] UserAdmin,
        [Display(Name = "Data Export")] DataExport,
        [Display(Name = "Attachments Editor")] AttachmentsEditor,
    }

    internal enum ServerEnvironment
    {
        Development,
        Staging,
        Production,
    }

    public enum SortOrder
    {
        Ascending,
        Descending,
    }
}
