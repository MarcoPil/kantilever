using InfoSupport.WSA.Common;

namespace Kantilever.Magazijnbeheer.Events
{
    public class ArtikelUitMagazijnGehaald : DomainEvent
    {
        public ArtikelUitMagazijnGehaald() : base("Kantilever.Magazijnbeheer.ArtikelUitMagazijnGehaald")
        {
        }

        public long ArtikelID { get; set; }
        public int Voorraad { get; set; }
    }
}