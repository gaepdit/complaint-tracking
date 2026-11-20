using System.ComponentModel;

namespace Cts.Domain.Identity;

public record UserPreferences
{
    public ThemePreference Theme { get; set; } = ThemePreference.Dark;
}

public enum ThemePreference
{
    [Description("auto")] Auto,
    [Description("light")] Light,
    [Description("dark")] Dark,
}