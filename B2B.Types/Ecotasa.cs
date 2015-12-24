using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Ecotasa
    {
        public string VT_PRODUCTO { get; set; }
        public string VT_CATEGORIA { get; set; }
        public string VT_DESCRIPCION { get; set; }
        public string VT_DETALLES { get; set; }
        public decimal VT_PVP1 { get; set; }
        public DateTime MODIFICADO { get; set; }
        public decimal VT_PORC_IMP { get; set; }
    }
}
