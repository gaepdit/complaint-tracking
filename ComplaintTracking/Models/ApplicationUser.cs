using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ComplaintTracking.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// See: https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(25)]
        public string Phone { get; set; }

        [InverseProperty("Users")]
        public virtual Office Office { get; set; }
        public Guid? OfficeId { get; set; }

        public bool Active { get; set; } = true;

        // Generated fields
        public string FullName
        {
            get
            {
                return String.Join(" ", new string[] { FirstName, LastName }.Where(s => !string.IsNullOrEmpty(s)));
            }
        }

        [DisplayFormat(
            NullDisplayText = CTS.NotAvailableDisplayText,
            ConvertEmptyStringToNull = true)]
        public string SortableFullName
        {
            get
            {
                return String.Join(", ", new string[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));
            }
        }

        public string SelectableName
        {
            get
            {
                var sn = new StringBuilder();
                sn.AppendJoin(", ", new string[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));
                if (!Active)
                {
                    sn.Append(" [Inactive]");
                }

                return sn.ToString();
            }
        }

        public string SelectableNameWithOffice
        {
            get
            {
                var sn = new StringBuilder();
                sn.AppendJoin(", ", new string[] { LastName, FirstName }.Where(s => !string.IsNullOrEmpty(s)));
                if(Office != null)
                {
                    sn.Append(" – ");
                    sn.Append(Office.Name);
                }
                if (!Active)
                {
                    sn.Append(" [Inactive]");
                }
                return sn.ToString();
            }
        }
    }
}
