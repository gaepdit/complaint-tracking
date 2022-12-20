using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Cts.WebApp.Platform.PageDisplayHelpers;

/// <summary>
/// Enumeration type extension methods.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets an attribute on an enum field value.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to retrieve.</typeparam>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// The attribute of the specified type or null.
    /// </returns>
    private static T? GetAttributeOfType<T>(this Enum enumValue) where T : Attribute
    {
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumValue.ToString()).First();
        var attributes = memInfo.GetCustomAttributes<T>(false);
        return attributes.FirstOrDefault();
    }

    /// <summary>
    /// Gets the enum display name.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// Use <see cref="DisplayAttribute"/> if exists.
    /// Otherwise, use the standard string representation.
    /// </returns>
    public static string GetDisplayName(this Enum enumValue)
    {
        var attribute = enumValue.GetAttributeOfType<DisplayAttribute>();
        return attribute?.Name ?? enumValue.ToString();
    }

    /// <summary>
    /// Gets the enum description.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// Use <see cref="DescriptionAttribute"/> if exists.
    /// Otherwise, use the standard string representation.
    /// </returns>
    public static string GetDescription(this Enum enumValue)
    {
        var attribute = enumValue.GetAttributeOfType<DescriptionAttribute>();
        return attribute?.Description ?? enumValue.ToString();
    }
}
