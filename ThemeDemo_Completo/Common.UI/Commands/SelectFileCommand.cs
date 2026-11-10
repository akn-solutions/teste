using Ookii.Dialogs.Wpf;


namespace Common.UI.Commands;

public class SelectFileCommand
{
    /// <summary>
    /// Seleciona o arquivo e retorna o caminho
    /// </summary>
    /// <param name="currentFile">Último arquivo selecionado</param>
    /// <param name="defaultExtension">Extensão padrão do arquivo</param>
    /// <param name="filter">Filtro de arquivos</param>        
    /// <returns>
    /// responseUser = true se OK, false se cancelar  
    /// resultPath = caminho selecionado
    public static (bool responseUser, string resultPath) SetSelectFile(string currentFile, string defaultExtension = "txt", string filter = "Solid Works Part (*.sldprt)|*.sldprt")
    {
        string fullFilePath = currentFile;

        var dialog = new VistaOpenFileDialog
        {
            Title = "Selecione o Arquivo",
            DefaultExt = defaultExtension,
            Filter = filter,
            FilterIndex = 2
        };


        if (!string.IsNullOrWhiteSpace(currentFile))
        {
            string directory = System.IO.Path.GetDirectoryName(currentFile);

            if (System.IO.Directory.Exists(directory))
                dialog.InitialDirectory = directory;


            dialog.FileName = currentFile;
        }

        var result = dialog.ShowDialog();

        if (result == true && !string.IsNullOrWhiteSpace(dialog.FileName))
        {
            fullFilePath = dialog.FileName;
            return (true, fullFilePath);
        }

        return (false, fullFilePath);
    }
}