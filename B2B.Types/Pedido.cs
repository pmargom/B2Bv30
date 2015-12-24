using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public enum EstadoPedido
    {
        ENA = 1, // Enviado a Neumáticos Atlántico
        RNA = 2, // Recibido en Neumáticos Atlántico
        PRE = 3, // En preparación
        LPE = 4, // Listo para envío
        ENV = 5, // Enviado
        ENT = 6  // Entregado
    }

    public class Pedido
    {
        public int idEstado { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public long idPedido { get; set; }
        public string Referencia { get; set; }
        public string VF_AlBARAN { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public bool? PorAgencia { get; set; }
        public string DirEnvio { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImporteDescuento { get; set; }
        public decimal PrecioConDescuento { get; set; }
        public decimal NFU { get; set; }
        public decimal IGIC { get; set; }
        public decimal ImporteIGIC { get; set; }
        public decimal Total { get; set; }
        public string Fichero { get; set; }
        public List<Producto> Productos { get; set; }
        public Cliente Cliente { get; set; }
        public string VC_CLIENTE { get; set; }
        public string Observaciones { get; set; } 
    }
}
