using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Mensaje
    {
        public int idMensaje { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int RemitenteIdUsuario { get; set; }
        public string Remitente { get; set; }
        public int DestinatarioIdUsuario { get; set; }
        public string Destinatario { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public bool? Leido { get; set; }
        public Buzon Buzon { get; set; }
        public string TipoUser { get; set; }
        public bool EnviarATodos { get; set; }
    }
}
