using InfoSupport.WSA.Common;

namespace Kantilever.Magazijnbeheer
{
    public class ArtikelInMagazijnGezet : DomainEvent
    {
        public ArtikelInMagazijnGezet() : base("Kantilever.Magazijnbeheer.ArtikelInMagazijnGezet")
        {
        }

        public long ArtikelID { get; set; }
        public int Voorraad { get; set; }
    }
}