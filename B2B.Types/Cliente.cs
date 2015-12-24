using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Cliente
    {
        public DateTime? VC_HORA { get; set; }
        public int? ID { get; set; }
        public string VC_CIF { get; set; }
        public string VC_CLIENTE { get; set; }
        public string VC_NOMBRE { get; set; }
        public string VC_DENOMINACION { get; set; }
        public string VC_TFNO { get; set; }
        public string VC_FAX { get; set; }
        public string VC_DIRECCION { get; set; }
        public string VC_DIRECCIONB { get; set; }
        public string VC_POBLACION { get; set; }
        public string VC_PROVINCIA { get; set; }
        public string VC_CODPOSTAL { get; set; }
        public string VC_FORMAPAGO { get; set; }
        public string VC_ZONA { get; set; }
        public int? VC_PVP { get; set; }
        public string VC_CODFORMAPAGO { get; set; }
        public string VC_BANCO { get; set; }
        public string VC_BANCO_DIRECCION { get; set; }
        public string VC_BANCO_POBLACION { get; set; }
        public string VC_BANCO_CUENTA { get; set; }
    }
}
