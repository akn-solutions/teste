using Ookii.Dialogs.Wpf;


namespace Common.UI.Commands;

public class SelectFolderCommand
{
    /// <summary>
    /// Seleciona a pasta e retorna o caminho
    /// </summary>
    /// <param name="currentPath">Último caminho selecionado</param>
    /// <returns>
    /// responseUser = true se OK, false se cancelar  
    /// resultPath = caminho selecionado
    /// </returns>
    public static (bool responseUser, string resultPath) SetSelectFolder(string currentPath)
    {
        string path = currentPath;

        var dialog = new VistaFolderBrowserDialog();


        if (!string.IsNullOrWhiteSpace(currentPath))
            dialog.SelectedPath = currentPath;

        dialog.Description = "Selecione uma pasta";
        dialog.UseDescriptionForTitle = true;

        bool? result = dialog.ShowDialog();

        if (result == true && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
        {
            path = dialog.SelectedPath;
            return (true, path);
        }


        return (false, path);
    }
}