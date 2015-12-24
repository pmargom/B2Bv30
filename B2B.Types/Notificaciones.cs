using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace B2B.Types
{
    public class Notificaciones
    {
        public bool? pedidoConfirmado { get; set; }
        public bool? pedidoConDatos { get; set; }
    }
}
