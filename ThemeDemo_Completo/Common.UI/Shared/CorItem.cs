using System.Windows.Media;


namespace Common.UI.Shared;

public class CorItem
{
    /// <summary>
    /// Nome exibido para o usuário, como "Vermelho", "Amarelo", etc.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Nome oriund odo JSON "vermelho", "amarelo", etc.
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Cor exibida no formulário, como vermelho, amarelo, etc.
    /// </summary>
    public SolidColorBrush Brush { get; set; }
    /// <summary>
    /// Número da cor no padrão ACI (AutoCAD Color Index), como 1 para vermelho, 2 para amarelo, etc.
    /// </summary>
    public int ACI { get; set; }
}
