using Common.UI.ViewModels;
using System.Windows;
using System.Windows.Input;


namespace Common.UI.Views;

public partial class ProgressBarView : Window
{
    public ProgressBarView(ProgressBarViewModel progressBarViewModel)
    {
        InitializeComponent();
        this.MouseDown += ProgressBarView_MouseDown;
        DataContext = progressBarViewModel;
    }


    /// <summary>
    /// Permite arrastar a janela clicando em qualquer parte dela
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProgressBarView_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (sender is Window window)
                window.DragMove();
        }
    }
}