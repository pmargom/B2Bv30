using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Sesion
    {
        public long? idSesion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? idUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string VC_CLIENTE { get; set; }
        public string VC_NOMBRE { get; set; }
        public int? NPedidos { get; set; }
    }
}
