using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class County
    {
        public int Id { get; set; }

        [StringLength(20)]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string Name { get; set; }
    }
}
