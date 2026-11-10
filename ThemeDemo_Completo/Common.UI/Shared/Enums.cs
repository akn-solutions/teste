namespace Common.UI.Shared;

public class Enums
{
    public enum SegmentTypeI
    {
        SketchLine = 0,
        SketchArc = 1,
        SketchEllipse = 2,
        SketchSpline = 3,
        Unknown = 4
    }

    public enum LinhaSentido
    {
        Horizontal = 0,
        Vertical = 1,
        Circulo = 2,
        Inclinada = 3
    }

    public enum TypeProperty
    {
        Codigo = 0,
        Material = 1,
        IProperty = 2
    }

    public enum Definition
    {
        Igual = 0,
        Maior = 1,
        Menor = 2
    }

    public enum MarcacaoType
    {
        Circulo = 0,
        Linha = 1,
        Triangulo = 2
    }
}
