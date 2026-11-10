using System;
using System.Windows;
using System.Windows.Controls;

namespace ThemeDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ThemeCombo.SelectedItem is ComboBoxItem item)
        {
            var themeName = item.Content?.ToString();

            if (Enum.TryParse<ThemeMode>(themeName, out var theme))
            {
                ThemeManager.SetTheme(theme);
                Application.Current.Properties["Theme"] = theme.ToString();
            }
        }
    }

    private void DemoButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Tema mudou com sucesso!");
    }
}