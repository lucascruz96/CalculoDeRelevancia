using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CalculoDeRelevancia
{
    public class Termo
    {
        public static int LastId = 0;
        public int CodigoTermo { get; set; }
        public string Texto { get; set; }
        public bool Valido { get; set; }
        public List<PublicacaoTermo> PublicacoesTermos { get; set; }

        public Termo()
        {
            LastId++;
            CodigoTermo = LastId;
            PublicacoesTermos = new List<PublicacaoTermo>();
        }

        public void Salvar()
        {
            var comandoSQL = new StringBuilder();
            comandoSQL.Append("INSERT INTO tcc.termos (Termo, Valido) ");
            comandoSQL.AppendFormat("VALUES('{0}', {1})", MySqlHelper.EscapeString(Texto), Valido);

            CodigoTermo = AcessoBanco.Instance.ExecuteNonQuery(comandoSQL.ToString());
        }

        public static List<int> RetonarCodigos(List<string> termos)
        {
            var codigos = new List<int>();

            var comandoSQL = new StringBuilder();
            comandoSQL.Append("SELECT CodigoTermo FROM tcc.termos WHERE ");
            comandoSQL.AppendFormat("Termo IN ({0})", Util.ListaParaString(termos));

            DataTable dtResultado = AcessoBanco.Instance.ExecuteReader(comandoSQL.ToString());

            foreach (DataRow dr in dtResultado.Rows)
                codigos.Add(int.Parse(dr["CodigoTermo"].ToString()));

            return codigos;
        }

        public void SalvarVinculo()
        {
            foreach(PublicacaoTermo pt in PublicacoesTermos)
            {
                pt.CodigoTermo = CodigoTermo;
                pt.Salvar();
            }
        }

        public string RetornarInsert()
        {
            var comandoSQL = new StringBuilder();
            comandoSQL.Append("INSERT INTO tcc.termos (CodigoTermo,Termo, Valido) ");
            comandoSQL.AppendFormat("VALUES({0},'{1}', {2});", CodigoTermo, MySqlHelper.EscapeString(Texto), Valido);

            return comandoSQL.ToString();
        }

        public List<string> RetornarVinculos()
        {
            var vinculos = new List<string>();

            foreach (PublicacaoTermo pt in PublicacoesTermos)
            {
                pt.CodigoTermo = CodigoTermo;
                vinculos.Add(pt.RetornarInsert());
            }

            return vinculos;
        }
    }
}
