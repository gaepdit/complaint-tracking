using ComplaintTracking.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class ActionType : IAuditable
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public bool Active { get; set; } = true;
    }
}
