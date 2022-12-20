using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Platform.PageDisplayHelpers;

/// <summary>
/// Enumeration type extension methods.
/// </summary>
public static class EnumExtensions
{
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
}
