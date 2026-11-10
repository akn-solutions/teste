# Modificações Necessárias no Common.UI

Para integrar o sistema de temas (Dark, Light e System) no seu projeto Common.UI, você precisa fazer as seguintes modificações:

## 📋 Resumo das Mudanças

1. ✅ **CRIADO**: `Themes/LightTheme.xaml` - Arquivo de tema claro
2. ✅ **CRIADO**: `Themes/DarkTheme.xaml` - Arquivo de tema escuro
3. ⚠️ **MODIFICAR**: `Themes/Brushes.xaml` - Adicionar referências dinâmicas para os temas
4. ⚠️ **MODIFICAR**: `App.xaml` - Incluir os temas no ResourceDictionary
5. ⚠️ **MODIFICAR**: Seus outros arquivos XAML de temas - Usar `DynamicResource` ao invés de `StaticResource`

---

## 🔧 MODIFICAÇÕES DETALHADAS

### 1. Common.UI/Themes/Brushes.xaml

**O que fazer**: Adicionar cor dinâmica para background e foreground da janela

**Adicione NO FINAL do arquivo `Brushes.xaml`, antes da tag de fechamento `</ResourceDictionary>`:**

```xml
    <!-- Dynamic Window Brushes for Theme Support -->
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="{DynamicResource WindowBackgroundColor}"/>
    <SolidColorBrush x:Key="WindowForegroundBrush" Color="{DynamicResource WindowForegroundColor}"/>
```

**Antes:**
```xml
    <VisualBrush
        x:Key="textPlaceHolderValor"
        AlignmentX="Center"
        Stretch="None"
        TileMode="None">
        <VisualBrush.Visual>
            <TextBlock
                Padding="10,0,0,0"
                Foreground="{StaticResource CorGray}"
                Text="Insira o Valor..." />
        </VisualBrush.Visual>
    </VisualBrush>

</ResourceDictionary>
```

**Depois:**
```xml
    <VisualBrush
        x:Key="textPlaceHolderValor"
        AlignmentX="Center"
        Stretch="None"
        TileMode="None">
        <VisualBrush.Visual>
            <TextBlock
                Padding="10,0,0,0"
                Foreground="{StaticResource CorGray}"
                Text="Insira o Valor..." />
        </VisualBrush.Visual>
    </VisualBrush>

    <!-- Dynamic Window Brushes for Theme Support -->
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="{DynamicResource WindowBackgroundColor}"/>
    <SolidColorBrush x:Key="WindowForegroundBrush" Color="{DynamicResource WindowForegroundColor}"/>

</ResourceDictionary>
```

---

### 2. IMPORTANTE: Converter StaticResource para DynamicResource

Para que os temas funcionem corretamente em tempo de execução, você DEVE converter os estilos existentes:

**Afeta os arquivos:**
- `Themes/Buttons.xaml`
- `Themes/Labels.xaml`
- `Themes/Windows.xaml`
- `Themes/TextBoxes.xaml`
- E todos os outros arquivos que usam cores

**O que mudar:**

❌ **De:**
```xml
<Setter Property="Foreground" Value="{StaticResource CorBlack}" />
```

✅ **Para:**
```xml
<Setter Property="Foreground" Value="{DynamicResource CorBlack}" />
```

**Exemplo no Buttons.xaml:**
```xml
<!-- ANTES -->
<Style x:Key="StyleDefaultButton">
    <Setter Property="Control.Foreground" Value="{StaticResource CorBlack}" />
    <Setter Property="Control.BorderBrush" Value="{StaticResource CorGreen500}" />
    ...
</Style>

<!-- DEPOIS -->
<Style x:Key="StyleDefaultButton">
    <Setter Property="Control.Foreground" Value="{DynamicResource CorBlack}" />
    <Setter Property="Control.BorderBrush" Value="{DynamicResource CorGreen500}" />
    ...
</Style>
```

---

## 📦 Arquivos Criados Automaticamente

Os seguintes arquivos já foram criados para você:

### ✅ No Common.UI:
- `Themes/LightTheme.xaml` - Cores para tema claro
- `Themes/DarkTheme.xaml` - Cores para tema escuro

### ✅ Novo Projeto de Demo:
- `ThemeDemo/` - Projeto WPF completo com suporte a temas
  - `ThemeManager.cs` - Gerenciador de temas
  - `App.xaml` e `App.xaml.cs` - Aplicação
  - `MainWindow.xaml` e `MainWindow.xaml.cs` - Interface com dropdown

---

## 🚀 Como Usar Após as Modificações

### No seu projeto WPF que usa Common.UI:

```csharp
// Em App.xaml.cs
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    
    // Carrega o tema salvo ou usa System por padrão
    var savedTheme = Properties["Theme"]?.ToString() ?? "System";
    
    if (Enum.TryParse<ThemeMode>(savedTheme, out var theme))
    {
        ThemeManager.SetTheme(theme);
    }
}

// Quando o usuário mudar o tema (ex: em um ComboBox)
private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (Enum.TryParse<ThemeMode>(ThemeCombo.SelectedItem.ToString(), out var theme))
    {
        ThemeManager.SetTheme(theme);
        Application.Current.Properties["Theme"] = theme.ToString();
        Application.Current.SaveProperties();
    }
}
```

---

## 💡 Dicas Importantes

1. **DynamicResource vs StaticResource**:
   - `StaticResource` é resolvido em tempo de compilação (não muda em runtime)
   - `DynamicResource` é resolvido em tempo de execução (muda quando o tema muda)

2. **Windows 10/11 System Theme**:
   - O ThemeManager detecta automaticamente se o Windows está em modo claro ou escuro
   - Lê a configuração em: `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\AppsUseLightTheme`

3. **Persistência**:
   - A preferência do usuário é salva em `Application.Current.Properties`
   - Carrega automaticamente na próxima execução

---

## ✅ Checklist de Implementação

- [ ] Adicionei as cores `WindowBackgroundColor` e `WindowForegroundColor` em `Brushes.xaml`
- [ ] Converti todos os `StaticResource` para `DynamicResource` em todos os arquivos XAML
- [ ] Testei a troca de temas no projeto ThemeDemo
- [ ] Copiei o `ThemeManager.cs` para meu projeto
- [ ] Implementei a lógica de seleção de tema no meu projeto
- [ ] Salvei as preferências do usuário

---

## 📝 Exemplo Prático

Veja o projeto `ThemeDemo` para um exemplo completo funcional com:
- Dropdown para selecionar tema
- Botão que muda de cor com o tema
- TextBlock que responde ao tema
- Persistência de preferências

