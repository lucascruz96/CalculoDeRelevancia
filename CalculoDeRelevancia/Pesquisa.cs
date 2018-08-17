using System.Collections.Generic;
using System.Linq;

namespace CalculoDeRelevancia
{
    public class Pesquisa
    {
        private List<string> _termos;

        public Pesquisa(List<string> termos)
        {
            _termos = termos;
        }

        public List<PublicacaoPesquisa> Aplicar()
        {
            var pubsPesquisa = new List<PublicacaoPesquisa>();

            Dictionary<int, int> codigosOcorrencias = PublicacaoTermo.RetornarPublicacoesOcorrencias(_termos);

            List<Publicacao> publicacoes = Publicacao.ListarPorCodigos(codigosOcorrencias.Keys.ToList());

            foreach (KeyValuePair<int, int> valores in codigosOcorrencias)
            {
                var publicacaoPesquisa = new PublicacaoPesquisa(valores.Key);

                int qtPubAutores = PublicacaoAutor.SomatorioPublicacoesSemelhantes(valores.Key, codigosOcorrencias.Keys.ToList());

                publicacaoPesquisa.CalcularRelevancia(
                                        int.Parse(publicacoes.Find(p => p.CodigoPublicacao == valores.Key).CitedBy),
                                        valores.Value, qtPubAutores);

                pubsPesquisa.Add(publicacaoPesquisa);
            }

            return OrdernarResultados(pubsPesquisa);
        }

        private List<PublicacaoPesquisa> OrdernarResultados(List<PublicacaoPesquisa> pubsPesquisa)
        {
            var aux = new PublicacaoPesquisa(0);

            for (int i = 0; i < pubsPesquisa.Count - 1; i++)
            {
                for (int j = 0; j < pubsPesquisa.Count - (i + 1); j++)
                {
                    if (pubsPesquisa[j].Relevancia < pubsPesquisa[j + 1].Relevancia)
                    {
                        aux = pubsPesquisa[j];
                        pubsPesquisa[j] = pubsPesquisa[j + 1];
                        pubsPesquisa[j + 1] = aux;
                    }
                }
            }

            return pubsPesquisa;
        }
    }
}
