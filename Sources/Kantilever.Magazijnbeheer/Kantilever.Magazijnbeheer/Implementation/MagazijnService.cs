using InfoSupport.WSA.Infrastructure;
using Kantilever.Magazijnbeheer.DAL;
using Kantilever.Magazijnbeheer.Entities;
using Kantilever.Magazijnbeheer.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantilever.Magazijnbeheer
{
    public class MagazijnService : IMagazijnService
    {
        private readonly DbContextOptions<MagazijnContext> _options;
        private readonly IEventPublisher _publisher;

        public MagazijnService()
        {
        }

        public MagazijnService(DbContextOptions<MagazijnContext> options, IEventPublisher publisher)
        {
            _options = options;
            _publisher = publisher;
        }

        public void HaalArtikelUitMagazijn(HaalArtikelUitMagazijnCommand command)
        {
            using (var context = new MagazijnContext(_options))
            {
                var artikel = context.Find<ArtikelVoorraad>(command.ArtikelID);

                int voorraad = artikel?.Voorraad ?? 0;
                if (voorraad < command.Aantal)
                {
                    throw new InvalidOperationException($"Er zijn {voorraad} artikelen met ID={command.ArtikelID} in het magzijn. Men kan er dan niet {command.Aantal} uithalen");
                }
                else
                {
                    artikel.Voorraad -= command.Aantal;
                }
                context.SaveChanges();
                _publisher.Publish(new ArtikelUitMagazijnGehaald() { ArtikelID = artikel.ArtikelID, Voorraad = artikel.Voorraad });
            }
        }

        public void ZetArtikelInMagazijn(ZetArtikelInMagazijnCommand command)
        {
            using (var context = new MagazijnContext(_options))
            {
                var artikel = context.Find<ArtikelVoorraad>(command.ArtikelID);
                if (artikel == null)
                {
                    context.Voorraad.Add(new ArtikelVoorraad() { ArtikelID = command.ArtikelID, Voorraad = command.Aantal });
                }
                else
                {
                    artikel.Voorraad += command.Aantal;
                }
                context.SaveChanges();
                _publisher.Publish(new ArtikelInMagazijnGezet() { ArtikelID = artikel.ArtikelID, Voorraad = artikel.Voorraad });
            }
        }
    }
}
