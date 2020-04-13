using ComplaintTracking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintTracking.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(ApplicationUser user)
        {
            if (user != null)
            {
                Id = user.Id;
                Active = user.Active;
                Email = user.Email;
                Phone = user.Phone;
                FullName = user.FullName;
                Office = user.Office?.Name;
                SortableFullName = user.SortableFullName;
            }
        }

        public string Id { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = "Name")]
        public string SortableFullName { get; set; }

        [UIHint("EmailAddress")]
        public string Email { get; set; }

        public string EmailMD5Hash
        {
            get
            {
                return string.Join("",
                    System.Security.Cryptography.MD5.Create()
                    .ComputeHash(System.Text.Encoding.ASCII.GetBytes(Email.Trim().ToLower()))
                    .Select(s => s.ToString("x2"))
                    );
            }
        }

        [Display(Name = "Phone Number")]
        [DisplayFormat(
            NullDisplayText = CTS.NoneDisplayText,
            ConvertEmptyStringToNull = true)]
        public string Phone { get; set; }

        [Display(Name = "Office")]
        [DisplayFormat(
            NullDisplayText = CTS.NoneDisplayText,
            ConvertEmptyStringToNull = true)]
        public string Office { get; set; }

        [Display(Name = "Active?")]
        [UIHint("BooleanActive")]
        public bool Active { get; set; }

        [Display(Name = "CTS Roles")]
        public IList<CtsRole> CtsRoles { get; set; }

        public IList<string> Roles
        {
            get
            {
                return CtsRoles
                    .Select(x => x.ToString())
                    .ToList();
            }
            set
            {
                CtsRoles = value
                    .Select(x => Enum.Parse(typeof(CtsRole), x))
                    .Cast<CtsRole>()
                    .ToList();
            }
        }
    }
}
