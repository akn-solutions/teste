using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;


namespace Common.UI.Behaviors;

/// <summary>
/// Define todos os caracteres digitados em maiúsculo no TextBox associado.
/// </summary>
public class TextBoxUpperCaseBehavior : Behavior<TextBox>
{
    protected override void OnAttached() => AssociatedObject.CharacterCasing = CharacterCasing.Upper;

}
