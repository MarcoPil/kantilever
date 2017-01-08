using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kantilever.Magazijnbeheer;
using Kantilever.MagazijnApp.Models;
using Kantilever.MagazijnApp.Models.MagazijnViewModels;

namespace Kantilever.MagazijnApp.Controllers
{
    public class MagazijnController : Controller
    {
        private readonly IMagazijnService _magazijnbeheer;

        public MagazijnController(IMagazijnService magazijnbeheer)
        {
            _magazijnbeheer = magazijnbeheer;
        }
        public IActionResult Index()
        {
            ArtikelVoorraad[] model =
            {
                new ArtikelVoorraad { Artikelnummer = 17, Artikelnaam = "mand", Voorraad = 14 },
                new ArtikelVoorraad { Artikelnummer = 19, Artikelnaam = "mand", Voorraad = 18 },
                new ArtikelVoorraad { Artikelnummer = 21, Artikelnaam = "mand", Voorraad = 22 },
            };

            return View(model);
        }

        public ActionResult VoorraadAanpassen(int id)
        {
            var model = new ArtikelVoorraadAanpassing { Artikelnummer = 17, Artikelnaam = "mand", Voorraad = 14 };
            return View(model);
        }

        [HttpPost]
        public ActionResult VoorraadAanpassen(int id, int Aanpassing)
        {
            if (Aanpassing < 0)
            {
                var command = new HaalArtikelUitMagazijnCommand { ArtikelID = id, Aantal = Math.Abs(Aanpassing) };
                _magazijnbeheer.HaalArtikelUitMagazijn(command);
            }
            else if (Aanpassing > 0)
            {
                var command = new ZetArtikelInMagazijnCommand { ArtikelID = id, Aantal = Aanpassing };
                _magazijnbeheer.ZetArtikelInMagazijn(command);
            }
            return RedirectToAction("Index");
        }
    }
}
