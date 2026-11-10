using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using IOFile = System.IO.File;
using IOFileInfo = System.IO.FileInfo;
using IOPath = System.IO.Path;

namespace VerificarEstuturaVersoes
{
    class Item
    {
        public Document Doc;
        public HashSet<string> NomesDocumento = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> UsadoPor = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    internal static class CompararEstruturas
    {

        static Application inv;
        static string raiz;
        static readonly StringBuilder log = new StringBuilder();
        static readonly Dictionary<string, Item> itens = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
        static readonly List<string> naoResolvidas = new List<string>();
        static readonly List<string> errosDesenho = new List<string>();


        internal static void Set()
        {
            try { inv = (Application)Marshal.GetActiveObject("Inventor.Application"); }
            catch { Sair("Inventor não está aberto."); return; }

            if (!(inv.ActiveDocument is AssemblyDocument asm)) { Sair("O documento ativo não é uma montagem."); return; }

            raiz = IOPath.GetDirectoryName(asm.FullFileName);
            Console.WriteLine("Coletando informações...");

            Ambiente(asm);
            Coletar((Document)asm, null);
            Arquivos();
            Detalhamentos();
            Problemas();
            Resumo();

            string arqLog = IOPath.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                $"Diagnostico_{IOPath.GetFileNameWithoutExtension(asm.FullFileName)}_{System.Environment.MachineName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            IOFile.WriteAllText(arqLog, log.ToString(), Encoding.UTF8);
            Sair($"Log salvo em: {arqLog}");
        }

        // ---------------------------------------------------------------- AMBIENTE
        static void Ambiente(AssemblyDocument asm)
        {
            Escrever("=== AMBIENTE ===");
            Escrever($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
            Escrever($"Usuário: {System.Environment.UserName} | Máquina: {System.Environment.MachineName} | Windows: {System.Environment.OSVersion}");

            dynamic sv = inv.SoftwareVersion;
            Escrever($"Inventor: {V(() => (string)sv.DisplayVersion)} | Build: {V(() => Convert.ToString(sv.BuildIdentifier))} | Service Pack: {V(() => Convert.ToString(sv.ServicePack))}");

            DesignProject p = inv.DesignProjectManager.ActiveDesignProject;
            Escrever("");
            Escrever("--- Projeto ativo ---");
            Escrever($"Arquivo: {V(() => p.FullFileName)}");
            Escrever($"Tipo: {V(() => Convert.ToString(((dynamic)p).ProjectType))}");
            Escrever($"Workspace: {V(() => p.WorkspacePath)}");
            try { foreach (ProjectPath pp in p.WorkgroupPaths) Escrever($"Workgroup: {pp.Name} -> {pp.Path}"); } catch { }
            try { foreach (ProjectPath pp in p.LibraryPaths) Escrever($"Library: {pp.Name} -> {pp.Path}"); } catch { }
            Escrever($"Content Center: {V(() => Convert.ToString(((dynamic)p).ContentCenterPath))}");

            Escrever("");
            Escrever("--- Montagem principal ---");
            Escrever($"Arquivo: {asm.FullFileName}");
            Escrever($"Pasta raiz (~): {raiz}");
            Escrever("");
        }

        // ---------------------------------------------------------------- COLETA
        static void Coletar(Document doc, string pai)
        {
            string chave = doc.FullFileName;
            bool novo = !itens.TryGetValue(chave, out var item);
            if (novo) { item = new Item { Doc = doc }; itens[chave] = item; }

            item.NomesDocumento.Add(doc.FullDocumentName);
            if (pai != null) item.UsadoPor.Add(Rel(pai));
            if (!novo) return;

            foreach (DocumentDescriptor dd in doc.ReferencedDocumentDescriptors)
            {
                if (B(() => dd.ReferenceMissing))
                {
                    naoResolvidas.Add($"{Rel(dd.FullDocumentName)}  (referenciado por {Rel(chave)})");
                    continue;
                }
                try { Coletar(dd.ReferencedDocument, chave); }
                catch (Exception ex) { naoResolvidas.Add($"{Rel(dd.FullDocumentName)}  (ERRO: {ex.Message})"); }
            }
        }

        // ---------------------------------------------------------------- ARQUIVOS
        static void Arquivos()
        {
            Escrever($"=== ARQUIVOS ({itens.Count}) ===");
            foreach (var item in itens.Values.OrderBy(i => Rel(i.Doc.FullFileName), StringComparer.OrdinalIgnoreCase))
            {
                Document d = item.Doc;
                string f = d.FullFileName;

                Escrever($"[{Tipo(d)}] {Rel(f)}");
                Comum(d);
                Escrever($"   Part Number: {Prop(d, "Design Tracking Properties", "Part Number")} | Descrição: {Prop(d, "Design Tracking Properties", "Description")}");

                if (item.NomesDocumento.Count > 1 || item.NomesDocumento.Any(n => n.Contains("<")))
                    Escrever($"   Nomes de documento: {string.Join(" | ", item.NomesDocumento)}");
                if (item.UsadoPor.Count > 0)
                    Escrever($"   Usado por: {string.Join(", ", item.UsadoPor.OrderBy(x => x))}");

                if (d is PartDocument pd) Peca(pd);
                else if (d is AssemblyDocument ad) Montagem(ad);

                Ole(d);
                Escrever("");
            }
        }

        static void Comum(Document d)
        {
            string f = d.FullFileName;
            Escrever($"   Versão salva: {V(() => d.SoftwareVersionSaved.DisplayVersion)} | Precisa migrar: {V(() => SN(d.NeedsMigrating))}");
            Escrever($"   Modificável: {V(() => SN(d.IsModifiable))} | Read-only: {V(() => SN(new IOFileInfo(f).IsReadOnly))} | Reservado: {V(() => d.ReservedForWrite ? d.ReservedForWriteName : "Não")}");
            Escrever($"   {V(() => { var fi = new IOFileInfo(f); return $"Tamanho: {fi.Length / 1024} KB | Modificado: {fi.LastWriteTime:dd/MM/yyyy HH:mm}"; })} | Caminho: {f.Length} caracteres{(f.StartsWith(@"\\") ? " | UNC" : "")}");
            Escrever($"   InternalName: {V(() => d.InternalName)} | RevisionId: {V(() => d.RevisionId)}");
        }

        static void Peca(PartDocument pd)
        {
            var def = pd.ComponentDefinition;
            Escrever($"   BOM: {V(() => def.BOMStructure.ToString())} | Content Center: {V(() => SN(def.IsContentMember))} | Membro iPart: {V(() => SN(def.IsiPartMember))} | Fábrica iPart: {V(() => SN(def.IsiPartFactory))}");
            if (B(() => def.IsiPartMember))
                Escrever($"   Fábrica do iPart: {V(() => Rel((string)((dynamic)def).iPartMember.ReferencedDocumentDescriptor.FullDocumentName))}");
            try { foreach (DerivedPartComponent c in def.ReferenceComponents.DerivedPartComponents) Escrever($"   Derivada de peça: {V(() => Rel(c.ReferencedDocumentDescriptor.FullDocumentName))}"); } catch { }
            try { foreach (DerivedAssemblyComponent c in def.ReferenceComponents.DerivedAssemblyComponents) Escrever($"   Derivada de montagem: {V(() => Rel(c.ReferencedDocumentDescriptor.FullDocumentName))}"); } catch { }
            ModelStates(def);
        }

        static void Montagem(AssemblyDocument ad)
        {
            var def = ad.ComponentDefinition;
            Escrever($"   BOM: {V(() => def.BOMStructure.ToString())} | Membro iAssembly: {V(() => SN(def.IsiAssemblyMember))} | Fábrica iAssembly: {V(() => SN(def.IsiAssemblyFactory))}");

            int total = 0, sup = 0, excl = 0, adap = 0, flex = 0, subst = 0, padr = 0;
            foreach (ComponentOccurrence o in def.Occurrences)
            {
                total++;
                if (B(() => o.Suppressed)) sup++;
                if (B(() => (bool)((dynamic)o).Excluded)) excl++;
                if (B(() => o.Adaptive)) adap++;
                if (B(() => o.Flexible)) flex++;
                if (B(() => o.IsSubstituteOccurrence)) subst++;
                if (B(() => o.IsPatternElement)) padr++;
            }
            Escrever($"   Ocorrências: {total} | Suprimidas: {sup} | Excluídas: {excl} | Adaptativas: {adap} | Flexíveis: {flex} | Substitutas: {subst} | Em padrão: {padr}");

            var rm = def.RepresentationsManager;
            Escrever($"   Vista: {V(() => rm.DesignViewRepresentations.Count.ToString())} (ativa: {V(() => rm.ActiveDesignViewRepresentation.Name)})" +
                     $" | Posicional: {V(() => rm.PositionalRepresentations.Count.ToString())} (ativa: {V(() => rm.ActivePositionalRepresentation.Name)})" +
                     $" | LOD: {V(() => Convert.ToString(((dynamic)rm).LevelOfDetailRepresentations.Count))} (ativo: {V(() => (string)((dynamic)rm).ActiveLevelOfDetailRepresentation.Name)})");
            ModelStates(def);
        }

        static void ModelStates(object def)
        {
            string n = V(() => Convert.ToString(((dynamic)def).ModelStates.Count));
            if (n != "N/D" && n != "1")
                Escrever($"   Model States: {n} (ativo: {V(() => (string)((dynamic)def).ModelStates.ActiveModelState.Name)})");
        }

        static void Ole(Document d)
        {
            try
            {
                foreach (ReferencedOLEFileDescriptor o in d.ReferencedOLEFileDescriptors)
                    Escrever($"   OLE: {Rel(o.FullFileName)}{(IOFile.Exists(o.FullFileName) ? "" : "  << NÃO ENCONTRADO")}");
            }
            catch { }
        }

        // ---------------------------------------------------------------- DETALHAMENTOS
        static void Detalhamentos()
        {
            Escrever("=== DETALHAMENTOS (mesmo nome, mesma pasta do modelo) ===");

            var desenhos = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            var semDesenho = new List<string>();
            foreach (var k in itens.Keys)
            {
                bool achou = false;
                foreach (var ext in new[] { ".idw", ".dwg" })
                {
                    string c = IOPath.ChangeExtension(k, ext);
                    if (!IOFile.Exists(c)) continue;
                    if (ext == ".dwg" && !B(() => (bool)((dynamic)inv.FileManager).IsInventorDWG(c))) continue;
                    desenhos.Add(c);
                    achou = true;
                }
                if (!achou) semDesenho.Add(Rel(k));
            }

            bool silent = inv.SilentOperation;
            inv.SilentOperation = true;
            try { foreach (var c in desenhos) Desenho(c); }
            finally { inv.SilentOperation = silent; }

            Escrever($"--- Modelos sem detalhamento ({semDesenho.Count}) ---");
            foreach (var s in semDesenho.OrderBy(x => x)) Escrever($"   {s}");
            Escrever("");
        }

        static void Desenho(string c)
        {
            Document aberto = null;
            foreach (Document x in inv.Documents)
                if (string.Equals(x.FullFileName, c, StringComparison.OrdinalIgnoreCase)) { aberto = x; break; }

            DrawingDocument dw;
            try { dw = (DrawingDocument)(aberto ?? inv.Documents.Open(c, false)); }
            catch (Exception ex)
            {
                Escrever($"[Desenho] {Rel(c)}  << ERRO AO ABRIR: {ex.Message}");
                errosDesenho.Add($"{Rel(c)}: {ex.Message}");
                Escrever("");
                return;
            }

            Escrever($"[Desenho] {Rel(c)}");
            Comum((Document)dw);

            foreach (DocumentDescriptor dd in dw.ReferencedDocumentDescriptors)
            {
                bool falta = B(() => dd.ReferenceMissing);
                Escrever($"   Ref: {Rel(dd.FullDocumentName)}{(falta ? "  << NÃO RESOLVIDA" : "")}");
                if (falta) naoResolvidas.Add($"{Rel(dd.FullDocumentName)}  (referenciado pelo desenho {Rel(c)})");
            }

            foreach (Sheet s in dw.Sheets)
            {
                Escrever($"   Folha: {s.Name} | Vistas: {s.DrawingViews.Count}");
                foreach (DrawingView v in s.DrawingViews)
                {
                    Escrever($"      {v.Name} [{V(() => v.ViewType.ToString())}] -> {V(() => Rel(v.ReferencedDocumentDescriptor.FullDocumentName))}" +
                             $" | Vista: {V(() => v.DesignViewRepresentation)}" +
                             $" | Posicional: {V(() => v.ActivePositionalRepresentation)}" +
                             $" | LOD: {V(() => (string)((dynamic)v).ActiveLevelOfDetailRepresentation)}" +
                             $" | Model State: {V(() => (string)((dynamic)v).ActiveModelState)}");
                }
            }

            Ole((Document)dw);
            if (aberto == null) try { dw.Close(true); } catch { }
            Escrever("");
        }

        // ---------------------------------------------------------------- PROBLEMAS
        static void Problemas()
        {
            Escrever($"=== REFERÊNCIAS NÃO RESOLVIDAS ({naoResolvidas.Count}) ===");
            foreach (var s in naoResolvidas.Distinct().OrderBy(x => x)) Escrever($"   {s}");
            Escrever("");

            Escrever($"=== ERROS AO ABRIR DESENHOS ({errosDesenho.Count}) ===");
            foreach (var s in errosDesenho) Escrever($"   {s}");
            Escrever("");

            Escrever("=== MESMO NOME DE ARQUIVO EM PASTAS DIFERENTES ===");
            foreach (var g in itens.Keys.GroupBy(k => IOPath.GetFileName(k), StringComparer.OrdinalIgnoreCase).Where(g => g.Count() > 1))
                Escrever($"   {g.Key}: {string.Join(" | ", g.Select(Rel))}");
            Escrever("");

            Escrever("=== MESMO InternalName EM ARQUIVOS DIFERENTES ===");
            foreach (var g in itens.Values.GroupBy(i => V(() => i.Doc.InternalName)).Where(g => g.Count() > 1))
                Escrever($"   {g.Key}: {string.Join(" | ", g.Select(i => Rel(i.Doc.FullFileName)))}");
            Escrever("");
        }

        // ---------------------------------------------------------------- RESUMO
        static void Resumo()
        {
            var docs = itens.Values.Select(i => i.Doc).ToList();
            Escrever("=== RESUMO ===");
            foreach (var g in docs.GroupBy(Tipo).OrderBy(g => g.Key)) Escrever($"   {g.Key}: {g.Count()}");
            Escrever($"   Referências não resolvidas: {naoResolvidas.Distinct().Count()}");
            Escrever($"   Precisam migrar: {docs.Count(d => B(() => d.NeedsMigrating))}");
            Escrever($"   Não modificáveis: {docs.Count(d => !B(() => d.IsModifiable))}");
            Escrever($"   Read-only: {docs.Count(d => B(() => new IOFileInfo(d.FullFileName).IsReadOnly))}");
            Escrever($"   Reservados por outro usuário: {docs.Count(d => B(() => d.ReservedForWrite && !d.ReservedForWriteName.Contains(System.Environment.UserName)))}");
            Escrever($"   Maior caminho: {docs.Max(d => d.FullFileName.Length)} caracteres");
            Escrever("   Versões salvas:");
            foreach (var g in docs.GroupBy(d => V(() => d.SoftwareVersionSaved.DisplayVersion)).OrderBy(g => g.Key))
                Escrever($"      {g.Key}: {g.Count()}");
        }

        // ---------------------------------------------------------------- AUXILIARES
        static string Tipo(Document d)
        {
            if (d is PartDocument p)
            {
                if (B(() => p.ComponentDefinition.IsContentMember)) return "Content Center";
                return p.ComponentDefinition is SheetMetalComponentDefinition ? "Chapa" : "Peça";
            }
            if (d is AssemblyDocument a)
                return a.ComponentDefinition is WeldmentComponentDefinition ? "Soldagem" : "Montagem";
            if (d is DrawingDocument) return "Desenho";
            return d.DocumentType.ToString();
        }

        static string Prop(Document d, string set, string nome) =>
            V(() => Convert.ToString(d.PropertySets[set][nome].Value));

        static string Rel(string p)
        {
            if (string.IsNullOrEmpty(p)) return p;
            return p.StartsWith(raiz + "\\", StringComparison.OrdinalIgnoreCase) ? "~" + p.Substring(raiz.Length) : p;
        }

        static string V(Func<string> f) { try { return f() ?? ""; } catch { return "N/D"; } }
        static bool B(Func<bool> f) { try { return f(); } catch { return false; } }
        static string SN(bool b) => b ? "Sim" : "Não";

        static void Escrever(string linha) { log.AppendLine(linha); }

        static void Sair(string msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine("Pressione uma tecla para sair...");
            Console.ReadKey();
        }
    }
}

