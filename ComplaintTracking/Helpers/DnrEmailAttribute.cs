using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ComplaintTracking.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DnrEmailAddressAttribute : ValidationAttribute
    {
        private const int MatchTimeoutInSeconds = 2;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !IsValidDnrEmailAddress(value.ToString()!.Trim()))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        private bool IsValidDnrEmailAddress(string emailAddress)
        {
            var regex = new Regex(RegexPatterns.DnrEmailPattern, default, TimeSpan.FromSeconds(MatchTimeoutInSeconds));
            return !string.IsNullOrEmpty(emailAddress) && regex.IsMatch(emailAddress);
        }
    }
}
