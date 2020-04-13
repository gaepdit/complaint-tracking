using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class State
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(2)]
        public string PostalAbbreviation { get; set; }
    }
}
