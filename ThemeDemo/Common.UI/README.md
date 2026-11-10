# Componentes de UI Reutilizáveis para Aplicações Desktop .NET
Solução modular para componentes visuais, temas, estilos e controles personalizados para aplicações WPF, com suporte a templates, behaviors, commands e gerenciamento de temas, construída com separação clara de responsabilidades e reutilizabilidade entre projetos.

---

## 📁 Estrutura do Projeto

```
Project 'Common.UI'
├── Dependencies/
├── Assets/
│   ├── Icons/
│   └── Images/
├── Behaviors/
│   ├── TextBoxCaretBehavior.cs
│   └── TextBoxUpperCaseBehavior.cs
├── Commands/
│   ├── CancelarCommand.cs
│   ├── RelayCommand.cs
│   ├── SelectFileCommand.cs
│   └── SelectFolderCommand.cs
├── Shared/
│   ├── ColorPalette.cs
│   ├── CoreItem.cs
│   ├── Enums.cs
│   └── Extensions.cs
├── Styles/
│   ├── AknStyles.xaml            # Call within the projects
│   └── ThemeResourceDictionary.cs
├── Themes/
│   ├── Brushes.xaml
│   ├── Buttons.xaml
│   ├── CheckBoxes.xaml
│   ├── Colors.xaml
│   ├── ComboBoxes.xaml
│   ├── Labels.xaml
│   ├── ListViews.xaml
│   ├── PasswordBoxes.xaml
│   ├── RadioButtons.xaml
│   ├── Sizes.xaml
│   ├── StackPanels.xaml
│   ├── TextBlocks.xaml
│   ├── TextBoxes.xaml
│   ├── Thickness.xaml
│   ├── TreeViewItems.xaml
│   └── Windows.xaml
├── ViewModels/
│   ├── ProgressBarViewModel.cs
│   └── ViewModelBase.cs
└── Views/
    └── ProgressBarView.xaml
```

---

## 📦 Dependências Externas - Packages

| Pacote | Versão | Propósito |
|--------|--------|----------|
| `Microsoft.Xaml.Behaviors.Wpf` | 1.1 | Behaviors para controles WPF |
| `Ooki.Dialogs.Wpf` | 5.0.1 | Diálogos personalizados para WPF |

---

## 🎯 Funcionalidades

- ✅ Componentes WPF personalizados e reutilizáveis
- ✅ Behaviors para controles (TextBox(Upper Case, cursor no final do conteúdo))
- ✅ Commands comuns para ações de UI
- ✅ Paleta de cores centralizada
- ✅ Templates XAML pré-definidos
- ✅ ViewModels base para MVVM
- ✅ Assets de ícones e imagens inclusos
- ❌ Sistema completo de temas e estilos

---

## 🚀 Como Usar

### Pré-requisitos

- .NET Framework 4.8.1 ou superior
- WPF (Windows Presentation Foundation)
- Referência do projeto no seu arquivo .csproj

### Instalação

1. Adicione a referência do projeto `Common.UI` ao seu projeto:
   ```xml
   <ProjectReference Include="...\Common.UI.csproj" />
   ```

2. Restaure os pacotes NuGet:
   ```bash
   dotnet restore
   ```

---

## 🏗️ Arquitetura

```
┌──────────────────────────────┐
│  Views                       │  ← Componentes Visuais
│  └─ ProgressBarView          │
├──────────────────────────────┤
│  ViewModels                  │  ← Lógica
│  ├─ ProgressBarViewModel     │
│  └─ ViewModelBase            │
├──────────────────────────────┤
│  Themes                      │  ← Temas e Estilos
├──────────────────────────────┤
│  Commands                    │  ← Ações de UI
├──────────────────────────────┤
│  Behaviors                   │  ← Comportamentos
├──────────────────────────────┤
│  Assets                      │  ← Ícones e Imagens
└──────────────────────────────┘
```

---

## 📦 Versões

### 📌 Versão atual **v1.0.0**
👉 [🔽 Baixar última versão](https://github.com/akn-solutions/dotnet-common/releases/tag/v1.0.0-ui)

- [versão v1.0.0](https://github.com/akn-solutions/dotnet-common/releases/tag/v1.0.0-ui)

---

## 🎓 Recursos Adicionais

- [WPF Documentation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf)
- [MVVM Pattern](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
- [Best Practices .NET](https://docs.microsoft.com/en-us/dotnet/fundamentals)