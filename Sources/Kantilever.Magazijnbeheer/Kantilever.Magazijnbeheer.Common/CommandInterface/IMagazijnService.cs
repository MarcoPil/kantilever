using InfoSupport.WSA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantilever.Magazijnbeheer
{
    [Microservice("Kantilever.Magazijnbeheer")]
    public interface IMagazijnService
    {
        void ZetArtikelInMagazijn(ZetArtikelInMagazijnCommand command);
        void HaalArtikelUitMagazijn(HaalArtikelUitMagazijnCommand command);
    }
}
