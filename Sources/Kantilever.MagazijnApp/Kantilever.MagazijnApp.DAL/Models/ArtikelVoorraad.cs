using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kantilever.MagazijnApp.Models
{
    public class ArtikelVoorraad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Artikelnummer { get; set; }
        public string Artikelnaam { get; set; }
        public int Voorraad { get; set; }
    }
}
