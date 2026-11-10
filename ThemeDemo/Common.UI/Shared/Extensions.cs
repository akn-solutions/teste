using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Common.UI.Shared;

public static class Extensions
{
    // Convert um List<T> em um ObservableCollection<T>               
    private static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable) => new ObservableCollection<T>(enumerable);


    /// <summary>
    /// Converte o tipo Enum para uma string para poder ser salvo em um arquivo texto
    /// </summary>
    public static string GetStringTypeProperty(this Enums.TypeProperty value)
    {
        string valorString = string.Empty;

        switch (value)
        {
            case Enums.TypeProperty.Codigo:
                valorString = "Código";
                break;
            case Enums.TypeProperty.Material:
                valorString = "Material";
                break;
            case Enums.TypeProperty.IProperty:
                valorString = "iProperty";
                break;
        }

        return valorString;
    }


    /// <summary>
    /// Converte o tipo string para o tipo Enums.TypeProperty
    /// </summary>
    public static Enums.TypeProperty ToEnumTypeProperty(this string value)
    {
        Enums.TypeProperty valorEnum = Enums.TypeProperty.Codigo;
        switch (value)
        {
            case "Código":
                valorEnum = Enums.TypeProperty.Codigo;
                break;
            case "Material":
                valorEnum = Enums.TypeProperty.Material;
                break;
            case "iProperty":
                valorEnum = Enums.TypeProperty.IProperty;
                break;
        }

        return valorEnum;
    }


    /// <summary>
    /// Converte o tipo string para o tipo Enums.Definition
    /// </summary>
    public static Enums.Definition ToEnumDefinition(this string value)
    {
        Enums.Definition valorEnum = Enums.Definition.Igual;
        switch (value)
        {
            case "=":
                valorEnum = Enums.Definition.Igual;
                break;
            case "<":
                valorEnum = Enums.Definition.Maior;
                break;
            case ">":
                valorEnum = Enums.Definition.Menor;
                break;
        }

        return valorEnum;
    }


    /// <summary>
    /// Convert string to double
    /// </summary>
    public static double ToDouble(this string valorStr)
    {
        double valorInt = 0;
        Double.TryParse(valorStr, out valorInt);
        return valorInt;
    }


    // Converte STRING p/ INTEIRO(Int32)
    public static Int32 ToInt32(this object value)
    {
        string strValue = value.ToString();
        int intTemp = 0;

        Int32.TryParse(strValue, out intTemp);

        return intTemp;
    }

}