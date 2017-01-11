using InfoSupport.WSA.Infrastructure;
using Kantilever.Magazijnbeheer.Commands;
using Kantilever.Magazijnbeheer.DAL;
using Kantilever.Magazijnbeheer.Entities;
using Kantilever.Magazijnbeheer.Events;
using Kantilever.Magazijnbeheer.Shared;
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

        public MagazijnService(DbContextOptions<MagazijnContext> options, IEventPublisher publisher)
        {
            _options = options;
            _publisher = publisher;
        }

        public void HaalArtikelUitMagazijn(HaalArtikelUitMagazijnCommand command)
        {
            Console.WriteLine($"Execute HaalArtikelUitMagazijnCommand(id={command.ArtikelID},aantal={command.Aantal})");
            using (var context = new MagazijnContext(_options))
            {
                var artikel = context.Find<ArtikelVoorraad>(command.ArtikelID);
                if (artikel == null)
                {
                    throw new ArtikelOnbekendException($"Artikel (met artikelnummer={command.ArtikelID}) is niet aanwezig in magazijn.");
                }
                else
                {
                    int voorraad = artikel.Voorraad;
                    if (voorraad < command.Aantal)
                    {
                        throw new OnvoldoendeVoorraadException($"Er zijn {voorraad} artikelen met ID={command.ArtikelID} in het magzijn. Men kan er dan niet {command.Aantal} uithalen");
                    }
                    else
                    {
                        artikel.Voorraad -= command.Aantal;
                    }
                    context.SaveChanges();
                    _publisher.Publish(new ArtikelUitMagazijnGehaald() { ArtikelID = artikel.ArtikelID, Voorraad = artikel.Voorraad });
                }
            }
        }

        public void ZetArtikelInMagazijn(ZetArtikelInMagazijnCommand command)
        {
            Console.WriteLine($"Execute ZetArtikelInMagazijnCommand(id={command.ArtikelID},aantal={command.Aantal})");
            using (var context = new MagazijnContext(_options))
            {
                var artikel = context.Find<ArtikelVoorraad>(command.ArtikelID);
                if (artikel == null)
                {
                    artikel = new ArtikelVoorraad() { ArtikelID = command.ArtikelID, Voorraad = command.Aantal };
                    context.Voorraad.Add(artikel);
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
