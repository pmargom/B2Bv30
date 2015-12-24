using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Factura
    {
        public DateTime VF_HORA { get; set; }
        public int ID { get; set; }
        public string VF_CLIENTE { get; set; }
        public string VF_NOMBRE_CLIENTE { get; set; }
        public string VF_FACTURA { get; set; }
        public DateTime? VF_FECHA_FACT { get; set; }
        public string VF_ALBARAN { get; set; }
        public DateTime? VF_FECHA_ALB { get; set; }
        public string VF_ARTICULO {get; set; }
        public string VF_DESCARTICULO {get; set; }
        public string VF_CATEGORIA {get; set; }
        public decimal VF_UNIDADES  {get; set; }
        public decimal VF_PVENTA  {get; set; }
        public decimal VF_PORC_DCTO  {get; set; }
        public decimal VF_IMP_DCTO  {get; set; }
        public decimal VF_NETO  {get; set; }
        public decimal VF_PORC_IGIC  {get; set; }
        public decimal IMP_BRUTO { get; set; }
        public decimal DTO { get; set; }
        public decimal IGIC { get; set; }
        public string NFU { get; set; }
        public decimal TOTAL { get; set; }
        public string FicheroFactura { get; set; }
        public string FicheroAlbaran { get; set; }
        public decimal VF_LINEA { get; set; }
    }   
}
