using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintTracking.ViewModels
{
    public class AccountIndexViewModel
    {
        [Display(Name = "Name")]
        public string FullName { get; set; }

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
        public string OfficeName { get; set; }

        // Interchangeable lists of Roles (string or enum)
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
