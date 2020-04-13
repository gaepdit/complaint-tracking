using System;
using System.Collections.Generic;
using ComplaintTracking.Data;
namespace ComplaintTracking.Models
{
    public class Office : IAuditable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public bool Active { get; set; } = true;

        public virtual ApplicationUser MasterUser { get; set; }
        public string MasterUserId { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
