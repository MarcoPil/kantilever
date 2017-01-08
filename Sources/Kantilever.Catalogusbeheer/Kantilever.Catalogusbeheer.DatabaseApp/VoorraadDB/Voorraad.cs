using System;
using System.Collections.Generic;

namespace Kantilever.Catalogusbeheer.DatabaseApp
{
    public partial class Voorraad
    {
        public int VrId { get; set; }
        public int VrProductId { get; set; }
        public int VrAantal { get; set; }
    }
}
