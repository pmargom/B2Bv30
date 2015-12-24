using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2B.Types
{
    public class Permiso
    {
        public int idRole { get; set; }
        public string CodModulo { get; set; }
        public bool Consultar { get; set; }
        public bool Crear { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
        public Modulo Modulo { get; set; }
        public Role Rol { get; set; }

        public Permiso() { }
    }
}
