using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeRelevancia
{
    public class PublicacaoTermo
    {
        public int CodigoPublicacao { get; set; }
        public int CodigoTermo { get; set; }
        public int Ocorrencias { get; set; }

        public void Salvar()
        {
            AcessoBanco.Instance.ExecuteNonQuery(RetornarInsert());
        }

        public string RetornarInsert()
        {
            var comandoSQL = new StringBuilder();
            comandoSQL.Append("INSERT INTO publicacoes_termos (CodigoPublicacao, CodigoTermo, Ocorrencias) ");
            comandoSQL.AppendFormat("VALUES({0}, {1}, {2});", CodigoPublicacao, CodigoTermo, Ocorrencias);

            return comandoSQL.ToString();
        }

        public static List<int> RetornarCodigosPublicacoes(List<string> termos)
        {
            var codigosPublicacoes = new List<int>();
            List<int> codigosTermos = Termo.RetonarCodigos(termos);

            var comandoSQL = new StringBuilder();

            comandoSQL.Append("SELECT CodigoPublicacao FROM publicacoes_termos ");
            comandoSQL.AppendFormat("WHERE CodigoTermo IN ({0}) ", Util.ListaIntParaString(codigosTermos));
            comandoSQL.AppendFormat("GROUP BY CodigoPublicacao HAVING COUNT(CodigoTermo) > {0}", termos.Count);

            DataTable dtResultado = AcessoBanco.Instance.ExecuteReader(comandoSQL.ToString());

            foreach (DataRow dr in dtResultado.Rows)
                codigosPublicacoes.Add(int.Parse(dr["CodigoPublicacao"].ToString()));

            return codigosPublicacoes;
        }

        public static Dictionary<int, int> RetornarPublicacoesOcorrencias(List<string> termos)
        {
            var codigosPublicacoes = new Dictionary<int, int>();
            List<int> codigosTermos = Termo.RetonarCodigos(termos);

            var comandoSQL = new StringBuilder();

            comandoSQL.Append("SELECT CodigoPublicacao, SUM(Ocorrencias) AS Somatorio ");
            comandoSQL.Append("FROM publicacoes_termos ");
            comandoSQL.AppendFormat("WHERE CodigoTermo IN ({0}) ", Util.ListaIntParaString(codigosTermos));
            comandoSQL.AppendFormat("GROUP BY CodigoPublicacao HAVING COUNT(CodigoTermo) = {0}", termos.Count);

            DataTable dtResultado = AcessoBanco.Instance.ExecuteReader(comandoSQL.ToString());

            foreach (DataRow dr in dtResultado.Rows)
                codigosPublicacoes.Add(int.Parse(dr["CodigoPublicacao"].ToString()), int.Parse(dr["Somatorio"].ToString()));

            return codigosPublicacoes;
        }
    }
}
