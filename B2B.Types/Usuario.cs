using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace B2B.Types
{
    public enum TipoUsuario
    {
        Cliente_Supervisor = 0,
        Staff = 1,
        Administrador = 2,
        Cliente_Comercial = 3,
        Cliente_Administrativo = 4
    }

    public class Usuario
    {
        public int? idUsuario { get; set; }
        public TipoUsuario tipo { get; set; }
        public string login { get; set; }
        public string pass { get; set; }
        public bool? Bloqueado { get; set; }
        public string VC_CLIENTE { get; set; }
        public string VC_DENOMINACION { get; set; }
        public Cliente cliente { set; get; }
        public bool? MostrarMinuaturas { get; set; }
        public bool? GenerarPdfPedido { get; set; }
        public bool? NotificarPedido { get; set; }
        public string Email { get; set; }
        public bool? ConfirmarAuto { get; set; }
        public string AlmacenPreferido { get; set; }
        public int? NRuedasPreselec { get; set; }
        public decimal DtoB2B { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        public List<Permiso> Permisos { get; set; }
        public TipoBusqueda TipoBusqueda { get; set; }
        public long idSesion { get; set; }
    }
}
