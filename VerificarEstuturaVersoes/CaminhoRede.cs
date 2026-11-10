using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

public static class CaminhoRede
{
    [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
    static extern int WNetGetConnection(string localName, StringBuilder remoteName, ref int length);

    /// <summary>"N:" -> "\\10.1.1.254\ENG". Retorna null se não for unidade de rede.</summary>
    public static string UncDaUnidade(string unidade)
    {
        var sb = new StringBuilder(1024);
        int len = sb.Capacity;
        return WNetGetConnection(unidade.TrimEnd('\\'), sb, ref len) == 0
            ? sb.ToString().TrimEnd('\\')
            : null;
    }

    /// <summary>Todas as unidades de rede mapeadas no PC: "N:" -> "\\servidor\compartilhamento".</summary>
    public static Dictionary<string, string> Mapeamentos()
    {
        var lista = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var d in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Network))
        {
            string unidade = d.Name.TrimEnd('\\');
            string unc = UncDaUnidade(unidade);
            if (unc != null) lista[unidade] = unc;
        }

        foreach (var m in lista)
            Console.WriteLine($@"{m.Key}\ -> {m.Value}\");

        return lista;
    }

    /// <summary>Converte qualquer caminho para UNC (se estiver numa unidade de rede).</summary>
    public static string ParaUnc(string caminho)
    {
        if (string.IsNullOrEmpty(caminho) || caminho.StartsWith(@"\\")) return caminho;
        string unidade = Path.GetPathRoot(caminho).TrimEnd('\\');
        string unc = UncDaUnidade(unidade);
        return unc == null ? caminho : unc + caminho.Substring(unidade.Length);
    }

    /// <summary>Converte um caminho UNC para a unidade mapeada correspondente (se houver).</summary>
    public static string ParaMapeado(string caminho)
    {
        if (string.IsNullOrEmpty(caminho) || !caminho.StartsWith(@"\\")) return caminho;

        // o mapeamento mais específico primeiro (\\srv\ENG\PROJETOS antes de \\srv\ENG)
        foreach (var m in Mapeamentos().OrderByDescending(m => m.Value.Length))
        {
            if (caminho.Equals(m.Value, StringComparison.OrdinalIgnoreCase) ||
                caminho.StartsWith(m.Value + "\\", StringComparison.OrdinalIgnoreCase))
                return m.Key + caminho.Substring(m.Value.Length);
        }
        return caminho;
    }

    /// <summary>Compara dois caminhos independente de estarem em UNC ou unidade mapeada.</summary>
    public static bool MesmoCaminho(string a, string b) =>
        string.Equals(ParaUnc(a)?.TrimEnd('\\'), ParaUnc(b)?.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase);
}