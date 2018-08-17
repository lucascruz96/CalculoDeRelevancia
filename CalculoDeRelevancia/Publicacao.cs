using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CalculoDeRelevancia
{
    public class Publicacao
    {
        public int CodigoPublicacao { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string SourceTitle { get; set; }
        public string CitedBy { get; set; }
        public string DOI { get; set; }
        public string Link { get; set; }
        public string EID { get; set; }
        public string Abstract { get; set; }
        public string AuthorKeywords { get; set; }

        public static List<Publicacao> ListarPorCodigos(List<int> codigos)
        {
            var comandoSQL = $"SELECT * FROM publicacoes WHERE CodigoPublicacao IN({Util.ListaIntParaString(codigos)})";

            DataTable dtResultado = AcessoBanco.Instance.ExecuteReader(comandoSQL);

            return MontarPublicacoes(dtResultado);
        }

        public virtual void Salvar()
        {
            var comandoSQL = new StringBuilder();
            comandoSQL.Append("INSERT INTO publicacoes ");
            comandoSQL.Append("(Title, Year, SourceTitle, CitedBy, Doi, Link, Eid, Abstract, AuthorKeywords)");
            comandoSQL.Append(" VALUES(");
            comandoSQL.AppendFormat("'{0}',", MySqlHelper.EscapeString((Title == null ? "" : Title)));
            comandoSQL.AppendFormat("{0},", Year);
            comandoSQL.AppendFormat("'{0}',", MySqlHelper.EscapeString((SourceTitle == null ? "" : SourceTitle)));
            comandoSQL.AppendFormat("'{0}',", Util.StringParaInteiro(CitedBy));
            comandoSQL.AppendFormat("'{0}',", MySqlHelper.EscapeString((DOI == null ? "" : DOI)));
            comandoSQL.AppendFormat("'{0}',", MySqlHelper.EscapeString((Link == null ? "" : Link)));
            comandoSQL.AppendFormat("'{0}',", EID);
            comandoSQL.AppendFormat("'{0}',", MySqlHelper.EscapeString((Abstract == null ? "" : Abstract)));
            comandoSQL.AppendFormat("'{0}')", MySqlHelper.EscapeString((AuthorKeywords == null ? "" : AuthorKeywords)));

            CodigoPublicacao = AcessoBanco.Instance.ExecuteNonQuery(comandoSQL.ToString());
        }

        public static List<Publicacao> Listar()
        {
            string comandoSQL = "SELECT * FROM publicacoes";

            DataTable dtResultado = AcessoBanco.Instance.ExecuteReader(comandoSQL);

            return MontarPublicacoes(dtResultado);
        }

        private static List<Publicacao> MontarPublicacoes(DataTable dtResultado)
        {
            var publicacoes = new List<Publicacao>();
            Publicacao publicacao;

            foreach(DataRow dr in dtResultado.Rows)
            {
                publicacao = new Publicacao
                {
                    CodigoPublicacao = Convert.ToInt32(dr["CodigoPublicacao"].ToString()),
                    Title = dr["Title"].ToString(),
                    Year = Convert.ToInt32(dr["Year"].ToString()),
                    SourceTitle = dr["SourceTitle"].ToString(),
                    CitedBy = dr["CitedBy"].ToString(),
                    DOI = dr["DOI"].ToString(),
                    Link = dr["Link"].ToString(),
                    EID = dr["EID"].ToString(),
                    Abstract = dr["Abstract"].ToString(),
                    AuthorKeywords = dr["AuthorKeywords"].ToString()
                };

                publicacoes.Add(publicacao);
            }

            return publicacoes;
        }        

    }
}
