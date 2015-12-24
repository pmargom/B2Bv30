
using B2B.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace B2Bv30
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        public Service()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        #region JSON WEBMETHODS

        #region USUARIOS

        //[WebMethod]
        //public List<Combo> listaTiposUsuarioJS()
        //{
        //    return (new Facades.Facades()).listaTiposUsuario();
        //}

        //[WebMethod]
        //public List<Combo> listaUsuariosComboJS(string tipo)
        //{
        //    return (new Facades.Facades()).listaUsuariosCombo(tipo);
        //}

        //[WebMethod]
        //public List<Combo> listaRolesComboJS()
        //{
        //    return new Facades.Facades().listaRolesCombo();
        //}

        //[WebMethod]
        //public List<Usuario> UsuariosDataJS(bool? showAdmin, bool? showStaff, bool? showClientes, bool? bloqueado)
        //{
        //    return (new Facades.Facades()).UsuariosData(showAdmin, showStaff, showClientes, bloqueado);
        //}

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoginJS(string userName, string pass)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Usuario user = new Facades.Facades().Login(userName, pass);

                var jsonResult = new
                {
                    idUsuario = user.idUsuario,
                    login = user.login,
                    idSesion = user.idSesion,
                    bloqueado = user.Bloqueado,
                    email = user.Email
                    //Bloqueado 
                    //public string VC_CLIENTE { get; set; }
                    //public string VC_DENOMINACION { get; set; }
                    //public Cliente cliente { set; get; }
                    //public bool? MostrarMinuaturas { get; set; }
                    //public bool? GenerarPdfPedido { get; set; }
                    //public bool? NotificarPedido { get; set; }
                    //public string Email { get; set; }
                    //public bool? ConfirmarAuto { get; set; }
                    //public string AlmacenPreferido { get; set; }
                    //public int? NRuedasPreselec { get; set; }
                    //public decimal DtoB2B { get; set; }
                    //public DateTime? UltimoAcceso { get; set; }

                    //public List<Permiso> Permisos { get; set; }
                    //public TipoBusqueda TipoBusqueda { get; set; }
                    //public long idSesion { get; set; }

                };

                //Context.Response.Write(js.Serialize(jsonResult));

                Context.Response.Write(js.Serialize(user));

            }
            catch (Exception ex)
            {
                var exResult = new
                {
                    menssage = ex.Message
                };
                Context.Response.Write(js.Serialize(exResult));
            }
        }

        [WebMethod]
        //[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void LogoutJS(long idSesion)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                var result = new Facades.Facades().Logout(idSesion);
                var jsResult = new
                {
                    affectedRows = result
                };
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(js.Serialize(jsResult));
            }
            catch (Exception ex)
            {
                var exResult = new
                {
                    menssage = ex.Message
                };
                Context.Response.Write(js.Serialize(exResult));
            }
        }

        //[WebMethod]
        //public string LoginOutJS(string userName, string pass)
        //{
        //    try
        //    {
        //        Usuario user = (new Facades.Facades()).Login(userName, pass);
        //        if (user == null) return null;

        //        string cadena = string.Format("{0};{1}", userName, pass);
        //        UnicodeEncoding s = new UnicodeEncoding();

        //        return Convert.ToBase64String(s.GetBytes(cadena));
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //[WebMethod]
        //public string UsuariosGuardarJS(int? idUsuario, string login, string pass, int? tipo, string VC_CLIENTE, bool? bloqueado,
        //                string AlmacenPreferido, bool? ConfirmarAuto, string Email, bool? GenerarPdfPedido, bool? MostrarMinuaturas,
        //                bool? NotificarPedido, int? NRuedasPreselec, decimal DtoB2B, bool EnviarCreadenciaesPorEmail, TipoBusqueda TipoBusqueda)
        //{
        //    return (new Facades.Facades()).UsuariosGuardar(idUsuario, login, pass, tipo, VC_CLIENTE, bloqueado, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
        //                                             MostrarMinuaturas, NotificarPedido, NRuedasPreselec, DtoB2B, EnviarCreadenciaesPorEmail, TipoBusqueda);
        //}

        //[WebMethod]
        //public string UsuariosEliminarJS(int idUsuario)
        //{
        //    return (new Facades.Facades()).UsuariosEliminar(idUsuario);
        //}

        //[WebMethod]
        //public string UsuarioTipoBusquedaGuardarJS(int idUsuario, TipoBusqueda tipoBusqueda)
        //{
        //    return (new Facades.Facades()).UsuarioTipoBusquedaGuardar(idUsuario, tipoBusqueda);
        //}

        //[WebMethod]
        //public string PerfilUsuarioGuardarJS(int? idUsuario, string AlmacenPreferido, bool? ConfirmarAuto, string Email,
        //                    bool? GenerarPdfPedido, bool? MostrarMinuaturas, bool? NotificarPedido, int? NRuedasPreselec)
        //{
        //    return (new Facades.Facades()).PerfilUsuarioGuardar(idUsuario, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
        //                                             MostrarMinuaturas, NotificarPedido, NRuedasPreselec);
        //}

        //[WebMethod]
        //public int CambiarPasswordJS(string login, string pass, string newPass)
        //{
        //    return new Facades.Facades().CambiarPassword(login, pass, newPass);
        //}

        #endregion

        #region FAMILIAS LOGOS

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public void FamiliasLogoDataJS(Familia familia)
        public void FamiliasLogoDataJS()
        {
            Familia familia = new Familia();
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                var result = new Facades.Facades().FamiliasLogoData(familia);
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(js.Serialize(result));
            }
            catch (Exception ex)
            {
                var exResult = new
                {
                    menssage = ex.Message
                };
                Context.Response.Write(js.Serialize(exResult));
            }
        }

        //[WebMethod]
        //public string FamiliasLogoGuardar(Familia familia)
        //{
        //    return new Facades.Facades().FamiliasLogoGuardar(familia);
        //}

        //[WebMethod]
        //public string FamiliasLogoEliminar(string VF_FAMILIA)
        //{
        //    return new Facades.Facades().FamiliasLogoEliminar(VF_FAMILIA);
        //}

        #endregion

        #endregion

        #region SOAP WEBMETHODS

        #region MENSAJES ENVIADOS

        [WebMethod]
        public string MensajeMarcarComoLeido(int idMensaje)
        {
            return new Facades.Facades().MensajeMarcarComoLeido(idMensaje);
        }

        [WebMethod]
        public int MensajeGuardar(Mensaje mensaje)
        {
            return new Facades.Facades().MensajeGuardar(mensaje);
        }

        [WebMethod]
        public List<Mensaje> MensajesData(Mensaje mensaje)
        {
            return new Facades.Facades().MensajesData(mensaje);
        }

        #endregion

        #region FAMILIAS LOGOS

        [WebMethod]
        public List<Familia> FamiliasLogoData(Familia familia)
        {
            return new Facades.Facades().FamiliasLogoData(familia);
        }

        [WebMethod]
        public string FamiliasLogoGuardar(Familia familia)
        {
            return new Facades.Facades().FamiliasLogoGuardar(familia);
        }

        [WebMethod]
        public string FamiliasLogoEliminar(string VF_FAMILIA)
        {
            return new Facades.Facades().FamiliasLogoEliminar(VF_FAMILIA);
        }

        #endregion

        #region SESIONES

        [WebMethod]
        public List<Sesion> SesionesData(Sesion sesion)
        {
            return new Facades.Facades().SesionesData(sesion);
        }

        #endregion

        #region CLIENTES VS PEDIDOS

        [WebMethod]
        public List<ClientesVsPedidos> ClientesVsPedidosData(DateTime? FechaDesde, DateTime? FechaHasta, string clienteDesde, string clienteHasta)
        {
            return new Facades.Facades().ClientesVsPedidosData(FechaDesde, FechaHasta, clienteDesde, clienteHasta);
        }

        #endregion

        #region PEDIDOS POR CLIENTE

        [WebMethod]
        public List<PedidoXCliente> PedidosXClientesData(DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            return new Facades.Facades().PedidosXClientesData(FechaDesde, FechaHasta, VC_CLIENTE);
        }

        #endregion

        #region BUSQUEDAS FALLIDAS

        [WebMethod]
        public List<BusquedaFallida> BusquedasFallidasData(string Referencia, DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            return new Facades.Facades().BusquedasFallidasData(Referencia, FechaDesde, FechaHasta, VC_CLIENTE);
        }

        [WebMethod]
        public int BusquedasFallidasGuardar(string Referencia, string VC_CLIENTE)
        {
            return new Facades.Facades().BusquedasFallidasGuardar(Referencia, VC_CLIENTE);
        }

        [WebMethod]
        public int BusquedasFallidasEliminar(string Referencia)
        {
            return new Facades.Facades().BusquedasFallidasEliminar(Referencia);
        }

        #endregion

        #region CONFIGURACION

        [WebMethod]
        public List<ParametroConfig> ConfigData(string parametro)
        {
            return (new Facades.Facades()).ConfigData(parametro);
        }

        [WebMethod]
        public int ConfiguracionGuardar(string parametro, string valor)
        {
            return new Facades.Facades().ConfiguracionGuardar(parametro, valor);
        }

        #endregion

        #region FACTURAS

        [WebMethod]
        public List<Factura> FacturasData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return (new Facades.Facades()).FacturasData(cliente, fechaDesde, fechaHasta);
        }

        [WebMethod]
        public List<Factura> FacturasDataDesglosada(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return (new Facades.Facades()).FacturasDataDesglosada(cliente, fechaDesde, fechaHasta);
        }

        [WebMethod]
        public List<Factura> FacturaData(int idFactura)
        {
            return (new Facades.Facades()).FacturaData(idFactura);
        }

        [WebMethod]
        public List<Factura> FacturasGuardar(string cliente, string nombre_cliente, string factura, DateTime? fecha_fact, string albaran
                                 , DateTime? fecha_alb, string articulo, string descarticulo, string categoria, decimal unidades
                                 , decimal pventa, decimal porc_dcto, decimal imp_dcto, decimal neto, decimal porc_igic, decimal VF_LINEA)
        {
            return (new Facades.Facades()).FacturasGuardar(cliente, nombre_cliente, factura, fecha_fact, albaran, fecha_alb, articulo, descarticulo, categoria, unidades, pventa, porc_dcto, imp_dcto, neto, porc_igic, VF_LINEA);
        }

        //[WebMethod]
        //public int FacturaActualizarFicheropruebaPDF()
        //{
        //    return (new Facades.Facades()).FacturaActualizarFichero("FV13003576");
        //}

        [WebMethod]
        public int FacturaActualizarFichero(string VF_FACTURA)
        {
            return (new Facades.Facades()).FacturaActualizarFichero(VF_FACTURA);
        }

        [WebMethod]
        public int FacturasBorrarSiExiste(string factura, string albaran, decimal VF_LINEA)
        {
            return new Facades.Facades().FacturasBorrarSiExiste(factura, albaran, VF_LINEA);
        }

        [WebMethod]
        public int FacturasPorClienteBorrar(string cliente)
        {
            return new Facades.Facades().FacturasPorClienteBorrar(cliente);
        }

        #endregion

        #region ALBARANES

        [WebMethod]
        public List<Factura> AlbaranesData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta, bool? showAll)
        {
            return (new Facades.Facades()).AlbaranesData(cliente, fechaDesde, fechaHasta, showAll);
        }

        [WebMethod]
        public int AlbaranActualizarFichero(string VF_ALBARAN)
        {
            return (new Facades.Facades()).AlbaranActualizarFichero(VF_ALBARAN);
        }

        #endregion

        #region VENCIMIENTOS

        [WebMethod]
        public List<Vencimiento> VencimientosData(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return (new Facades.Facades()).VencimientosData(cliente, fechaDesde, fechaHasta);
        }

        [WebMethod]
        public List<Vencimiento> VencimientosDataDesglosada(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return (new Facades.Facades()).VencimientosDataDesglosada(cliente, fechaDesde, fechaHasta);
        }

        [WebMethod]
        public List<Vencimiento> VencimientosGuardar(decimal cliente, string factura, long documento, string tipo_doc,
                               DateTime emision, DateTime vencimiento, decimal importe, decimal estado)
        {
            return (new Facades.Facades()).VencimientosGuardar(cliente, factura, documento, tipo_doc, emision, vencimiento, importe, estado);
        }

        [WebMethod]
        public int EfectoActualizarFichero(string VF_DOCUMENTO)
        {
            return (new Facades.Facades()).EfectoActualizarFichero(VF_DOCUMENTO);
        }

        [WebMethod]
        public int VencimientosBorrarSiExiste(long documento)
        {
            return new Facades.Facades().VencimientosBorrarSiExiste(documento);
        }

        #endregion

        #region CLIENTES

        [WebMethod]
        public List<Combo> ClientesCombo()
        {
            return (new Facades.Facades()).ClientesCombo();
        }

        [WebMethod]
        public List<Cliente> ClientesData()
        {
            return (new Facades.Facades()).ClientesData();
        }

        [WebMethod]
        public List<Cliente> ClientesGuardar(string cliente, string nombre, string denominacion, string cif, string direccion
                 , string direccionb, string tfno, string fax, string poblacion, string provincia
                 , string codpostal, string formapago, string zona, int pvp, string VC_CODFORMAPAGO, string VC_BANCO, string VC_BANCO_DIRECCION, string VC_BANCO_POBLACION, string VC_BANCO_CUENTA)
        {
            return (new Facades.Facades()).ClientesGuardar(cliente, nombre, denominacion, cif, direccion, direccionb, tfno, fax, poblacion, provincia, codpostal, formapago, zona, pvp, VC_CODFORMAPAGO, VC_BANCO, VC_BANCO_DIRECCION, VC_BANCO_POBLACION, VC_BANCO_CUENTA);
        }

        [WebMethod]
        public int ClientesBorrarSiExiste(string cliente)
        {
            return new Facades.Facades().ClientesBorrarSiExiste(cliente);
        }

        #endregion

        //#region MENSAJES

        //[WebMethod]
        //public List<Mensaje> MensajesData(int? idBuzon, int? idMensaje, DateTime? FechaCreacionDesde, DateTime? FechaCreacionHasta,
        //                            DateTime? FechaEnvioDesde, DateTime? FechaEnvioHasta, int? idUsuarioRemitente,
        //                            int? idUsuarioDestinatario, bool? Leido)
        //{
        //    return new Facades.Facades().MensajesData(idBuzon, idMensaje, FechaCreacionDesde, FechaCreacionHasta, FechaEnvioDesde, FechaEnvioHasta,
        //                            idUsuarioRemitente, idUsuarioDestinatario, Leido);
        //}

        //[WebMethod]
        //public string MensajeGuardar(int idBuzon, int RemitenteIdUsuario, int DestinatarioIdUsuario, string Asunto, string Contenido, bool EnviarCopiaPorEmail)
        //{
        //    return new Facades.Facades().MensajeGuardar(idBuzon, RemitenteIdUsuario, DestinatarioIdUsuario, Asunto, Contenido, EnviarCopiaPorEmail);
        //}

        //[WebMethod]
        //public string MensajeActualizarFechaEnvio(int idMensaje, DateTime? FechaEnvio)
        //{
        //    return new Facades.Facades().MensajeGuardar(idMensaje, FechaEnvio);
        //}

        //[WebMethod]
        //public string MensajeMarcarComoLeido(int idMensaje)
        //{
        //    return new Facades.Facades().MensajeMarcarComoLeido(idMensaje);
        //}

        //#endregion

        #region BUZON

        [WebMethod]
        public List<Combo> BuzonesCombo()
        {
            return new Facades.Facades().BuzonesCombo();
        }

        #endregion

        #region USUARIOS

        [WebMethod]
        public List<Combo> listaTiposUsuario()
        {
            return (new Facades.Facades()).listaTiposUsuario();
        }

        [WebMethod]
        public List<Combo> listaUsuariosCombo(string tipo)
        {
            return (new Facades.Facades()).listaUsuariosCombo(tipo);
        }

        [WebMethod]
        public List<Combo> listaRolesCombo()
        {
            return new Facades.Facades().listaRolesCombo();
        }

        [WebMethod]
        public List<Usuario> UsuariosData(bool? showAdmin, bool? showStaff, bool? showClientes, bool? bloqueado)
        {
            return (new Facades.Facades()).UsuariosData(showAdmin, showStaff, showClientes, bloqueado);
        }

        [WebMethod]
        public Usuario Login(string userName, string pass)
        {
            return (new Facades.Facades()).Login(userName, pass);
        }

        [WebMethod]
        public int Logout(long idSesion)
        {
            return new Facades.Facades().Logout(idSesion);
        }

        [WebMethod]
        public string LoginOut(string userName, string pass)
        {
            try
            {
                Usuario user = (new Facades.Facades()).Login(userName, pass);
                if (user == null) return null;

                string cadena = string.Format("{0};{1}", userName, pass);
                UnicodeEncoding s = new UnicodeEncoding();

                return Convert.ToBase64String(s.GetBytes(cadena));
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        public string UsuariosGuardar(int? idUsuario, string login, string pass, int? tipo, string VC_CLIENTE, bool? bloqueado,
                        string AlmacenPreferido, bool? ConfirmarAuto, string Email, bool? GenerarPdfPedido, bool? MostrarMinuaturas,
                        bool? NotificarPedido, int? NRuedasPreselec, decimal DtoB2B, bool EnviarCreadenciaesPorEmail, TipoBusqueda TipoBusqueda)
        {
            return (new Facades.Facades()).UsuariosGuardar(idUsuario, login, pass, tipo, VC_CLIENTE, bloqueado, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
                                                     MostrarMinuaturas, NotificarPedido, NRuedasPreselec, DtoB2B, EnviarCreadenciaesPorEmail, TipoBusqueda);
        }

        [WebMethod]
        public string UsuariosEliminar(int idUsuario)
        {
            return (new Facades.Facades()).UsuariosEliminar(idUsuario);
        }

        [WebMethod]
        public string UsuarioTipoBusquedaGuardar(int idUsuario, TipoBusqueda tipoBusqueda)
        {
            return (new Facades.Facades()).UsuarioTipoBusquedaGuardar(idUsuario, tipoBusqueda);
        }

        [WebMethod]
        public string PerfilUsuarioGuardar(int? idUsuario, string AlmacenPreferido, bool? ConfirmarAuto, string Email,
                            bool? GenerarPdfPedido, bool? MostrarMinuaturas, bool? NotificarPedido, int? NRuedasPreselec)
        {
            return (new Facades.Facades()).PerfilUsuarioGuardar(idUsuario, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
                                                     MostrarMinuaturas, NotificarPedido, NRuedasPreselec);
        }

        [WebMethod]
        public int CambiarPassword(string login, string pass, string newPass)
        {
            return new Facades.Facades().CambiarPassword(login, pass, newPass);
        }

        #endregion

        #region PEDIDOS

        [WebMethod]
        public List<Pedido> PedidosData(long? idPedido, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE, string AlmacenPreferido)
        {
            return (new Facades.Facades()).PedidosData(idPedido, idEstado, fechaDesde, fechaHasta, VC_CLIENTE, AlmacenPreferido);
        }

        [WebMethod]
        public List<Estado> EstadosCombo()
        {
            return (new Facades.Facades()).EstadosCombo();
        }

        //[WebMethod]
        //public string PedidosGuardarpruebaPDF()
        //{
        //    List<Producto> lsProductos = new List<Producto>();
        //    Producto p = new Producto();
        //    p.ID = 1;

        //    p.VP_PRODUCTO = "DU25555193";
        //    p.Cantidad = 1;
        //    p.PrecioUnidad = 200;
        //    p.VP_CATEGORIA = "C";
        //    p.Ecotasa = 2;
        //    p.VP_PORC_IMP = 3;
        //    p.VT_PORC_IMP = 3;

        //    lsProductos.Add(p);
        //    return (new Facades.Facades()).PedidosGuardar(122, DateTime.Today, DateTime.Today, false, "CRTA GRAL BUZANADA - ", 144, 0, 4,3, "", lsProductos);
        //}

        [WebMethod]
        public string PedidosGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                            string dirEnvio, decimal? baseImp, decimal? descuento,
                            decimal? nfu, decimal? igic, string observaciones, List<Producto> lsProductos)
        {
            return (new Facades.Facades()).PedidosGuardar(idUsuario, fechaEnvio, fechaEntrega, porAgencia, dirEnvio, baseImp, descuento, nfu, igic, observaciones, lsProductos);
        }

        [WebMethod]
        public List<Producto> ProductosPedidoGetData(long idPedido)
        {
            return (new Facades.Facades()).ProductosPedidoGetData(idPedido);
        }

        [WebMethod]
        public int ActualizarEstadoPedido(long idPedido, DateTime? fecha)
        {
            return (new Facades.Facades()).ActualizarEstadoPedido(idPedido, fecha);
        }

        [WebMethod]
        public int AnularPedido(long idPedido)
        {
            return new Facades.Facades().AnularPedido(idPedido);
        }

        [WebMethod]
        public List<string> EstadoActualYSiguiente(int idEstado)
        {
            return (new Facades.Facades()).EstadoActualYSiguiente(idEstado);
        }

        #endregion

        #region RESERVAS

        [WebMethod]
        public List<Reserva> ReservasData(long? idReserva, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE)
        {
            return (new Facades.Facades()).ReservasData(idReserva, idEstado, fechaDesde, fechaHasta, VC_CLIENTE);
        }

        [WebMethod]
        public List<Estado> EstadosReservaCombo()
        {
            return (new Facades.Facades()).EstadosReservaCombo();
        }

        //[WebMethod]
        //public string ReservasGuardarpruebaPDF()
        //{

        //    List<Producto> lsProductos = new List<Producto>();
        //    Producto p = new Producto();
        //    p.ID = 1;

        //    p.VP_PRODUCTO = "DU25555193";
        //    p.Cantidad = 1;
        //    p.PrecioUnidad = 200;
        //    p.VP_CATEGORIA = "C";
        //    p.Ecotasa = 2;
        //    p.VP_PORC_IMP = 3;
        //    p.VT_PORC_IMP = 3;

        //    lsProductos.Add(p);
        //    return (new Facades.Facades()).ReservasGuardar(122, DateTime.Today, DateTime.Today, false, "CRTA GRAL BUZANADA - ", 144, 0, 4, 3, "", lsProductos);
        //}

        [WebMethod]
        public string ReservasGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                            string dirEnvio, decimal? baseImp, decimal? descuento,
                            decimal? nfu, decimal? igic, string observaciones, List<Producto> lsProductos)
        {
            return (new Facades.Facades()).ReservasGuardar(idUsuario, fechaEnvio, fechaEntrega, porAgencia, dirEnvio, baseImp, descuento, nfu, igic, observaciones, lsProductos);
        }

        [WebMethod]
        public List<Producto> ProductosReservaGetData(long idReserva)
        {
            return (new Facades.Facades()).ProductosReservaGetData(idReserva);
        }

        [WebMethod]
        public int ActualizarEstadoReserva(long idReserva, DateTime? fecha)
        {
            return (new Facades.Facades()).ActualizarEstadoReserva(idReserva, fecha);
        }

        [WebMethod]
        public int AnularReserva(long idReserva)
        {
            return new Facades.Facades().AnularReserva(idReserva);
        }

        [WebMethod]
        public List<string> EstadoReservaActualYSiguiente(int idEstado)
        {
            return (new Facades.Facades()).EstadoReservaActualYSiguiente(idEstado);
        }

        #endregion

        #region PRODUCTOS

        [WebMethod]
        public List<Producto> ProductosBuscadorStaff(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2)
        {
            return (new Facades.Facades()).ProductosBuscadorStaff(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2);
        }

        [WebMethod]
        public List<Producto> ProductosBuscador(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2)
        {
            return (new Facades.Facades()).ProductosBuscador(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2);
        }

        [WebMethod]
        public List<Producto> ProductosBuscadorV3(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2,
                                                  int pagina, int regPagina, ref int nFilasTotales, string ordenarPor, string ordenAscDesc)
        {
            return (new Facades.Facades()).ProductosBuscadorV3(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2, pagina, regPagina, ref nFilasTotales, ordenarPor, ordenAscDesc);
        }

        [WebMethod]
        public List<Producto> ProductosGuardar(string familia, string descfam, string producto, string descripcion
                                  , string producto1, string modelo, decimal? serie, decimal? llanta, string medida
                                  , string ic, string iv, decimal? tipo_neuma, string desc_tipo
                                  , decimal pvp1, decimal pvp2, decimal pvp3, int tipo_ofer, string categoria
                                  , decimal VP_PORC_IMP, string VP_IMAGEN, int? VP_IMPORTADO
                                  , decimal VP_NIVELRUIDO, string VP_EFICOMBUSTIBLE, string VP_ADHERENCIA, decimal VP_VALORRUIDO)
        {
            return (new Facades.Facades()).ProductosGuardar(familia, descfam, producto, descripcion, producto1, modelo, serie, llanta, medida, ic, iv, tipo_neuma, desc_tipo, pvp1, pvp2, pvp3, tipo_ofer, categoria, VP_PORC_IMP, VP_IMAGEN, VP_IMPORTADO, VP_NIVELRUIDO, VP_EFICOMBUSTIBLE, VP_ADHERENCIA, VP_VALORRUIDO);
        }

        [WebMethod]
        public List<Producto> ProductosData(string VP_FAMILIA, string VP_PRODUCTO, DateTime? MODIFICADO)
        {
            return (new Facades.Facades()).ProductosData(VP_FAMILIA, VP_PRODUCTO, MODIFICADO);
        }

        [WebMethod]
        public List<Producto> ProductosDataV3(string VP_PRODUCTO)
        {
            return (new Facades.Facades()).ProductosDataV3(VP_PRODUCTO);
        }

        [WebMethod]
        public int PermisosGuardar(int idUsuario, string xmlPermisos)
        {
            return new Facades.Facades().PermisosGuardar(idUsuario, xmlPermisos);
        }

        #endregion

        #region PRODCUTOS PEDIDO

        [WebMethod]
        public Pedido DetallesPedido(long idPedido)
        {
            return new Facades.Facades().DetallesPedido(idPedido);
        }

        [WebMethod]
        public Reserva DetallesReserva(long idReserva)
        {
            return new Facades.Facades().DetallesReserva(idReserva);
        }

        #endregion

        #region PENDIENTES

        [WebMethod]
        public List<Pendiente> PendientesPorProductoData(string producto)
        {
            return (new Facades.Facades()).PendientesPorProductoData(producto);
        }

        [WebMethod]
        public List<Pendiente> PendientesData(DateTime? llegada)
        {
            return (new Facades.Facades()).PendientesGetData(llegada);
        }

        [WebMethod]
        public List<Pendiente> PendientesGuardar(string articulo, DateTime llegada, string contenedor)
        {
            return (new Facades.Facades()).PendientesGuardar(articulo, llegada, contenedor);
        }

        [WebMethod]
        public int PendientesBorrarSiExiste(string articulo)
        {
            return new Facades.Facades().PendientesBorrarSiExiste(articulo);
        }

        #endregion

        #region EMAIL

        [WebMethod]
        public bool SendEmail(string[] addressesCc, string[] addressesBcc, string subject, string messageBody, string att)
        {
            return (new Facades.Facades()).Send(
                (addressesCc != null ? addressesCc.ToList<string>() : null),
                (addressesBcc != null ? addressesBcc.ToList<string>() : null), subject, messageBody, att);
        }

        [WebMethod]
        public bool SendEmailFromGUI(string addressesFrom, string subject, string messageBody)
        {
            return new Facades.Facades().Send(addressesFrom, subject, messageBody);
        }

        #endregion

        #region COMBOS

        [WebMethod]
        public List<Combo> FamiliasLogoCombo()
        {
            return (new Facades.Facades()).FamiliasLogoCombo();
        }

        [WebMethod]
        public List<Combo> FamiliasCombo()
        {
            return (new Facades.Facades()).FamiliasCombo();
        }

        [WebMethod]
        public List<Combo> ModelosCombo(string familia)
        {
            return (new Facades.Facades()).ModelosCombo(familia);
        }

        [WebMethod]
        public List<Combo> TipoNeumaticosCombo()
        {
            return (new Facades.Facades()).TipoNeumaticosCombo();
        }

        #endregion

        #region NOTIFICACIONES

        [WebMethod]
        public List<Notificaciones> NotificacionesData()
        {
            return new List<Notificaciones>();
        }

        #endregion

        #region ECOTASA

        [WebMethod]
        public List<Ecotasa> EcotasasData()
        {
            return (new Facades.Facades()).EcotasasData();
        }

        [WebMethod]
        public List<Ecotasa> EcotasaGuardar(string categoria, string descripcion, string detalles, decimal pvp1, decimal VT_PORC_IMP)
        {
            return (new Facades.Facades()).EcotasaGuardar(categoria, descripcion, detalles, pvp1, VT_PORC_IMP);
        }

        [WebMethod]
        public int EcotasaBorrarSiExiste(string categoria)
        {
            return new Facades.Facades().EcotasaBorrarSiExiste(categoria);
        }

        #endregion

        #region STOCK

        [WebMethod]
        public List<Stock> StockData()
        {
            return (new Facades.Facades()).StockData();
        }

        //[WebMethod]
        //public List<Stock> StockGuardar(string familia, string descfamilia, string articulo, string descarticulo, string articulo1
        //                               , string modelo, string indvelocidad, string indcarga, string categoria
        //                               , decimal stock_a01, decimal stock_a02, decimal stock_a03, decimal stock_a04
        //                               , decimal stock_a18, decimal stock_a19, decimal stock_a22, decimal stock_a23
        //                               , decimal stock_a32, decimal stock_a44, decimal stock_a54, decimal stock_gen
        //                               , decimal stock_a12, decimal stock_a13, decimal stock_a55, decimal stock_a24
        //                               , decimal stock_a27, decimal stock_a29, decimal stock_a31, decimal stock_a43
        //                               , decimal stock_a45, decimal stock_a46, decimal stock_a47, decimal stock_a56
        //                               , decimal stock_a53, decimal stock_a60, decimal stock_a63
        //                               )
        //{
        //    return (new Facades.Facades()).StockGuardar(familia, descfamilia, articulo, descarticulo, articulo1, modelo, indvelocidad, indcarga, categoria, stock_a01, stock_a02, stock_a03, stock_a04, stock_a18, stock_a19, stock_a22, stock_a23, stock_a32, stock_a44, stock_a54, stock_gen, stock_a12, stock_a13, stock_a55, stock_a24, stock_a27, stock_a29, stock_a31, stock_a43, stock_a45, stock_a46, stock_a47, stock_a56);
        //}

        #endregion

        #region SYNC

        [WebMethod]
        public void SyncLastUpdate(bool SyncNocturna)
        {
            new Facades.Facades().SyncLastUpdate(SyncNocturna);
        }

        [WebMethod]
        public DateTime? SyncLastDate(bool SyncNocturna)
        {
            return new Facades.Facades().SyncLastDate(SyncNocturna);
        }

        #endregion

        #region PROMOCIONES

        [WebMethod]
        public List<Promocion> PromocionesData(Promocion promocion)
        {
            return new Facades.Facades().PromocionesData(promocion);
        }

        [WebMethod]
        public string PromocionesGuardar(Promocion promocion)
        {
            return new Facades.Facades().PromocionesGuardar(promocion);
        }

        [WebMethod]
        public string PromocionesEliminar(int idPromo)
        {
            return new Facades.Facades().PromocionesEliminar(idPromo);
        }

        #endregion

        #endregion
    }

}
