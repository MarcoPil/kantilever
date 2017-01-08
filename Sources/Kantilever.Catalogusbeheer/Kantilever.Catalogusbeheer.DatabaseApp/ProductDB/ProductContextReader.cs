using Kantilever.Catalogusbeheer.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kantilever.Catalogusbeheer.DatabaseApp
{
    public class ProductContextReader
    {
        public static void ReadProductContext()
        {
            using (var context = new ProductContext())
            {
                var query = from product in context.Product
                            select new ArtikelAanCatalogusToegevoegd
                            {
                                Artikelnummer = product.ProdId,
                                Naam = product.ProdNaam,
                                Beschrijving = product.ProdBeschrijving,
                                Prijs = product.ProdPrijs,
                                AfbeeldingUrl = product.ProdAfbeeldingurl,
                                LeverbaarVanaf = product.ProdLeverbaarvanaf,
                                LeverbaarTot = product.ProdLeverbaartot,
                                LeverancierCode = product.ProdLeveranciersproductid,
                                Leverancier = product.ProdLev.LevNaam,
                                Categorieen = (from pc in context.ProductCategorie
                                               join cat in context.Categorie on pc.ProdcatCatId equals cat.CatId
                                               where pc.ProdcatProdId == product.ProdId
                                               select cat.CatNaam).ToList()
                            };

                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

                foreach (var evt in query)
                {
                    Console.WriteLine($"\t\tnew ArtikelAanCatalogusToegevoegd {{ Artikelnummer = {evt.Artikelnummer}, Naam = \"{evt.Naam}\", Beschrijving = \"{evt.Beschrijving}\", Prijs = {evt.Prijs.ToString()}M, AfbeeldingUrl = \"{evt.AfbeeldingUrl}\", LeverbaarVanaf = {Date(evt.LeverbaarVanaf)}, LeverbaarTot = {DateOrNull(evt.LeverbaarTot)}, LeverancierCode = \"{evt.LeverancierCode}\", Leverancier = \"{evt.Leverancier}\", Categorieen = new List<string>{{\"{String.Join("\",\"", evt.Categorieen)}\"}} }},");
                }
            }
        }

        private static string Date(DateTime date)
        {
            return $"new DateTime({date.Year}, {date.Month}, {date.Day})";
        }

        private static string DateOrNull(DateTime? date)
        {
            if (date.HasValue)
                return Date(date.Value);
            else
                return "null";
        }


    }
}