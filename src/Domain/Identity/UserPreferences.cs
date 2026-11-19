using System.ComponentModel;

namespace Cts.Domain.Identity;

public record UserPreferences
{
    public ThemePreference Theme { get; set; } = ThemePreference.Light;
}

public enum ThemePreference
{
    [Description("auto")] Auto,
    [Description("light")] Light,
    [Description("dark")] Dark,
}