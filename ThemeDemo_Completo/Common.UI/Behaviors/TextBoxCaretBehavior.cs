using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;


namespace Common.UI.Behaviors;

/// <summary>
/// Define o cursor no final do TextBox pra o text box associado sempre que ele tiver o foco.
/// </summary>
public class TextBoxCaretBehavior : Behavior<TextBox>
{
    protected override void OnAttached()
    {
        AssociatedObject.GotFocus += (s, e) => AssociatedObject.CaretIndex = AssociatedObject.Text.Length;
    }
}
