using System;
using System.Windows;


namespace Common.UI.Styles;

public class ThemeResourceDictionary : ResourceDictionary
{
    private const string BasePath = "/Common.UI;component/Themes/";

    public new string Source
    {
        set => base.Source = new Uri(BasePath + value, UriKind.Relative);
    }
}
