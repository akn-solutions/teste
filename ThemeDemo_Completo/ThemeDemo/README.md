# Theme Demo - WPF Theme Switcher

Projeto de demonstração de um sistema de temas (Dark, Light e System) para WPF usando o Common.UI.

## 🎨 Funcionalidades

- ✅ Tema **Light** (Claro)
- ✅ Tema **Dark** (Escuro)
- ✅ Tema **System** (Detecta configuração do Windows)
- ✅ Persistência de preferências do usuário
- ✅ Transição suave entre temas

## 📁 Estrutura do Projeto

```
ThemeDemo/
├── App.xaml              # Configuração da aplicação
├── App.xaml.cs           # Code-behind da aplicação
├── MainWindow.xaml       # Interface principal
├── MainWindow.xaml.cs    # Code-behind da interface
├── ThemeManager.cs       # Gerenciador de temas (Core)
└── ThemeDemo.csproj      # Arquivo do projeto
```

## 🚀 Como Usar

### 1. Abrir o Projeto

```bash
# Usando Visual Studio
File > Open > Selecionar ThemeDemo.csproj

# Ou com dotnet CLI
dotnet build
dotnet run
```

### 2. Mudar o Tema

1. Execute a aplicação
2. Use o **ComboBox** para selecionar:
   - **Light** - Tema claro com fundo branco
   - **Dark** - Tema escuro com fundo cinza-escuro
   - **System** - Deteta a configuração do Windows

3. Observe como o botão, texto e fundo mudam de cor automaticamente

### 3. Verificar Persistência

- Mude o tema
- Feche a aplicação
- Abra novamente
- O tema anterior será carregado automaticamente

## 🔧 Componentes Principais

### ThemeManager.cs

```csharp
public enum ThemeMode
{
    Light,   // Tema claro
    Dark,    // Tema escuro
    System   // Usa configuração do Windows
}

public static class ThemeManager
{
    public static void SetTheme(ThemeMode mode)
    {
        // Carrega e aplica o tema correspondente
    }
}
```

### App.xaml.cs

Carrega o tema na inicialização:

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    var savedTheme = Properties["Theme"]?.ToString() ?? "System";
    
    if (Enum.TryParse<ThemeMode>(savedTheme, out var theme))
    {
        ThemeManager.SetTheme(theme);
    }
}
```

### MainWindow.xaml

Interface simples com:
- 📦 ComboBox para seleção de tema
- 🔘 Botão de demonstração (cores mudam com tema)
- 📝 TextBlock que responde ao tema

## 🎯 O Que Muda Com os Temas

| Elemento | Light | Dark |
|----------|-------|------|
| Background | `#FFFFFF` (Branco) | `#1E1E1E` (Cinza-Escuro) |
| Foreground | `#1A1A18` (Preto) | `#FFFFFF` (Branco) |
| Botão | Verde com texto branco | Verde com texto branco |
| Texto | Preto | Branco |

## 🔗 Dependências

- **Common.UI** - Biblioteca de componentes customizados
  - Themes/LightTheme.xaml
  - Themes/DarkTheme.xaml
  - Themes/Brushes.xaml
  - Estilos dos botões e controles

## 📋 Requisitos do Windows

- Windows 10 ou posterior
- .NET 6.0 ou superior
- Visual Studio 2022 (ou VS Code com C# Extension)

## 🐛 Troubleshooting

### Os temas não mudam?
1. Verifique se `DynamicResource` é usado nos estilos
2. Verifique se os arquivos de tema estão no local correto
3. Verifique o console de erros do Visual Studio

### A cor do botão não muda?
1. Certifique-se de que o MainWindow.xaml usa `DynamicResource`
2. Limpe e recompile a solução (`Ctrl+Shift+B`)

### System Theme não detecta?
1. Verifique se você está em Windows 10 ou posterior
2. Verifique as configurações do Windows:
   - Settings > Personalization > Colors > Choose your color

## 💡 Extensões Futuras

- [ ] Salvar tema em arquivo de configuração
- [ ] Adicionar mais temas (Blue, Purple, etc.)
- [ ] Animação na transição de tema
- [ ] Tema automático baseado na hora do dia
- [ ] Mais controles styled (TextBox, CheckBox, etc.)

## 📄 Licença

Mesmo projeto do Common.UI

## 👨‍💻 Autor

Criado para demonstração de sistema de temas em WPF

---

**Pronto para usar!** Qualquer dúvida, consulte `MODIFICACOES_COMMON_UI.md`
