using InfoSupport.WSA.Infrastructure;
using Kantilever.Catalogusbeheer.Events;
using Kantilever.Magazijnbeheer;
using Kantilever.Magazijnbeheer.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantilever.MagazijnApp.EventBusListener
{
    public class EventListener : EventDispatcher
    {
        public EventListener(BusOptions options = null) : base(options)
        {
        }

        public void ArtikelAanCatalogusToegevoegd(ArtikelAanCatalogusToegevoegd evt)
        {
            Console.WriteLine(evt.GetType().Name);
            Console.WriteLine($"nr={evt.Artikelnummer}, naam={evt.Naam}");
        }
        public void ArtikelUitCatalogusVerwijderd(ArtikelUitCatalogusVerwijderd evt)
        {
            Console.WriteLine(evt.GetType().Name);
            Console.WriteLine(evt.Artikelnummer);
        }
        public void ArtikelInMagazijnGezet(ArtikelInMagazijnGezet evt)
        {
            Console.WriteLine(evt.GetType().Name);
            Console.WriteLine($"nr={evt.ArtikelID}, voorraad={evt.Voorraad}");
        }
        public void ArtikelInMagazijnGezet(ArtikelUitMagazijnGehaald evt)
        {
            Console.WriteLine(evt.GetType().Name);
            Console.WriteLine($"nr={evt.ArtikelID}, voorraad={evt.Voorraad}");
        }
    }
}
