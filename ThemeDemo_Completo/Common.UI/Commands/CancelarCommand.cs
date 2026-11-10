using System.Windows;


namespace Common.UI.Commands;

public static class CancelarCommand
{
    public static void Set(object obj)
    {
        if (obj is Window window)
        {
            window.Close();
        }

        Application.Current.Shutdown();
    }
}