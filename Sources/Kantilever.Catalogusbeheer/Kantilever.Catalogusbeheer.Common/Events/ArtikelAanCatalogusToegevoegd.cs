using System.Collections.Generic;
using System;
using InfoSupport.WSA.Common;

namespace Kantilever.Catalogusbeheer.Events
{
    public class ArtikelAanCatalogusToegevoegd: DomainEvent
    {
        public ArtikelAanCatalogusToegevoegd() : base("Kantilever.Catalogusbeheer.ArtikelAanCatalogusToegevoegd")
        {

        }
        public int Artikelnummer { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public decimal Prijs { get; set; }
        public string AfbeeldingUrl { get; set; }
        public DateTime LeverbaarVanaf { get; set; }
        public DateTime? LeverbaarTot { get; set; }
        public string LeverancierCode { get; set; }
        public string Leverancier { get; set; }
        public IList<string> Categorieen { get; set; }
    }
}