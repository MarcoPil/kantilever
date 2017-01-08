using System.Collections.Generic;
using System;
using InfoSupport.WSA.Common;

namespace Kantilever.Catalogusbeheer.Events
{
    public class ArtikelUitCatalogusVerwijderd : DomainEvent
    {
        public ArtikelUitCatalogusVerwijderd() : base("Kantilever.Catalogusbeheer.ArtikelUitCatalogusVerwijderd")
        {

        }
        public int Artikelnummer { get; set; }
    }
}