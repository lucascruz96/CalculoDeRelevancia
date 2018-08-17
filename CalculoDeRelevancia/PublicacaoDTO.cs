using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeRelevancia
{
    public class PublicacaoDTO : Publicacao
    {
        public string Authors { get; set; }
        public string Volume { get; set; }
        public string Issue { get; set; }
        public string ArtNo { get; set; }
        public string PageStart { get; set; }
        public string PageEnd { get; set; }
        public string PageCount { get; set; }
        public string DocumentType { get; set; }
        public string Source { get; set; }
        public string Affiliations { get; set; }
        public string AuthorsWithAffiliations { get; set; }
        public string AccessType { get; set; }


        public override void Salvar()
        {
            base.Salvar();
        }

        public override bool Equals(object obj)
        {
            var publicacao = obj as PublicacaoDTO;

            if (publicacao == null)
                return false;

            return
                Authors == publicacao.Authors &&
                Volume == publicacao.Volume &&
                Issue == publicacao.Issue &&
                ArtNo == publicacao.ArtNo &&
                PageStart == publicacao.PageStart &&
                PageEnd == publicacao.PageEnd &&
                PageCount == publicacao.PageCount &&
                DocumentType == publicacao.DocumentType &&
                Source == publicacao.Source &&
                Affiliations == publicacao.Affiliations &&
                AuthorsWithAffiliations == publicacao.AuthorsWithAffiliations &&
                AccessType == publicacao.AccessType &&
                Title == publicacao.Title &&
                Year == publicacao.Year &&
                SourceTitle == publicacao.SourceTitle &&
                CitedBy == publicacao.CitedBy &&
                DOI == publicacao.DOI &&
                Link == publicacao.Link &&
                EID == publicacao.EID &&
                Abstract == publicacao.Abstract &&
                AuthorKeywords == publicacao.AuthorKeywords;
        }

        public override int GetHashCode()
        {
            var hashCode = 607589318;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Authors);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Volume);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Issue);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ArtNo);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PageStart);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PageEnd);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PageCount);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DocumentType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Affiliations);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AuthorsWithAffiliations);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AccessType);
            return hashCode;
        }
    }
}
