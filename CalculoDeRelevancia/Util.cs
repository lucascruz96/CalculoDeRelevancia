using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CalculoDeRelevancia
{
    public class Util
    {
        public static int StringParaInteiro(string texto)
        {
            int result;

            int.TryParse(texto, out result);

            return result;
        }

        public static string LerArquivo(string caminho)
        {
            var arquivo = new StreamReader(caminho);

            string conteudo = arquivo.ReadToEnd();

            arquivo.Close();

            return conteudo;
        }

        public static List<string> ListarArquivos()
        {
            return Directory.GetFiles($@"{Environment.CurrentDirectory}\bases\", "*.json").ToList();
        }

        public static string ListaParaString(List<string> lista)
        {
            var resultado = new StringBuilder();

            foreach (string s in lista)
                resultado.Append((resultado.Length > 0 ? $",'{s}'" : $"'{s}'"));

            return resultado.ToString();
        }

        public static string ListaIntParaString(List<int> lista)
        {
            var resultado = new StringBuilder();

            foreach (int s in lista)
                resultado.Append((resultado.Length > 0 ? $",{s}" : $"{s}"));

            return resultado.ToString();
        }
    }
}
