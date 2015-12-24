using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class ProductosPedido
    {
        public long idPedido { get; set; }
        public string VP_PRODUCTO { get; set; }
        public int VL_UNIDADES { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal VT_PORC_IMP { get; set; }
        public decimal VP_PORC_IMP { get; set; }
    }
}
