using Microsoft.Win32;
using System;
using System.Windows;

namespace ThemeDemo;

public enum ThemeMode
{
    Light,
    Dark,
    System
}

public static class ThemeManager
{
    private static readonly string ThemePath = "pack://application:,,,/Common.UI;component/Themes/";

    public static void SetTheme(ThemeMode mode)
    {
        string themeName = mode switch
        {
            ThemeMode.Dark => "DarkTheme.xaml",
            ThemeMode.Light => "LightTheme.xaml",
            ThemeMode.System => GetSystemTheme() == "Dark" ? "DarkTheme.xaml" : "LightTheme.xaml",
            _ => "LightTheme.xaml"
        };

        var uri = new Uri(ThemePath + themeName);
        var resourceDict = new ResourceDictionary { Source = uri };

        // Remove o tema anterior
        Application.Current.Resources.MergedDictionaries.Clear();

        // Adiciona o novo tema
        Application.Current.Resources.MergedDictionaries.Add(resourceDict);
    }

    private static string GetSystemTheme()
    {
        try
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");

            if (value is int intValue)
            {
                return intValue == 1 ? "Light" : "Dark";
            }
        }
        catch { }

        return "Light"; // padrão
    }
}