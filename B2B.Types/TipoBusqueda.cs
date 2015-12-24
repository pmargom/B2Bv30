using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public enum TipoArticulo
    {
        OTRO = 0,
        LLANTA = 1,
        NEUMATICO = 2,
        LUBRICANTE = 3
    }

    public enum TipoNeumatico
    {
        NONEUMATICO = 0,
        TURISMO = 1,
        TODOTERRENO = 2,
        COMERCIAL = 3,
        CAMION = 4,
        MOTO = 5
    }

    public class TipoBusqueda
    {
        public bool Llantas { get; set; }
        public bool Lubricante { get; set; }
        public bool Otros { get; set; }
        public bool Neumatico { get; set; }
        public List<TipoNeumatico> TipoNeumatico { get; set; }
    }

}
