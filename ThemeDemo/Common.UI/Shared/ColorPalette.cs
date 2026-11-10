using System.Collections.ObjectModel;
using System.Windows.Media;


namespace Common.UI.Shared;

public static class ColorPalette
{
    public static readonly ObservableCollection<CorItem> Default =
        new ObservableCollection<CorItem>
        {
            new CorItem { Name = "Vermelho", Key = "vermelho", Brush = Brushes.Red,     ACI = 1 },
            new CorItem { Name = "Amarelo",  Key = "amarelo",  Brush = Brushes.Yellow,  ACI = 2 },
            new CorItem { Name = "Verde",    Key = "verde",    Brush = Brushes.Lime,    ACI = 3 }, // Verde puro
            new CorItem { Name = "Ciano",    Key = "ciano",    Brush = Brushes.Cyan,    ACI = 4 }, // Ciano
            new CorItem { Name = "Azul",     Key = "azul",     Brush = Brushes.Blue,    ACI = 5 },
            new CorItem { Name = "Magenta",  Key = "magenta",  Brush = Brushes.Magenta, ACI = 6 },
        };
}
