using Kantilever.Catalogusbeheer.Events;
using Kantilever.Magazijnbeheer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kantilever.Catalogusbeheer.DatabaseApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ProductContextReader.ReadProductContext();

            using (var context = new VoorraadContext())
            {
                var query = from voorraad in context.Voorraad
                            select new ArtikelInMagazijnGezet
                            {
                                ArtikelID = voorraad.VrProductId,
                                Voorraad = voorraad.VrAantal,
                            };

                foreach (var v in query)
                {
                    Console.WriteLine($"new ArtikelInMagazijnGezet {{ ArtikelID = {v.ArtikelID}, Voorraad = {v.Voorraad} }},");
                }
            }
        }
    }
}