using System;
using System.Collections.Generic;

namespace Kantilever.Catalogusbeheer.DatabaseApp
{
    public partial class Product
    {
        public int ProdId { get; set; }
        public int ProdLevId { get; set; }
        public string ProdNaam { get; set; }
        public string ProdBeschrijving { get; set; }
        public string ProdAfbeeldingurl { get; set; }
        public decimal ProdPrijs { get; set; }
        public DateTime ProdLeverbaarvanaf { get; set; }
        public DateTime? ProdLeverbaartot { get; set; }
        public string ProdLeveranciersproductid { get; set; }

        public virtual Leverancier ProdLev { get; set; }
    }
}
