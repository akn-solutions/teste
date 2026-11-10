using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;


namespace Common.UI.ViewModels;

public class ProgressBarViewModel : ViewModelBase
{
    const int _diametroExterno = 300;
    int _diametroInterno = 0;
    const int _gap = 14; // espaço entre os arcos

    public ProgressBarViewModel()
    {
        ExecucaoReal();

        //ExecucaoTesteVisual();
    }

    private void ExecucaoReal()
    {
        StrokeThickness = 16;
        StrokeThicknessSub = 8;

        // SEMPRE DEVE EXECUTAR
        CalcularDiametroInterno();

        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Arc = 0; // teste 270 - progresso principal (0–360)                
            ArcSub = 0; // teste 180 - progresso secundário

            ArcText = 0; // texto %                                
            ArcSubText = 0;

            ArcSubVisibility = Visibility.Visible;
            ArcSubTextVisibility = Visibility.Visible;

            TxtProgresso = "Obtendo Informações";
            TxtNFilesStart = 0;
            TxtNFilesEnd = 0;
            TxtNFilesVisibility = Visibility.Visible;
        }
    }

    /// <summary>
    /// Calcula o diâmetro interno do arco interno
    /// </summary>
    private void CalcularDiametroInterno()
    {
        _diametroInterno = _diametroExterno - (StrokeThickness / 2)
        - (StrokeThicknessSub / 2) - (_gap * 2);

        OnPropertyChanged(nameof(ArcSubGeometry));
    }


    private int strokeThickness;
    /// <summary>
    /// Esposura da borda do arco
    /// </summary>
    public int StrokeThickness
    {
        get { return strokeThickness; }
        set
        {
            strokeThickness = value;
            OnPropertyChanged();
        }
    }


    private int strokeThicknessSub;
    /// <summary>
    /// Esposura da borda do arco secundário
    /// </summary>
    public int StrokeThicknessSub
    {
        get { return strokeThicknessSub; }
        set
        {
            strokeThicknessSub = value;
            OnPropertyChanged();
        }
    }


    private double arc;
    /// <summary>
    /// Progresso principal (0–360 graus)
    /// </summary>
    public double Arc
    {
        get { return arc; }
        set
        {
            arc = value;

            // Converte graus para porcentagem (0–100)
            arcText = Math.Round(value / 3.6, 0);

            OnPropertyChanged(); // atualiza UI
            OnPropertyChanged(nameof(ArcText)); // atualiza texto %
            OnPropertyChanged(nameof(ArcGeometry)); // atualiza desenho do arco
        }
    }
    // Usa método reutilizável para criar a geometria do arco
    public Geometry ArcGeometry => CreateArc(Arc, _diametroExterno, StrokeThickness);


    private double arcSub;
    /// <summary>
    /// Progresso secundário (interno)
    /// </summary>
    public double ArcSub
    {
        get { return arcSub; }
        set
        {
            arcSub = value;

            // Converte para %
            arcSubText = Math.Round(value / 3.6, 0);

            OnPropertyChanged(nameof(ArcSubText));
            OnPropertyChanged(nameof(ArcSubGeometry));
        }
    }
    // Usa método reutilizável para criar a geometria do arco
    public Geometry ArcSubGeometry => CreateArc(ArcSub, _diametroInterno, StrokeThicknessSub);


    private Visibility arcSubVisibility;
    /// <summary>
    /// Controla se o arco interno aparece
    /// </summary>
    public Visibility ArcSubVisibility
    {
        get { return arcSubVisibility; }
        set
        {
            arcSubVisibility = value;
            OnPropertyChanged();
        }
    }


    private double arcText;
    /// <summary>
    /// Texto (%) do arco principal
    /// </summary>
    public double ArcText
    {
        get { return arcText; }
        set { arcText = value; }
    }


    private double arcSubText;
    /// <summary>
    ///  Texto (%) do arco secundário
    /// </summary>
    public double ArcSubText
    {
        get { return arcSubText; }
        set { arcSubText = value; }
    }


    private Visibility arcSubTextVisibility;
    /// <summary>
    /// Controla visibilidade do texto secundário
    /// </summary>
    public Visibility ArcSubTextVisibility
    {
        get { return arcSubTextVisibility; }
        set
        {
            arcSubTextVisibility = value;
            OnPropertyChanged();
        }
    }


    private string txtProgresso;
    /// <summary>
    /// Texto de status (ex: "Processando...")
    /// </summary>
    public string TxtProgresso
    {
        get { return txtProgresso; }
        set
        {
            txtProgresso = value;
            OnPropertyChanged();
        }
    }

    private int txtNFilesStart;
    /// <summary>
    /// Quantidade inicial de arquivos
    /// </summary>
    public int TxtNFilesStart
    {
        get { return txtNFilesStart; }
        set
        {
            txtNFilesStart = value;
            OnPropertyChanged();
        }
    }


    private int txtNFilesEnd;
    /// <summary>
    /// Quantidade total de arquivos
    /// </summary>
    public int TxtNFilesEnd
    {
        get { return txtNFilesEnd; }
        set
        {
            txtNFilesEnd = value;
            OnPropertyChanged();
        }
    }


    private Visibility txtNFilesVisibility;
    /// <summary>
    /// Mostra/oculta info de arquivos
    /// </summary>
    public Visibility TxtNFilesVisibility
    {
        get { return txtNFilesVisibility; }
        set
        {
            txtNFilesVisibility = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    /// NOVO: gera o desenho do arco (substitui o ed:Arc)
    /// </summary>
    /// <param name="angle">valor em graus</param>
    /// <param name="radius">raio do círculo</param>
    /// <returns></returns>
    private Geometry CreateArc(double angle, double diameter, int strokeTickness)
    {
        double radius = (diameter / 2) - (strokeTickness / 2.0);  // ajusta raio pra ficar interno à borda
        double center = _diametroExterno / 2; // centro Xe Y

        double startAngle = -90; // começa no topo
        double endAngle = startAngle + angle;

        // converte graus → radianos
        double startRad = startAngle * Math.PI / 180;
        double endRad = endAngle * Math.PI / 180;

        // ponto inicial do arco
        Point startPoint = new Point(
            center + radius * Math.Cos(startRad),
            center + radius * Math.Sin(startRad));

        // ponto final do arco
        Point endPoint = new Point(
            center + radius * Math.Cos(endRad),
            center + radius * Math.Sin(endRad));

        // define se o arco é maior que 180°
        bool isLargeArc = angle >= 180;

        // cria a geometria do arco
        var figure = new PathFigure
        {
            StartPoint = startPoint,
            IsClosed = false,
            IsFilled = false
        };

        figure.Segments.Add(new ArcSegment(
            endPoint,
            new Size(radius, radius),
            0,
            isLargeArc,
            SweepDirection.Clockwise,
            true));

        return new PathGeometry(new[] { figure });
    }


    #region Testes   
    private void ExecucaoTesteVisual()
    {
        StrokeThickness = 16;
        StrokeThicknessSub = 8;

        // SEMPRE DEVE EXECUTAR
        CalcularDiametroInterno();

        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Arc = 0; // teste 270 - progresso principal (0–360)                
            ArcSub = 120; // teste 180 - progresso secundário


            ArcSubVisibility = Visibility.Visible;
            ArcSubTextVisibility = Visibility.Visible;

            TxtProgresso = "Obtendo Informaçõesc";
            TxtNFilesStart = 1;
            TxtNFilesEnd = 10;
            TxtNFilesVisibility = Visibility.Visible;

            StartDesignAnimation();
        }
    }

    private void StartDesignAnimation()
    {
        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(30);

        timer.Tick += (s, e) =>
        {
            Arc += 3;      // principal
            ArcSub += 5;   // secundário

            if (Arc >= 360) Arc = 0;
            if (ArcSub >= 360) ArcSub = 0;
        };

        timer.Start();
    }
    #endregion
}