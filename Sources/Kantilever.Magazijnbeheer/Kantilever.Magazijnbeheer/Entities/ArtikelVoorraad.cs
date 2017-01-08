﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kantilever.Magazijnbeheer.Entities
{
    public class ArtikelVoorraad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArtikelID { get; set; }
        public int Voorraad { get; set; }
    }
}