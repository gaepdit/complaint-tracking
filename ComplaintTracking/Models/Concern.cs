using System;
using System.ComponentModel.DataAnnotations;
using ComplaintTracking.Data;

namespace ComplaintTracking.Models
{
    public class Concern : IAuditable
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        [DisplayFormat(
            NullDisplayText = CTS.NotEnteredDisplayText,
            ConvertEmptyStringToNull = true)]
        public string Name { get; set; }
        public bool Active { get; set; } = true;
    }
}
