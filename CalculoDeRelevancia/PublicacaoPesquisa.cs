
namespace CalculoDeRelevancia
{
    public class PublicacaoPesquisa
    {
        public int CodigoPublicacao { get; set; }
        public double Relevancia { get; set; }

        public PublicacaoPesquisa(int codigoPublicacao)
        {
            CodigoPublicacao = codigoPublicacao;
        }

        public void CalcularRelevancia(int citacoes, int ocorrenciasTermos, int ocorenciasAutores)
        {
            Relevancia = (citacoes * 0.2) + (ocorrenciasTermos * 0.1) + (ocorenciasAutores * 0.1);
        }
    }
}
