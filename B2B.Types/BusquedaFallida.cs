using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class BusquedaFallida
    {
        public string Referencia { get; set; }
        public DateTime Fecha { get; set; }
        public string VC_CLIENTE { get; set; }
        public string VC_NOMBRE { get; set; }
        public string AlmacenPreferido { get; set; }
    }
}
