using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class ClientesVsPedidos
    {
        public decimal idCliente { get; set; }
        public string VC_CLIENTE { get; set; }
        public string VC_NOMBRE { get; set; }
        public int PedidosTotales { get; set; }
        public int UnidadesTotales { get; set; }
    }
}
