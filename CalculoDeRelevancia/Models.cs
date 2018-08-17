namespace Tcc
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Models : DbContext
    {
        // Your context has been configured to use a 'Models' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Tcc.Models' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Models' 
        // connection string in the application configuration file.
        public Models()
            : base("name=Models")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Publicacao> Publicacoes { get; set; }
    }

    public class Publicacao
    {
        public string Authors { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Sourcetitle { get; set; }
        public object Volume { get; set; }
        public object Issue { get; set; }
        public object ArtNo { get; set; }
        public object Pagestart { get; set; }
        public object Pageend { get; set; }
        public object Pagecount { get; set; }
        public object Citedby { get; set; }
        public object DOI { get; set; }
        public string Link { get; set; }
        public string DocumentType { get; set; }
        public string Source { get; set; }
        public string EID { get; set; }
    }
}