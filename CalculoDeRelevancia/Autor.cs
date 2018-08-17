using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace CalculoDeRelevancia
{
    public class Autor
    {
        public static int LastId = 0;
        public int CodigoAutor { get; set; }
        public string NomeAutor { get; set; }
        public List<PublicacaoAutor> PublicacoesAutores { get; set; }


        public Autor()
        {
            LastId++;
            CodigoAutor = LastId;
            PublicacoesAutores = new List<PublicacaoAutor>();
        }

        public void Salvar()
        {
            var comandoSQL = new StringBuilder();
            comandoSQL.Append("INSERT INTO autores (NomeAutor) ");
            comandoSQL.AppendFormat("VALUES('{0}')", MySqlHelper.EscapeString(NomeAutor));

            CodigoAutor = AcessoBanco.Instance.ExecuteNonQuery(comandoSQL.ToString());
        }

        public void SalvarVinculo()
        {
            foreach (PublicacaoAutor pa in PublicacoesAutores)
            {
                pa.CodigoAutor = CodigoAutor;
                pa.Salvar();
            }
        }

        public string RetornarInsert()
        {
            var comandoSQL = new StringBuilder();
            comandoSQL.Append("INSERT INTO autores (CodigoAutor,NomeAutor) ");
            comandoSQL.AppendFormat("VALUES({0},'{1}');", CodigoAutor, MySqlHelper.EscapeString(NomeAutor));

            return comandoSQL.ToString();
        }

        public string RetornarVinculos()
        {
            var vinculos = new StringBuilder();

            foreach (PublicacaoAutor pa in PublicacoesAutores)
            {
                pa.CodigoAutor = CodigoAutor;
                vinculos.AppendLine(pa.RetornarInsert());
            }

            return vinculos.ToString();
        }
    }
}
