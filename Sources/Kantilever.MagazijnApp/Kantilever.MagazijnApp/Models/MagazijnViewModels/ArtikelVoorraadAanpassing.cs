using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantilever.MagazijnApp.Models.MagazijnViewModels
{
    public class ArtikelVoorraadAanpassing
    {
        public int Artikelnummer { get; set; }
        public string Artikelnaam { get; set; }
        public int Voorraad { get; set; }
        public int Aanpassing { get; set; }
    }
}
