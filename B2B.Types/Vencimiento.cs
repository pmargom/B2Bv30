using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Vencimiento
    {
        public int ID { get; set; }
        public decimal VE_CLIENTE { get; set; }
        public string VE_FACTURA { get; set; }
        public long VE_DOCUMENTO { get; set; }
        public string VE_TIPO_DOC { get; set; }
        public DateTime VE_EMISION { get; set; }
        public DateTime VE_VENCIMIENTO { get; set; }
        public decimal VE_IMPORTE { get; set; }
        public DateTime MODIFICADO { get; set; }
        public string Fichero { get; set; }
        public decimal VE_ESTADO { get; set; }
    }
}
