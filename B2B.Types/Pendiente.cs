using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Pendiente
    {
        public DateTime MODIFICADO { get; set; }
        public string VL_ARTICULO { get; set; }
        public DateTime VL_LLEGADA { get; set; }
        public string VL_CONTENEDOR { get; set; }
    }
}
