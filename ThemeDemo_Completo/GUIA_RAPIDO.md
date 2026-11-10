# 🎨 Guia Rápido - Sistema de Temas WPF

## 📦 O Que Foi Criado

### ✅ No Common.UI (Sua biblioteca):
```
Common.UI/
├── Themes/
│   ├── LightTheme.xaml      ← NOVO (tema claro)
│   ├── DarkTheme.xaml       ← NOVO (tema escuro)
│   └── Brushes.xaml         ← PRECISA MODIFICAR
└── ...
```

### ✅ Novo Projeto de Demo (ThemeDemo):
```
ThemeDemo/
├── App.xaml                 ← Aplicação
├── App.xaml.cs
├── MainWindow.xaml          ← Interface com dropdown
├── MainWindow.xaml.cs
├── ThemeManager.cs          ← Core do sistema de temas
├── ThemeDemo.csproj
├── ThemeDemo.sln
└── README.md
```

---

## 🚀 Passos para Usar

### **Passo 1: Modifique o Common.UI**

**Arquivo**: `Common.UI/Themes/Brushes.xaml`

Adicione ao final (antes de `</ResourceDictionary>`):

```xml
    <!-- Dynamic Window Brushes for Theme Support -->
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="{DynamicResource WindowBackgroundColor}"/>
    <SolidColorBrush x:Key="WindowForegroundBrush" Color="{DynamicResource WindowForegroundColor}"/>
```

**Passo 2**: Converter `StaticResource` → `DynamicResource`

Em TODOS os arquivos de tema (`Buttons.xaml`, `Labels.xaml`, `Windows.xaml`, etc):

```xml
<!-- ANTES -->
<Setter Property="Foreground" Value="{StaticResource CorBlack}" />

<!-- DEPOIS -->
<Setter Property="Foreground" Value="{DynamicResource CorBlack}" />
```

---

### **Passo 2: Copie o ThemeManager**

Copie o arquivo `ThemeDemo/ThemeManager.cs` para seu projeto e use assim:

```csharp
// Trocar tema
ThemeManager.SetTheme(ThemeMode.Dark);
ThemeManager.SetTheme(ThemeMode.Light);
ThemeManager.SetTheme(ThemeMode.System);
```

---

### **Passo 3: Configure seu App.xaml.cs**

```csharp
using System.Windows;

public partial class App : Application
{
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
}
```

---

### **Passo 4: Adicione um Dropdown na Interface**

```xaml
<ComboBox x:Name="ThemeCombo" SelectionChanged="ThemeCombo_SelectionChanged">
    <ComboBoxItem Content="Light"/>
    <ComboBoxItem Content="Dark"/>
    <ComboBoxItem Content="System" IsSelected="True"/>
</ComboBox>
```

---

### **Passo 5: Manipule a Seleção**

```csharp
private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (ThemeCombo.SelectedItem is ComboBoxItem item)
    {
        var themeName = item.Content?.ToString();
        
        if (Enum.TryParse<ThemeMode>(themeName, out var theme))
        {
            ThemeManager.SetTheme(theme);
            
            // Salva a preferência
            Application.Current.Properties["Theme"] = theme.ToString();
            Application.Current.SaveProperties();
        }
    }
}
```

---

## 🎯 Resultado Final

### **Light Theme** 🌞
- Fundo: Branco (`#FFFFFF`)
- Texto: Preto (`#1A1A18`)
- Botões: Cores vivas

### **Dark Theme** 🌙
- Fundo: Cinza-escuro (`#1E1E1E`)
- Texto: Branco (`#FFFFFF`)
- Botões: Mesmas cores (legíveis em qualquer tema)

### **System Theme** 🖥️
- Detecta: Configuração do Windows
- Muda automaticamente se o usuário mudar as preferências do SO

---

## 🧪 Testar o Projeto Demo

```bash
# Abra em Visual Studio
File > Open > ThemeDemo.sln

# Ou compile pelo terminal
cd ThemeDemo
dotnet build
dotnet run
```

---

## 📝 Checklist Final

- [ ] Adicionei as cores em `Brushes.xaml`
- [ ] Converti `StaticResource` → `DynamicResource` em todos os temas
- [ ] Copiei `ThemeManager.cs`
- [ ] Implementei a lógica no `App.xaml.cs`
- [ ] Adicionei o ComboBox na interface
- [ ] Testei a troca de temas
- [ ] Verifiquei persistência (fechar e abrir novamente)

---

## 💡 Dicas

1. **Use `DynamicResource`** - Necessário para temas mudarem em tempo real
2. **System Theme** - Lê automaticamente `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize`
3. **Salve as preferências** - Use `Application.Current.SaveProperties()`
4. **Teste em ambos os temas** - Garanta que a interface fica legível

---

## ❓ Dúvidas?

Veja os arquivos:
- `MODIFICACOES_COMMON_UI.md` - Alterações detalhadas
- `ThemeDemo/README.md` - Documentação completa do projeto demo
- `ThemeDemo/MainWindow.xaml` - Exemplo de interface

---

**Pronto! Você tem um sistema de temas profissional no WPF! 🎉**
