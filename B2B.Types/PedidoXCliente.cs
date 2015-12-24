using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class PedidoXCliente
    {
        public int idEstado { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public long idPedido { get; set; }
        public string Referencia { get; set; }
        public DateTime Fecha { get; set; }
        public decimal idCliente { get; set; }
        public string VC_CLIENTE { get; set; }
        public string VC_NOMBRE { get; set; }
        public string VC_DENOMINACION { get; set; }
        public int Unidades { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImporteDescuento { get; set; }
        public decimal PrecioConDescuento { get; set; }
        public decimal NFU { get; set; }
        public decimal ImporteIGIC { get; set; }
        public decimal Total { get; set; }
    }
}
