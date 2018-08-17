using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SF.Snowball.Ext;

namespace CalculoDeRelevancia
{
    class Program
    {
        static void Main(string[] args)
        {
            //CarregarDados();

            PesquisarDados();

            Console.Write("Finalizado!");
            Console.ReadKey();
        }

        private static void PesquisarDados()
        {
            Console.WriteLine("Insira os termos de busca: ");
            string tema = Console.ReadLine();

            Console.WriteLine($"[{DateTime.Now}] Carregando stopwords...");
            List<string> stopWords = CarregarStopWords();

            List<string> termos = PegarPalavras(tema);

            termos = TratarPalavras(termos, stopWords);

            var pesquisa = new Pesquisa(termos);

            var resultado = pesquisa.Aplicar();

            ImprimirResultado(resultado);
        }

        private static void ImprimirResultado(List<PublicacaoPesquisa> resultado)
        {
            int indice = 0;

            Console.WriteLine("Código Publicação\t | \t Relevância");

            while (indice < 10 && indice < resultado.Count)
            {
                Console.WriteLine($"{resultado[indice].CodigoPublicacao}\t |\t {resultado[indice].Relevancia}");

                indice++;
            }
        }

        private static void CarregarDados()
        {
            Console.WriteLine($"[{DateTime.Now}] Carregando stopwords...");
            List<string> stopWords = CarregarStopWords();

            Console.WriteLine($"[{DateTime.Now}] Carregando publicações...");
            List<PublicacaoDTO> publicacoesDto = CarregarPublicacoes();

            Console.WriteLine($"[{DateTime.Now}] Salvando Publicações...");
            foreach (PublicacaoDTO publicacao in publicacoesDto)
                publicacao.Salvar();

            Console.WriteLine($"[{DateTime.Now}] Extraindo Autores...");
            ExtrairAutores(publicacoesDto);

            publicacoesDto.Clear();
            List<Publicacao> publicacoes = Publicacao.Listar();

            Console.WriteLine("Extraindo termos...");
            ExtrairTermos(publicacoes, stopWords);
        }

        private static void ExtrairAutores(List<PublicacaoDTO> publicacoes)
        {
            var autores = new List<Autor>();
            Autor autor;
            int i = 1;

            Console.WriteLine($"[{DateTime.Now}] Extraindo autores - Publicação {i} de {publicacoes.Count}.");

            foreach (PublicacaoDTO pub in publicacoes)
            {
                if (i % 1000 == 0)
                    Console.WriteLine($"[{DateTime.Now}] Extraindo autores - Publicação {i} de {publicacoes.Count}.");

                var palavras = PegarPalavras(pub.Authors, ',');
                int posAutor = -1;

                foreach (string p in palavras)
                {
                    posAutor = autores.FindIndex(a => a.NomeAutor == p.Trim());

                    if (posAutor >= 0)
                        autores[posAutor].PublicacoesAutores.Add(new PublicacaoAutor { CodigoPublicacao = pub.CodigoPublicacao });
                    else
                    {
                        autor = new Autor { NomeAutor = p.Trim() };
                        autor.PublicacoesAutores.Add(new PublicacaoAutor { CodigoPublicacao = pub.CodigoPublicacao });

                        autores.Add(autor);
                    }
                }

                i++;
            }

            SalvarAutores(autores);
        }

        private static void SalvarAutores(List<Autor> autores)
        {
            int i = 1;

            Console.WriteLine($"[{DateTime.Now}] Salvando autores...");
            Console.WriteLine($"[{DateTime.Now}] Salvando autor - Autor {i} de {autores.Count}");
            foreach (Autor autor in autores)
            {
                if (i % 1000 == 0)
                    Console.WriteLine($"[{DateTime.Now}] Salvando autores - Autor {i} de {autores.Count}");

                autor.Salvar();
                autor.SalvarVinculo();

                i++;
            }
        }

        private static void ExtrairTermos(List<Publicacao> publicacoes, List<string> stopWords)
        {
            var termos = new List<Termo>();
            Termo termo;
            int i = 1;

            Console.WriteLine($"[{DateTime.Now}] Extraindo termos - Publicação {i} de {publicacoes.Count}.");

            foreach (Publicacao pub in publicacoes)
            {
                if (i % 1000 == 0)
                    Console.WriteLine($"[{DateTime.Now}] Extraindo termos - Publicação {i} de {publicacoes.Count}.");

                var palavras = new List<string>();
                int posTermo = -1;
                int posPubTermo = -1;
                palavras.AddRange(PegarPalavras(TratarTexto(pub.Title)));
                palavras.AddRange(PegarPalavras(TratarTexto(pub.Abstract)));
                palavras.AddRange(PegarPalavras(TratarTexto(pub.AuthorKeywords)));

                palavras = TratarPalavras(palavras, stopWords);

                foreach (string p in palavras)
                {
                    posTermo = termos.FindIndex(t => t.Texto == p);

                    if (posTermo >= 0)
                    {
                        posPubTermo = termos[posTermo].PublicacoesTermos.FindIndex(pt => pt.CodigoPublicacao == pub.CodigoPublicacao);

                        if (posPubTermo >= 0)
                            termos[posTermo].PublicacoesTermos[posPubTermo].Ocorrencias++;
                        else
                            termos[posTermo].PublicacoesTermos.Add(new PublicacaoTermo { CodigoPublicacao = pub.CodigoPublicacao, Ocorrencias = 1 });
                    }
                    else
                    {
                        termo = new Termo { Texto = p, Valido = true };
                        termo.PublicacoesTermos.Add(new PublicacaoTermo { CodigoPublicacao = pub.CodigoPublicacao, Ocorrencias = 1 });

                        termos.Add(termo);
                    }
                }

                i++;
            }

            SalvarTermos(termos);
        }

        private static List<string> TratarPalavras(List<string> palavras, List<string> stopWords)
        {
            var palavrasTratadas = new List<string>();
            string textoTratado = string.Empty;

            foreach (var p in palavras)
            {
                textoTratado = TratarTermo(p);

                if (!string.IsNullOrWhiteSpace(textoTratado) && !stopWords.Contains(textoTratado))
                    palavrasTratadas.Add(textoTratado);
            }

            return palavrasTratadas;
        }

        private static List<string> RemoverStopWords(List<string> palavras, List<string> stopWords)
        {
            palavras.RemoveAll(p => stopWords.Contains(p));
            palavras.TrimExcess();

            return palavras;
        }

        private static void SalvarTermos(List<Termo> termos)
        {
            int i = 1;

            Console.WriteLine($"[{DateTime.Now}] Salvando termos...");
            Console.WriteLine($"[{DateTime.Now}] Salvando termos - Termo {i} de {termos.Count}");
            foreach (Termo termo in termos)
            {
                if (i % 1000 == 0)
                    Console.WriteLine($"[{DateTime.Now}] Salvando termos - Termo {i} de {termos.Count}");

                termo.Salvar();
                termo.SalvarVinculo();

                i++;
            }
        }

        private static List<string> PegarPalavras(string texto, char separador = ' ')
        {
            if (string.IsNullOrWhiteSpace(texto))
                return new List<string>();
            else
                return texto.Split(separador).ToList();
        }

        private static string TratarTexto(string texto)
        {
            string textoTratado = texto;
            textoTratado = Regex.Replace(textoTratado, "[.:/,\"“”∈‘’'λ;?±σ!©@#$%¨&*¬¢£§_~º°ª><≥=+()\\[\\]{}→×-]", " ");

            textoTratado = Regex.Replace(textoTratado, " {2,}", " ");

            return textoTratado;
        }

        private static List<PublicacaoDTO> CarregarPublicacoes()
        {
            var publicacoes = new List<PublicacaoDTO>();
            List<string> arquivos = Util.ListarArquivos();

            foreach (string arq in arquivos)
            {
                Console.WriteLine($"[{DateTime.Now}] Carregando arquivo {arq}");

                List<PublicacaoDTO> artigos = CarregarArquivo(arq);

                foreach (PublicacaoDTO pub in artigos)
                    if (!publicacoes.Contains(pub))
                        publicacoes.Add(pub);
            }

            return publicacoes;
        }

        private static List<PublicacaoDTO> CarregarArquivo(string arq)
        {
            string conteudo = Util.LerArquivo(arq);

            return JsonConvert.DeserializeObject<List<PublicacaoDTO>>(conteudo);
        }

        private static List<string> CarregarStopWords()
        {
            string conteudo = Util.LerArquivo("stopwords_en.txt");

            return conteudo.Split(';').ToList();
        }

        static string TratarTermo(string texto)
        {
            string textoStemmizado = AplicarStemming(texto.ToLower().Trim());

            return textoStemmizado;
        }

        private static string AplicarStemming(string texto)
        {
            var stemmer = new EnglishStemmer();
            stemmer.SetCurrent(texto);
            stemmer.Stem();
            return stemmer.GetCurrent();
        }
    }
}
