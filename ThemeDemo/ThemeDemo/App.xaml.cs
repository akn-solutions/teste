using System;
using System.Windows;

namespace ThemeDemo;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Carrega o tema salvo ou usa LIGHT por padrão
        var savedTheme = Properties["Theme"]?.ToString() ?? "Light";

        if (Enum.TryParse<ThemeMode>(savedTheme, out var theme))
        {
            ThemeManager.SetTheme(theme);
        }
        else
        {
            ThemeManager.SetTheme(ThemeMode.Light);  // ← AQUI: Light ao invés de System
        }
    }
}