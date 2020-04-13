using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ComplaintTracking
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            // First, look for [Display(Name="...")]
            var displayAttr = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            if (displayAttr != null)
            {
                return displayAttr.GetName();
            }

            // Next look for [DisplayName("...")]
            var displayNameAttr = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayNameAttribute>();

            if (displayNameAttr != null)
            {
                return displayNameAttr.DisplayName;
            }

            // Otherwise, just return ToString
            return enumValue.ToString();
        }
    }
}