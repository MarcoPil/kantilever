﻿using System;
using System.Collections.Generic;

namespace Kantilever.Catalogusbeheer.DatabaseApp
{
    public partial class Leverancier
    {
        public Leverancier()
        {
            Product = new HashSet<Product>();
        }

        public int LevId { get; set; }
        public string LevNaam { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
