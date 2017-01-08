using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantilever.Magazijnbeheer;
using InfoSupport.WSA.Infrastructure;

namespace Kantilever.MagazijnApp.Infrastructure
{
    public class MagazijnService : IMagazijnService, IDisposable
    {
        private readonly MicroserviceProxy _proxy;

        public MagazijnService(BusOptions busOptions)
        {
            var magazijnbeheerEndpoint = Environment.GetEnvironmentVariable("magazijnbeheer-endpoint");
            _proxy = new MicroserviceProxy(magazijnbeheerEndpoint, busOptions);
        }

        public void HaalArtikelUitMagazijn(HaalArtikelUitMagazijnCommand command)
        {
            _proxy.Execute(command);
        }

        public void ZetArtikelInMagazijn(ZetArtikelInMagazijnCommand command)
        {
            _proxy.Execute(command);
        }

        public void Dispose()
        {
            _proxy?.Dispose();
        }
    }
}
