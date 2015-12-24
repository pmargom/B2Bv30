using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Promocion
    {
        public int? idPromo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Activa { get; set; }
        public string BannerPeq { get; set; }
        public string BannerGra { get; set; }
        public TipoBusqueda TipoBusqueda { get; set; }
    }
}
