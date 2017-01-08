using InfoSupport.WSA.Common;

namespace Kantilever.Magazijnbeheer
{
    public class ArtikelInMagazijnGezet : DomainEvent
    {
        public ArtikelInMagazijnGezet() : base("Kantilever.Magazijnbeheer.ArtikelInMagazijnGezet")
        {
        }

        public int ArtikelID { get; set; }
        public int Voorraad { get; set; }
    }
}