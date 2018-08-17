using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeRelevancia
{
    public class PublicacaoAutor
    {
        public int CodigoPublicacao { get; set; }
        public int CodigoAutor { get; set; }

        public void Salvar()
        {
            AcessoBanco.Instance.ExecuteNonQuery(RetornarInsert());
        }

        public string RetornarInsert()
        {
            var comandoSQL = new StringBuilder();
            comandoSQL.Append("INSERT INTO publicacoes_autores (CodigoPublicacao, CodigoAutor) ");
            comandoSQL.AppendFormat("VALUES({0}, {1});", CodigoPublicacao, CodigoAutor);

            return comandoSQL.ToString();
        }

        public static int SomatorioPublicacoesSemelhantes(int codigoPublicacao, List<int> codigosSemelhantes)
        {
            List<int> codigosAutores = RetornarCodigoAutores(codigoPublicacao);

            var comandoSQL = new StringBuilder();
            comandoSQL.Append("SELECT COUNT(DISTINCT CodigoPublicacao) as Somatorio FROM publicacoes_autores ");
            comandoSQL.AppendFormat("WHERE CodigoAutor IN ({0})  AND  CodigoPublicacao IN({1})",
                                    Util.ListaIntParaString(codigosAutores), 
                                    Util.ListaIntParaString(codigosSemelhantes));

            DataTable dtResultado = AcessoBanco.Instance.ExecuteReader(comandoSQL.ToString());

            int resultado = 0;

            if (dtResultado.Rows.Count > 0)
                resultado = int.Parse(dtResultado.Rows[0]["Somatorio"].ToString());

            return resultado;
        }

        public static List<int> RetornarCodigoAutores(int codigoPublicacao)
        {
            var codigosAutores = new List<int>();
            string comandoSQL = $"SELECT CodigoAutor FROM publicacoes_autores WHERE CodigoPublicacao = {codigoPublicacao}";

            DataTable dtResultado = AcessoBanco.Instance.ExecuteReader(comandoSQL);

            foreach (DataRow dr in dtResultado.Rows)
                codigosAutores.Add(int.Parse(dr["CodigoAutor"].ToString()));

            return codigosAutores;
        }
    }
}
