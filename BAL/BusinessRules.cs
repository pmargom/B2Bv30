using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Configuration;
using BAL.pdf;
using Root.Reports;
using B2B.Types;
using B2B.Generic;
using System.IO;
using System.Transactions;
using PDFObjects;

namespace BusinessRules
{
    public class BusinessRules
    {
        private DataAccess.DataAccess mDataAccess;

        public DataAccess.DataAccess DAL
        {
            get
            {
                if (this.mDataAccess == null)
                {
                    this.mDataAccess = new DataAccess.DataAccess();
                }
                return this.mDataAccess;
            }
        }

        public BusinessRules()
        {
        }

        #region SQL Server Sync

        public DataSet SqlCleanTables()
        {
            return DAL.SqlCleanTables();
        }

        public DataSet SqlGuardar(string DBTable, string parametros)
        {
            return DAL.SqlGuardar(DBTable, parametros);
        }

        public DataSet Sql_Sincronizar(DateTime fechaNuevaSync, int tiempo)
        {
            return DAL.Sql_Sincronizar(fechaNuevaSync, tiempo);
        }

        public DataSet Sql_SyncUpdate(DateTime fechaSync)
        {
            return DAL.Sql_SyncUpdate(fechaSync);
        }

        public void SyncLastUpdate(bool SyncNocturna)
        {
            DAL.SyncLastUpdate(SyncNocturna);
        }

        public DataSet SyncLastDate(bool SyncNocturna)
        {
            return DAL.SyncLastDate(SyncNocturna);
        }

        #endregion

        #region MENSAJES

        public int MensajeMarcarComoLeido(int idMensaje)
        {
            return DAL.MensajeMarcarComoLeido(idMensaje);
        }

        public int MensajeGuardar(Mensaje mensaje)
        {
            try
            {
                int nMensajesGuardados = 0;
                if (mensaje.EnviarATodos)
                {
                    DataSet ds = DAL.listaUsuariosPorTipo(mensaje.TipoUser.ToString());
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return 0;
                    DataTable dt = ds.Tables[0];
                    int nMensajesAEnviar = ds.Tables[0].Rows.Count;
                    dt.AsEnumerable().ToList().ForEach(p => 
                    {
                        mensaje.DestinatarioIdUsuario = p.Field<int>("idUsuario");
                        int ret = DAL.MensajeGuardar(mensaje);
                        if (ret == 1) nMensajesGuardados++;
                    });
                    if (nMensajesAEnviar != nMensajesGuardados) return -2;
                }
                else // es un mensaje a un solo destinatario
                {
                    nMensajesGuardados = DAL.MensajeGuardar(mensaje);
                }
                return nMensajesGuardados;
            }
            catch (Exception)
            {
                return -1;
            }           
        }

        public DataSet MensajesData(Mensaje mensaje)
        {
            return DAL.MensajesData(mensaje);
        }

        #endregion

        #region FAMILIAS LOGOS

        public DataSet FamiliasLogoData(Familia familia)
        {
            return DAL.FamiliasLogoData(familia);
        }

        public DataSet FamiliasLogoGuardar(Familia familia)
        {
            return DAL.FamiliasLogoGuardar(familia);
        }

        public DataSet FamiliasLogoEliminar(string VF_FAMILIA)
        {
            return DAL.FamiliasLogoEliminar(VF_FAMILIA);
        }

        #endregion

        #region CLIENTES VS PEDIDOS

        public DataSet ClientesVsPedidosData(DateTime? FechaDesde, DateTime? FechaHasta, string clienteDesde, string clienteHasta)
        {
            return DAL.ClientesVsPedidosData(FechaDesde,FechaHasta, clienteDesde, clienteHasta);
        }

        #endregion

        #region SESIONES

        public DataSet SesionesData(Sesion sesion)
        {
            return DAL.SesionesData(sesion);
        }

        #endregion

        #region PEDIDOS POR CLIENTE

        public DataSet PedidosXClientesData(DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            return DAL.PedidosXClientesData(FechaDesde, FechaHasta,VC_CLIENTE);
        }

        #endregion

        #region BUSQUEDAS FALLIDAS

        public DataSet BusquedasFallidasData(string Referencia, DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            return DAL.BusquedasFallidasData(Referencia, FechaDesde, FechaHasta, VC_CLIENTE);
        }

        public int BusquedasFallidasGuardar(string Referencia, string VC_CLIENTE)
        {
            return DAL.BusquedasFallidasGuardar(Referencia, VC_CLIENTE);
        }

        public int BusquedasFallidasEliminar(string Referencia)
        {
            return DAL.BusquedasFallidasEliminar(Referencia);
        }

        #endregion

        #region CONFIGURACION

        public DataSet ConfigData(string parametro)
        {
            return DAL.ConfigData(parametro);
        }

        public int ConfiguracionGuardar(string parametro, string valor)
        {
            return DAL.ConfiguracionGuardar(parametro, valor);
        }

        #endregion

        #region FACTURAS

        public DataSet FacturasData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return DAL.FacturasData(cliente, fechaDesde, fechaHasta);
        }

        public DataSet FacturasDataDesglosada(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return DAL.FacturasDataDesglosada(cliente, fechaDesde, fechaHasta);
        }

        public DataSet FacturaData(int idFactura)
        {
            DataSet ds = null;
            try
            {
                ds = DAL.FacturaData(idFactura);
                DataTable facturas = ds.Tables[0];
                Factura fac = new Factura();
                facturas.AsEnumerable().ToList().ForEach(p =>
                {
                    fac.ID = p.Field<int>("ID");
                    fac.VF_CLIENTE = p.Field<string>("VF_CLIENTE");
                    fac.VF_FACTURA = p.Field<string>("VF_FACTURA");
                    fac.VF_ALBARAN = p.Field<string>("VF_ALBARAN");
                    fac.VF_FECHA_FACT = p.Field<DateTime?>("VF_FECHA_FACT");
                    fac.IMP_BRUTO = p.Field<decimal>("IMP_BRUTO");
                    fac.DTO = p.Field<decimal>("DTO");
                    fac.IGIC = p.Field<decimal>("IGIC");
                    fac.NFU = p.Field<string>("NFU");
                    fac.TOTAL = p.Field<decimal>("TOTAL");
                });

                RepFactura rep = new RepFactura();
                rep.Datos = fac;
                string path = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["PdfDir"]);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                rep.Save(string.Format(@"{0}\{1}.pdf", path, fac.ID));
            }
            catch { }
            return ds;
        }

        public DataSet FacturasGuardar(string cliente, string nombre_cliente, string factura, DateTime? fecha_fact, string albaran
                                     , DateTime? fecha_alb, string articulo, string descarticulo, string categoria, decimal unidades
                                     , decimal pventa, decimal porc_dcto, decimal imp_dcto, decimal neto, decimal porc_igic, decimal VF_LINEA)
        {
            return DAL.FacturasGuardar(cliente, nombre_cliente, factura, fecha_fact, albaran, fecha_alb, articulo, descarticulo, categoria, unidades, pventa, porc_dcto, imp_dcto, neto, porc_igic, VF_LINEA);
        }

        public int FacturaActualizarFichero(string VF_FACTURA)
        {
            try
            {
                // consulto la info para el PDF
                DataSet ds = DAL.DatosFacturaPDF(VF_FACTURA);
                if (!GenerarFacturaPDF(VF_FACTURA, ds)) return -1;

                // si generó el pdf, entonces tengo que actualizar la ruta en bd para posterior descarga
                string VDir = ConfigurationManager.AppSettings["VDir"];
                string PdfDir = ConfigurationManager.AppSettings["PdfDir"];
                string PdfFacturasDir = ConfigurationManager.AppSettings["PdfFacturasDir"];
                string rutaPDF = string.Format("{0}/{1}/{2}/{3}.pdf", VDir, PdfDir, PdfFacturasDir, VF_FACTURA);

                return DAL.FacturaActualizarFichero(VF_FACTURA, rutaPDF);
            }
            catch
            {
                return -1;
            }
        }

        public int FacturasBorrarSiExiste(string factura, string albaran, decimal VF_LINEA)
        {
            return DAL.FacturasBorrarSiExiste(factura, albaran, VF_LINEA);
        }

        public int FacturasPorClienteBorrar(string cliente)
        {
            return DAL.FacturasPorClienteBorrar(cliente);
        }

        #endregion

        #region ALBARANES

        public DataSet AlbaranesData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta, bool? showAll)
        {
            return DAL.AlbaranesData(cliente, fechaDesde, fechaHasta, showAll);
        }

        public int AlbaranActualizarFichero(string VF_ALBARAN)
        {
            try
            {
                // consulto la info para el PDF
                DataSet ds = DAL.DatosAlbaranPDF(VF_ALBARAN);
                if (!GenerarAlbaranPDF(VF_ALBARAN, ds)) return -1;

                // si generó el pdf, entonces tengo que actualizar la ruta en bd para posterior descarga
                string VDir = ConfigurationManager.AppSettings["VDir"];
                string PdfDir = ConfigurationManager.AppSettings["PdfDir"];
                string PdfAlbaranesDir = ConfigurationManager.AppSettings["PdfAlbaranesDir"];
                string rutaPDF = string.Format("{0}/{1}/{2}/{3}.pdf", VDir, PdfDir, PdfAlbaranesDir, VF_ALBARAN);

                return DAL.AlbaranActualizarFichero(VF_ALBARAN, rutaPDF);
            }
            catch
            {
                return -1;
            }
        }

        #endregion

        #region VENCIMIENTOS

        public DataSet VencimientosData(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return DAL.VencimientosData(cliente, fechaDesde, fechaHasta);
        }

        public DataSet VencimientosDataDesglosada(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return DAL.VencimientosDataDesglosada(cliente, fechaDesde, fechaHasta);
        }

        public DataSet VencimientosGuardar(decimal cliente, string factura, long documento, string tipo_doc,
                                   DateTime emision, DateTime vencimiento, decimal importe, decimal estado)
        {
            return DAL.VencimientosGuardar(cliente, factura, documento, tipo_doc, emision, vencimiento, importe, estado);
        }

        public int EfectoActualizarFichero(string VE_DOCUMENTO)
        {
            try
            {
                // consulto la info para el PDF
                DataSet ds = DAL.DatosEfectoPDF(VE_DOCUMENTO);

                DataRow fila = ds.Tables[0].Rows[0];
                if (string.IsNullOrEmpty(fila["VE_FECHA_FACT"].ToString())) return -2;
                if (!GenerarEfectoPDF(VE_DOCUMENTO, ds)) return -1;

                // si generó el pdf, entonces tengo que actualizar la ruta en bd para posterior descarga
                string VDir = ConfigurationManager.AppSettings["VDir"];
                string PdfDir = ConfigurationManager.AppSettings["PdfDir"];
                string PdfEfectosDir = ConfigurationManager.AppSettings["PdfEfectosDir"];
                string rutaPDF = string.Format("{0}/{1}/{2}/{3}.pdf", VDir, PdfDir, PdfEfectosDir, VE_DOCUMENTO);
                return DAL.EfectoActualizarFichero(VE_DOCUMENTO, rutaPDF);
            }
            catch
            {
                return -1;
            }
        }

        public int VencimientosBorrarSiExiste(long documento)
        {
            return DAL.VencimientosBorrarSiExiste(documento);
        }

        #endregion

        #region CLIENTES

        public DataSet ClientesCombo()
        {
            return DAL.ClientesCombo();
        }

        public DataSet ClientesData()
        {
            return DAL.ClientesData();
        }

        public DataSet ClientesGuardar(string cliente, string nombre, string denominacion, string cif, string direccion
                             , string direccionb, string tfno, string fax, string poblacion, string provincia
                             , string codpostal, string formapago, string zona, int pvp, string VC_CODFORMAPAGO, string VC_BANCO, string VC_BANCO_DIRECCION, string VC_BANCO_POBLACION, string VC_BANCO_CUENTA)
        {
            return DAL.ClientesGuardar(cliente, nombre, denominacion, cif, direccion, direccionb, tfno, fax, poblacion, provincia, codpostal, formapago, zona, pvp, VC_CODFORMAPAGO, VC_BANCO, VC_BANCO_DIRECCION, VC_BANCO_POBLACION, VC_BANCO_CUENTA);
        }

        public int ClientesBorrarSiExiste(string cliente)
        {
            return DAL.ClientesBorrarSiExiste(cliente);
        }

        #endregion

        //#region MENSAJES

        //public DataSet MensajesData(int? idBuzon, int? idMensaje, DateTime? FechaCreacionDesde, DateTime? FechaCreacionHasta,
        //                            DateTime? FechaEnvioDesde, DateTime? FechaEnvioHasta, int? idUsuarioRemitente,
        //                            int? idUsuarioDestinatario, bool? Leido)
        //{
        //    return DAL.MensajesData(idBuzon, idMensaje, FechaCreacionDesde, FechaCreacionHasta, FechaEnvioDesde, FechaEnvioHasta,
        //                            idUsuarioRemitente, idUsuarioDestinatario, Leido);
        //}

        //public DataSet MensajeGuardarEnviarPorEmail(int idBuzon, int RemitenteIdUsuario, int DestinatarioIdUsuario, string Asunto, string Contenido)
        //{
        //    return DAL.MensajeGuardarEnviarPorEmail(idBuzon, RemitenteIdUsuario, DestinatarioIdUsuario, Asunto, Contenido);
        //}

        //public int MensajeGuardar(int idBuzon, int RemitenteIdUsuario, int DestinatarioIdUsuario, string Asunto, string Contenido)
        //{
        //    return DAL.MensajeGuardar(idBuzon, RemitenteIdUsuario, DestinatarioIdUsuario, Asunto, Contenido);
        //}

        //public int MensajeGuardar(int idMensaje, DateTime? FechaEnvio)
        //{
        //    return DAL.MensajeGuardar(idMensaje, FechaEnvio);
        //}

        //public int MensajeMarcarComoLeido(int idMensaje)
        //{
        //    return DAL.MensajeMarcarComoLeido(idMensaje);
        //}

        //#endregion

        #region BUZON

        public DataSet BuzonesCombo()
        {
            return DAL.BuzonesCombo();
        }

        #endregion

        #region USUARIOS

        public DataSet ObtenerDatosUsuario (int idUsuario)
        {
            return DAL.ObtenerDatosUsuario(idUsuario);
        }

        public DataSet UsuariosData(bool? showAdmin, bool? showStaff, bool? showClientes, bool? bloqueado)
        {
            return DAL.UsuariosData(showAdmin, showStaff, showClientes, bloqueado);
        }

        public DataSet Login(string userName, string pass)
        {
            return DAL.Login(userName, pass);
        }

        public int Logout(long idSesion)
        {
            return DAL.Logout(idSesion);
        }

        public DataSet UsuariosGuardar(int? idUsuario, string login, string pass, int? tipo, string VC_CLIENTE, bool? bloqueado,
                   string AlmacenPreferido, bool? ConfirmarAuto, string Email, bool? GenerarPdfPedido, bool? MostrarMinuaturas,
                    bool? NotificarPedido, int? NRuedasPreselec, decimal DtoB2B, string TipoBusqueda)
        {
            return DAL.UsuariosGuardar(idUsuario, login, pass, tipo, VC_CLIENTE, bloqueado, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
                    MostrarMinuaturas, NotificarPedido, NRuedasPreselec, DtoB2B, TipoBusqueda);
        }

        public DataSet UsuariosEliminar(int idUsuario)
        {
            return DAL.UsuariosEliminar(idUsuario);
        }

        public DataSet UsuarioTipoBusquedaGuardar(int idUsuario, TipoBusqueda tipoBusqueda)
        {
            return DAL.UsuarioTipoBusquedaGuardar(idUsuario, tipoBusqueda.Serialize());
        }

        public DataSet PerfilUsuarioGuardar(int? idUsuario, string AlmacenPreferido, bool? ConfirmarAuto, string Email,
                   bool? GenerarPdfPedido, bool? MostrarMinuaturas, bool? NotificarPedido, int? NRuedasPreselec)
        {
            return DAL.PerfilUsuarioGuardar(idUsuario, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
                    MostrarMinuaturas, NotificarPedido, NRuedasPreselec);
        }

        public Usuario PerfilUsuario(string VC_CLIENTE)
        {
            try
            {
                DataSet ds = DAL.PerfilUsuario(VC_CLIENTE);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;

                Usuario usuario = new Usuario();

                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                    {
                        usuario.AlmacenPreferido = p.Field<string>("AlmacenPreferido");
                        usuario.Bloqueado = p.Field<bool>("Bloqueado");
                        usuario.ConfirmarAuto = p.Field<bool?>("ConfirmarAuto");
                        usuario.Email = p.Field<string>("Email");
                        usuario.GenerarPdfPedido = p.Field<bool?>("GenerarPdfPedido");
                        usuario.idUsuario = p.Field<int>("idUsuario");
                        usuario.login = p.Field<string>("login");
                        usuario.MostrarMinuaturas = p.Field<bool?>("MostrarMinuaturas");
                        usuario.NotificarPedido = p.Field<bool?>("NotificarPedido");
                        usuario.NRuedasPreselec = p.Field<int?>("NRuedasPreselec");
                        usuario.tipo = p.Field<int>("tipo").Parse<TipoUsuario>();
                        usuario.VC_CLIENTE = p.Field<string>("VC_CLIENTE");
                    });

                return usuario;
            }
            catch
            {
                return null;
            }
        }

        public int CambiarPassword(string login, string pass, string newPass)
        {
            return DAL.CambiarPassword(login, pass, newPass);
        }

        public DataSet listaRolesCombo()
        {
            return DAL.listaRolesCombo();
        }

        public DataSet listaTiposUsuario()
        {
            return DAL.listaTiposUsuario();
        }

        public DataSet listaUsuariosCombo(string tipo)
        {
            return DAL.listaUsuariosCombo(tipo);
        }

        public DataSet PermisosData(int? idUsuario, int? idRole, string CodModulo)
        {
            return DAL.PermisosData(idUsuario, idRole, CodModulo);
        }

        public int PermisosGuardar(int idUsuario, string xmlPermisos)
        {
            return DAL.PermisosGuardar(idUsuario, xmlPermisos);
        }

        #endregion

        #region DOCUMENTOS PDF

        private bool GenerarPedidoPDF(string tipo, Pedido pedido, ref string rutaPdf)
        {
            try
            {
                string pdfPath = string.Format(@"{0}{1}\{2}", AppDomain.CurrentDomain.BaseDirectory
                                                          , ConfigurationManager.AppSettings["PdfDir"]
                                                          , ConfigurationManager.AppSettings["PdfPedidosDir"]);
                if (!Directory.Exists(pdfPath)) Directory.CreateDirectory(pdfPath);
                pdfPath += string.Format(@"\{0}.pdf", pedido.idPedido);

                // paso los datos del pedido
                PedidoPDF pdf = new PedidoPDF();
                pdf.pedido = pedido;
                pdf.Tipo = tipo.ToUpperInvariant();
                pdf.EsCopia = true;

                pdf.Generar();

                pdf.Save(pdfPath);

                rutaPdf = pdfPath;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GenerarReservaPDF(string tipo, Reserva reserva, ref string rutaPdf)
        {
            try
            {
                string pdfPath = string.Format(@"{0}{1}\{2}", AppDomain.CurrentDomain.BaseDirectory
                                                          , ConfigurationManager.AppSettings["PdfDir"]
                                                          , ConfigurationManager.AppSettings["PdfReservasDir"]);
                if (!Directory.Exists(pdfPath)) Directory.CreateDirectory(pdfPath);
                pdfPath += string.Format(@"\{0}.pdf", reserva.idReserva);

                // paso los datos del pedido
                ReservaPDF pdf = new ReservaPDF();
                pdf.reserva = reserva;
                pdf.Tipo = tipo.ToUpperInvariant();
                pdf.EsCopia = true;

                pdf.Generar();

                pdf.Save(pdfPath);

                rutaPdf = pdfPath;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GenerarFacturaPDF(string VF_FACTURA, DataSet dsDatos)
        {
            try
            {
                string pdfPath = string.Format(@"{0}{1}\{2}", AppDomain.CurrentDomain.BaseDirectory
                                                          , ConfigurationManager.AppSettings["PdfDir"]
                                                          , ConfigurationManager.AppSettings["PdfFacturasDir"]);
                if (!Directory.Exists(pdfPath)) Directory.CreateDirectory(pdfPath);
                pdfPath += string.Format(@"\{0}.pdf", VF_FACTURA);

                // paso los datos del pedido
                FacturaPDF pdf = new FacturaPDF();
                pdf.DatosFacturaPDF = dsDatos;
                pdf.EsCopia = true;

                pdf.Generar();
                pdf.Save(pdfPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GenerarAlbaranPDF(string VF_ALBARAN, DataSet dsDatos)
        {
            try
            {
                string pdfPath = string.Format(@"{0}{1}\{2}", AppDomain.CurrentDomain.BaseDirectory
                                                          , ConfigurationManager.AppSettings["PdfDir"]
                                                          , ConfigurationManager.AppSettings["PdfAlbaranesDir"]);
                if (!Directory.Exists(pdfPath)) Directory.CreateDirectory(pdfPath);
                pdfPath += string.Format(@"\{0}.pdf", VF_ALBARAN);

                // paso los datos del pedido
                AlabaranPDF pdf = new AlabaranPDF();
                pdf.DatosAlbaranPDF = dsDatos;
                pdf.EsCopia = true;

                pdf.Generar();
                pdf.Save(pdfPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GenerarEfectoPDF(string VE_DOCUMENTO, DataSet dsDatos)
        {
            try
            {
                string pdfPath = string.Format(@"{0}{1}\{2}", AppDomain.CurrentDomain.BaseDirectory
                                                          , ConfigurationManager.AppSettings["PdfDir"]
                                                          , ConfigurationManager.AppSettings["PdfEfectosDir"]);
                if (!Directory.Exists(pdfPath)) Directory.CreateDirectory(pdfPath);
                pdfPath += string.Format(@"\{0}.pdf", VE_DOCUMENTO);

                // paso los datos del pedido
                EfectoPDF pdf = new EfectoPDF();
                pdf.DatosEfectoPDF = dsDatos;
                pdf.EsCopia = true;

                pdf.Generar();
                pdf.Save(pdfPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region PEDIDOS

        public DataSet EstadosCombo()
        {
            return DAL.EstadosCombo();
        }

        public string PedidosGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                                string dirEnvio, decimal? baseImp, decimal? descuento,
                                decimal? nfu, decimal? igic, string observaciones, List<Producto> lsProductos)
        {
            string error = "";
            Pedido pedido = null;
            using (TransactionScope scope = new TransactionScope())
            {
                long idPedido = -1;
                DataSet ds = DAL.PedidosGuardar(idUsuario, fechaEnvio, fechaEntrega, porAgencia, dirEnvio, baseImp, descuento, nfu, igic, observaciones, ref idPedido);
                if (ds == null || ds.Tables.Count == 0) return "Error al guardar el pedido.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();

                if (idPedido <= 0) return "Error al guardar los datos del pedido.";

                pedido = new Pedido();
                ds.Tables[1].AsEnumerable().ToList().ForEach(p =>
                    {
                        pedido.idEstado = p.Field<int>("idEstado");
                        pedido.Descripcion = p.Field<string>("descripcion");
                        pedido.idPedido = p.Field<long>("idPedido");
                        pedido.Referencia = p.Field<string>("Referencia");
                        pedido.VF_AlBARAN = p.Field<string>("VF_ALBARAN");
                        pedido.Fecha = p.Field<DateTime?>("Fecha");
                        pedido.FechaEnvio = p.Field<DateTime?>("FechaEnvio");
                        pedido.FechaEntrega = p.Field<DateTime?>("FechaEntrega");
                        pedido.DirEnvio = p.Field<string>("DirEnvio");
                        pedido.BaseImponible = p.Field<decimal>("BaseImponible");
                        pedido.Descuento = p.Field<decimal>("Descuento");
                        pedido.NFU = p.Field<decimal>("NFU");
                        //pedido.IGIC = p.Field<decimal>("IGIC");
                        pedido.ImporteDescuento = p.Field<decimal>("ImporteDescuento");
                        pedido.PrecioConDescuento = p.Field<decimal>("PrecioConDescuento");
                        pedido.ImporteIGIC = p.Field<decimal>("ImporteIGIC");
                        pedido.Total = p.Field<decimal>("Total");
                        pedido.Cliente = new Cliente()
                        {
                            VC_CIF = p.Field<string>("VC_CIF"),
                            VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                            VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                            VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                            VC_TFNO = p.Field<string>("VC_TFNO"),
                            VC_FAX = p.Field<string>("VC_FAX"),
                            VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                            VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                            VC_POBLACION = p.Field<string>("VC_POBLACION"),
                            VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                            VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                            VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                            VC_ZONA = p.Field<string>("VC_ZONA"),
                            VC_PVP = p.Field<int>("VC_PVP"),
                            VC_HORA = p.Field<DateTime?>("VC_HORA"),
                            VC_CODFORMAPAGO = p.Field<string>("VC_CODFORMAPAGO")
                        };
                        pedido.Observaciones = p.Field<string>("Observaciones");
                    });

                // guardo los productos del pedido

                if (lsProductos == null || lsProductos.Count == 0) return "Error: No existen productos en el pedido.";

                pedido.Productos = lsProductos;

                lsProductos.ForEach(p =>
                {
                    if (ProductosPedidoGuardar(idPedido, p.VP_PRODUCTO, p.Cantidad, p.PrecioUnidad, p.VP_CATEGORIA, p.Ecotasa, p.VP_PORC_IMP, p.VT_PORC_IMP) <= 0)
                    {
                        error = "Error al guardar los productos del pedido.";
                        return;
                    }
                });

                scope.Complete();
            }
            if (string.IsNullOrEmpty(error)) // si no hay error, entonces genero PDF
            {
                //debo consultar los productos del pedido para que se carguen datos como el detalle de la ecotasa
                lsProductos = new List<Producto>();
                ProductosPedidoGetData(pedido.idPedido).Tables[0].AsEnumerable().ToList().ForEach(
                    p => lsProductos.Add(new Producto()
                    {
                        ID = p.Field<long>("ID"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA"),
                        Cantidad = p.Field<int>("Cantidad"),
                        PrecioUnidad = p.Field<decimal>("PrecioUnidad"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        Ecotasa = p.Field<decimal>("Ecotasa"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                        ECOTASA_DETALLES = p.Field<string>("ECOTASA_DETALLES"),
                        VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP"),
                        VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP")
                    }
                ));
                pedido.Productos = lsProductos;
                string att = "";
                if (!GenerarPedidoPDF("pedido", pedido, ref att))
                {
                    error = "Error al generar copia del pedido en formato PDF.";
                }
                else //envio el pdf por email
                {
                    List<string> direccionesNeuatlan = new List<string>();
                    string emailPedidos = ConfigurationManager.AppSettings["emailPedidos"];
                    if (!string.IsNullOrEmpty(emailPedidos)) direccionesNeuatlan.Add(emailPedidos);

                    Usuario usuario = PerfilUsuario(pedido.Cliente.VC_CLIENTE);
                    List<string> direccionesCliente = new List<string>();
                    if (usuario != null && usuario.NotificarPedido.Value && !string.IsNullOrEmpty(usuario.Email)) direccionesCliente.Add(usuario.Email);

                    if (direccionesNeuatlan.Count > 0 && !string.IsNullOrEmpty(att))
                    {
                        string cuerpo = ConstruyeCuerpoEmailRecibido(pedido.Cliente.VC_DENOMINACION, pedido.Referencia);
                        bool res = Send(direccionesNeuatlan, null, direccionesCliente, string.Format("B2B NA: Su pedido Nro. {0} ha sido RECIBIDO", pedido.Referencia), cuerpo, att);
                        if (!res)
                        {
                            error += "Atención!: Su pedido se ha generado correctamente pero no ha sido posible su envío por email.";
                        }
                    }
                }
            }

            return error;

        }

        private string ConstruyeCuerpoEmailEnvioCredenciales(string cliente, TipoUsuario tipo, string username, string password)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores de:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>";
                plantilla += "Se ha creado un usuario de tipo: ";
                plantilla += "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><b>";
                plantilla += "token_tipo_cliente</b></span></span>. Para acceder a la aplicación B2B de ";
                plantilla += "Neumáticos Atlántico, S.L. debe usar los siguientes datos:<br />";
                plantilla += "<br />";
                plantilla += "&nbsp;&nbsp;&nbsp; 1.- Acceder a nuestro sitio web en la dirección:";
                plantilla += "<a href='http://www.neumaticosatlantico.com/'>http://www.neumaticosatlantico.com/</a>.<br />";
                plantilla += "&nbsp;&nbsp;&nbsp; 2.- Indicar como nombre de usuario: <b>token_username</b>.<br />";
                plantilla += "&nbsp;&nbsp;&nbsp; 3.- Indicar como contraseña: <b>token_pass</b>.<br />";
                plantilla += "<br />";
                plantilla += "Si tiene alguna duda o no puede acceder a la aplicación, por favor, póngase en contacto ";
                plantilla += "con nosotros.&nbsp;<u></u></span></p>";
                plantilla += "<p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>";
                plantilla += "<br />";
                plantilla += "Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente)
                                .Replace("token_tipo_cliente", tipo.ToString().Replace("_", "-"))
                                .Replace("token_username", username)
                                .Replace("token_pass", password);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ConstruyeCuerpoEmailRecibido(string cliente, string nPedido)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su pedido&nbsp;<b><i>Nro. token_n_pedido</i></b>&nbsp;ha sido “<b>RECIBIDO”</b>&nbsp;en nuestro&nbsp;<b>Call Center de Atención al Cliente</b> y se encuentra en estado <b>PENDIENTE DE CONFIRMACIÓN</b>, por lo que en breves minutos recibirá un mensaje electrónico, confirmando la disponibilidad de los artículos solicitados y le &nbsp;indicará si su pedido ha sido “<b>ACEPTADO</b>.”<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_pedido", nPedido);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ConstruyeCuerpoEmailAceptado(string cliente, string nPedido, string fechaEnvio)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su pedido&nbsp;<b><i>Nro. token_n_pedido</i></b>&nbsp;ha sido <b>“ACEPTADO”</b>&nbsp;en nuestro&nbsp;<b>Call Center de Atención al Cliente</b> con fecha programada de env&iacute;o para el d&iacute;a token_fecha_envio.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_pedido", nPedido).Replace("token_fecha_envio", fechaEnvio);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ConstruyeCuerpoEmailEntregado(string cliente, string nPedido, string fechaEntrega)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su pedido&nbsp;<b><i>Nro. token_n_pedido</i></b>&nbsp;ha sido <b>“ENTREGADO”</b> con fecha token_fecha_entrega. En caso de existir alguna observaci&oacute;n no dude en ponerse en contacto con nuestro <b>Call Center de Atención al Cliente</b> al tel&eacute;fono 902 402 044.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_pedido", nPedido).Replace("token_fecha_entrega", fechaEntrega);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ConstruyeCuerpoEmailAnulado(string cliente, string nPedido)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su pedido&nbsp;<b><i>Nro. token_n_pedido</i></b>&nbsp;ha sido <b>“ANULADO”</b>&nbsp;en nuestro&nbsp;<b>Call Center de Atención al Cliente</b>.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_pedido", nPedido);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet ProductosPedidoGetData(long idPedido)
        {
            return DAL.ProductosPedidoGetData(idPedido);
        }

        public DataSet PedidosData(long? idPedido, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE, string AlmacenPreferido)
        {
            return DAL.PedidosData(idPedido, idEstado, fechaDesde, fechaHasta, VC_CLIENTE, AlmacenPreferido);
        }

        public int ActualizarEstadoPedido(long idPedido, DateTime? fecha)
        {
            try
            {
                int resul = DAL.ActualizarEstadoPedido(idPedido, fecha);
                if (resul <= 0) return resul;
                // envio email al usuario
                #region
                DataSet ds = DetallesPedido(idPedido);
                if (ds == null || ds.Tables.Count < 2) return -1; //2 porque devuelvo 2 tables

                Pedido pedido = null;
                ds.Tables[0].AsEnumerable().ToList().ForEach(
                    p => pedido = new Pedido()
                    {
                        idEstado = p.Field<int>("idEstado"),
                        Codigo = p.Field<string>("Codigo"),
                        Descripcion = p.Field<string>("Descripcion"),
                        idPedido = p.Field<long>("idPedido"),
                        Referencia = p.Field<string>("Referencia"),
                        VF_AlBARAN = p.Field<string>("VF_AlBARAN"),
                        Fecha = p.Field<DateTime?>("Fecha"),
                        FechaEnvio = p.Field<DateTime?>("FechaEnvio"),
                        FechaEntrega = p.Field<DateTime?>("FechaEntrega"),
                        PorAgencia = p.Field<bool?>("PorAgencia"),
                        DirEnvio = p.Field<string>("DirEnvio"),
                        BaseImponible = p.Field<decimal>("BaseImponible"),
                        Descuento = p.Field<decimal>("Descuento"),
                        NFU = p.Field<decimal>("NFU"),
                        //IGIC = p.Field<decimal>("IGIC"),
                        ImporteDescuento = p.Field<decimal>("ImporteDescuento"),
                        PrecioConDescuento = p.Field<decimal>("PrecioConDescuento"),
                        ImporteIGIC = p.Field<decimal>("ImporteIGIC"),
                        Total = p.Field<decimal>("Total"),
                        Fichero = p.Field<string>("Fichero"),
                        Cliente = new Cliente()
                        {
                            VC_CIF = p.Field<string>("VC_CIF"),
                            VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                            VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                            VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                            VC_TFNO = p.Field<string>("VC_TFNO"),
                            VC_FAX = p.Field<string>("VC_FAX"),
                            VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                            VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                            VC_POBLACION = p.Field<string>("VC_POBLACION"),
                            VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                            VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                            VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                            VC_ZONA = p.Field<string>("VC_ZONA"),
                            VC_PVP = p.Field<int>("VC_PVP"),
                            VC_HORA = p.Field<DateTime?>("VC_HORA")
                        }
                    }
                );
                pedido.Productos = new List<Producto>();
                ds.Tables[1].AsEnumerable().ToList().ForEach(
                    p => pedido.Productos.Add(new Producto()
                    {
                        ID = p.Field<long>("ID"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA"),
                        Cantidad = p.Field<int>("Cantidad"),
                        PrecioUnidad = p.Field<decimal>("PrecioUnidad"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        Ecotasa = p.Field<decimal>("Ecotasa"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                    }
                    ));

                Usuario usuario = PerfilUsuario(pedido.Cliente.VC_CLIENTE);
                List<string> direccionesCliente = new List<string>();
                if (usuario != null && usuario.NotificarPedido.Value && !string.IsNullOrEmpty(usuario.Email)) direccionesCliente.Add(usuario.Email);

                if (direccionesCliente.Count > 0)
                {
                    string cuerpo = "";
                    string estado = "";
                    switch (pedido.idEstado)
                    {
                        case 2:
                            cuerpo = ConstruyeCuerpoEmailAceptado(pedido.Cliente.VC_DENOMINACION, pedido.Referencia, pedido.FechaEnvio.Value.ToShortDateString());
                            estado = "ACEPTADO";
                            break;
                        case 3:
                            cuerpo = ConstruyeCuerpoEmailEntregado(pedido.Cliente.VC_DENOMINACION, pedido.Referencia, pedido.FechaEntrega.Value.ToShortDateString());
                            estado = "ENTREGADO";
                            break;
                        default:
                            cuerpo = "";
                            estado = "";
                            break;
                    }
                    if (!string.IsNullOrEmpty(cuerpo) && !string.IsNullOrEmpty(estado))
                    {
                        List<string> lsBcc = null;
                        //string bccDir = ConfigurationManager.AppSettings["emailBcc"];
                        //if (!string.IsNullOrEmpty(bccDir))
                        //{
                        //    lsBcc = new List<string>();
                        //    lsBcc.Add(bccDir);
                        //}
                        bool res = Send(direccionesCliente, null, lsBcc, string.Format("B2B NA: Su pedido Nro. {0} ha sido {1}", pedido.Referencia, estado), cuerpo, null);
                    }
                }
                #endregion
                return resul;
            }
            catch
            {
                return -1;
            }
        }

        public int AnularPedido(long idPedido)
        {
            try
            {
                int resul = DAL.AnularPedido(idPedido);
                if (resul <= 0) return resul;
                // envio email al usuario

                DataSet ds = DetallesPedido(idPedido);
                if (ds == null || ds.Tables.Count < 2) return -1; //2 porque devuelvo 2 tables

                Pedido pedido = null;
                ds.Tables[0].AsEnumerable().ToList().ForEach(
                    p => pedido = new Pedido()
                    {
                        idEstado = p.Field<int>("idEstado"),
                        Codigo = p.Field<string>("Codigo"),
                        Descripcion = p.Field<string>("Descripcion"),
                        idPedido = p.Field<long>("idPedido"),
                        Referencia = p.Field<string>("Referencia"),
                        VF_AlBARAN = p.Field<string>("VF_AlBARAN"),
                        Fecha = p.Field<DateTime?>("Fecha"),
                        FechaEnvio = p.Field<DateTime?>("FechaEnvio"),
                        FechaEntrega = p.Field<DateTime?>("FechaEntrega"),
                        PorAgencia = p.Field<bool?>("PorAgencia"),
                        DirEnvio = p.Field<string>("DirEnvio"),
                        BaseImponible = p.Field<decimal>("BaseImponible"),
                        Descuento = p.Field<decimal>("Descuento"),
                        NFU = p.Field<decimal>("NFU"),
                        //IGIC = p.Field<decimal>("IGIC"),
                        ImporteDescuento = p.Field<decimal>("ImporteDescuento"),
                        PrecioConDescuento = p.Field<decimal>("PrecioConDescuento"),
                        ImporteIGIC = p.Field<decimal>("ImporteIGIC"),
                        Total = p.Field<decimal>("Total"),
                        Fichero = p.Field<string>("Fichero"),
                        Cliente = new Cliente()
                        {
                            VC_CIF = p.Field<string>("VC_CIF"),
                            VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                            VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                            VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                            VC_TFNO = p.Field<string>("VC_TFNO"),
                            VC_FAX = p.Field<string>("VC_FAX"),
                            VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                            VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                            VC_POBLACION = p.Field<string>("VC_POBLACION"),
                            VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                            VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                            VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                            VC_ZONA = p.Field<string>("VC_ZONA"),
                            VC_PVP = p.Field<int>("VC_PVP"),
                            VC_HORA = p.Field<DateTime?>("VC_HORA")
                        }
                    }
                );
                pedido.Productos = new List<Producto>();
                ds.Tables[1].AsEnumerable().ToList().ForEach(
                    p => pedido.Productos.Add(new Producto()
                    {
                        ID = p.Field<long>("ID"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA"),
                        Cantidad = p.Field<int>("Cantidad"),
                        PrecioUnidad = p.Field<decimal>("PrecioUnidad"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        Ecotasa = p.Field<decimal>("Ecotasa"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                    }
                    ));

                Usuario usuario = PerfilUsuario(pedido.Cliente.VC_CLIENTE);
                List<string> direccionesCliente = new List<string>();
                if (usuario != null && usuario.NotificarPedido.Value && !string.IsNullOrEmpty(usuario.Email)) direccionesCliente.Add(usuario.Email);

                if (direccionesCliente.Count > 0)
                {
                    string cuerpo = "";
                    string estado = "";
                    switch (pedido.idEstado)
                    {
                        case 2:
                            cuerpo = ConstruyeCuerpoEmailAceptado(pedido.Cliente.VC_DENOMINACION, pedido.Referencia, pedido.FechaEnvio.Value.ToShortDateString());
                            estado = "ACEPTADO";
                            break;
                        case 3:
                            cuerpo = ConstruyeCuerpoEmailEntregado(pedido.Cliente.VC_DENOMINACION, pedido.Referencia, pedido.FechaEntrega.Value.ToShortDateString());
                            estado = "ENTREGADO";
                            break;
                        case 9:
                            cuerpo = ConstruyeCuerpoEmailAnulado(pedido.Cliente.VC_DENOMINACION, pedido.Referencia);
                            estado = "ANULADO";
                            break;
                        default:
                            cuerpo = "";
                            estado = "";
                            break;
                    }
                    if (!string.IsNullOrEmpty(cuerpo) && !string.IsNullOrEmpty(estado))
                    {
                        string bccDir = ConfigurationManager.AppSettings["emailBcc"];
                        List<string> lsBcc = null;
                        if (!string.IsNullOrEmpty(bccDir))
                        {
                            lsBcc = new List<string>();
                            lsBcc.Add(bccDir);
                        }
                        bool res = Send(direccionesCliente, null, lsBcc, string.Format("B2B NA: Su pedido Nro. {0} ha sido {1}", pedido.Referencia, estado), cuerpo, null);
                    }
                }
                return resul;
            }
            catch
            {
                return -1;
            }
        }

        public DataSet EstadoActualYSiguiente(int idEstado)
        {
            return DAL.EstadoActualYSiguiente(idEstado);
        }

        public bool EnviarCredencialesUsuario(Usuario usuario)
        {
            try
            {
                string contenidoEmail = ConstruyeCuerpoEmailEnvioCredenciales(usuario.cliente.VC_DENOMINACION
                                                                            , usuario.tipo, usuario.login, usuario.pass);
                List<string> dirEmail = new List<string>();
                dirEmail.Add(usuario.Email);
                return Send(dirEmail, null, null, "B2B NA: Envio de credenciales de usuario", contenidoEmail, null);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region RESERVAS

        public DataSet EstadosReservaCombo()
        {
            return DAL.EstadosReservaCombo();
        }

        public string ReservasGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                                string dirEnvio, decimal? baseImp, decimal? descuento,
                                decimal? nfu, decimal? igic, string observaciones, List<Producto> lsProductos)
        {
            string error = "";
            Reserva reserva = null;
            using (TransactionScope scope = new TransactionScope())
            {
                long idReserva = -1;
                DataSet ds = DAL.ReservasGuardar(idUsuario, fechaEnvio, fechaEntrega, porAgencia, dirEnvio, baseImp, descuento, nfu, igic, observaciones, ref idReserva);
                if (ds == null || ds.Tables.Count == 0) return "Error al guardar la reserva.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();

                if (idReserva <= 0) return "Error al guardar los datos de la reserva.";

                reserva = new Reserva();
                ds.Tables[1].AsEnumerable().ToList().ForEach(p =>
                {
                    reserva.idEstado = p.Field<int>("idEstado");
                    reserva.Descripcion = p.Field<string>("descripcion");
                    reserva.idReserva = p.Field<long>("idReserva");
                    reserva.Referencia = p.Field<string>("Referencia");
                    reserva.VF_AlBARAN = p.Field<string>("VF_ALBARAN");
                    reserva.Fecha = p.Field<DateTime?>("Fecha");
                    reserva.FechaEnvio = p.Field<DateTime?>("FechaEnvio");
                    reserva.FechaEntrega = p.Field<DateTime?>("FechaEntrega");
                    reserva.DirEnvio = p.Field<string>("DirEnvio");
                    reserva.BaseImponible = p.Field<decimal>("BaseImponible");
                    reserva.Descuento = p.Field<decimal>("Descuento");
                    reserva.NFU = p.Field<decimal>("NFU");
                    //reserva.IGIC = p.Field<decimal>("IGIC");
                    reserva.ImporteDescuento = p.Field<decimal>("ImporteDescuento");
                    reserva.PrecioConDescuento = p.Field<decimal>("PrecioConDescuento");
                    reserva.ImporteIGIC = p.Field<decimal>("ImporteIGIC");
                    //reserva.Total = p.Field<decimal>("Total");
                    reserva.Cliente = new Cliente()
                    {
                        VC_CIF = p.Field<string>("VC_CIF"),
                        VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                        VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                        VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                        VC_TFNO = p.Field<string>("VC_TFNO"),
                        VC_FAX = p.Field<string>("VC_FAX"),
                        VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                        VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                        VC_POBLACION = p.Field<string>("VC_POBLACION"),
                        VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                        VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                        VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                        VC_ZONA = p.Field<string>("VC_ZONA"),
                        VC_PVP = p.Field<int>("VC_PVP"),
                        VC_HORA = p.Field<DateTime?>("VC_HORA"),
                        VC_CODFORMAPAGO = p.Field<string>("VC_CODFORMAPAGO")
                    };
                    reserva.Observaciones = p.Field<string>("Observaciones");
                });

                // guardo los productos del pedido

                if (lsProductos == null || lsProductos.Count == 0) return "Error: No existen productos en la reserva.";

                reserva.Productos = lsProductos;

                lsProductos.ForEach(p =>
                {
                    if (ProductosReservaGuardar(idReserva, p.VP_PRODUCTO, p.Cantidad, p.PrecioUnidad, p.VP_CATEGORIA, p.Ecotasa, p.VP_PORC_IMP, p.VT_PORC_IMP) <= 0)
                    {
                        error = "Error al guardar los productos de la reserva.";
                        return;
                    }
                });

                scope.Complete();
            }
            if (string.IsNullOrEmpty(error)) // si no hay error, entonces genero PDF
            {
                //debo consultar los productos del pedido para que se carguen datos como el detalle de la ecotasa
                lsProductos = new List<Producto>();

                ProductosReservaGetData(reserva.idReserva).Tables[0].AsEnumerable().ToList().ForEach(
                    p => lsProductos.Add(new Producto()
                    {
                        ID = p.Field<long>("ID"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA"),
                        Cantidad = p.Field<int>("Cantidad"),
                        PrecioUnidad = p.Field<decimal>("PrecioUnidad"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        Ecotasa = p.Field<decimal>("Ecotasa"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                        ECOTASA_DETALLES = p.Field<string>("ECOTASA_DETALLES"),
                        VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP"),
                        VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP")
                    }
                ));
                reserva.Productos = lsProductos;
                string att = "";
                if (!GenerarReservaPDF("reserva", reserva, ref att))
                {
                    error = "Error al generar copia de la reserva en formato PDF.";
                }
                else //envio el pdf por email
                {
                    List<string> direccionesNeuatlan = new List<string>();
                    string emailReservas = ConfigurationManager.AppSettings["emailReservas"];
                    if (!string.IsNullOrEmpty(emailReservas)) direccionesNeuatlan.Add(emailReservas);

                    Usuario usuario = PerfilUsuario(reserva.Cliente.VC_CLIENTE);
                    List<string> direccionesCliente = new List<string>();
                    if (usuario != null && usuario.NotificarPedido.Value && !string.IsNullOrEmpty(usuario.Email)) direccionesCliente.Add(usuario.Email);

                    if (direccionesNeuatlan.Count > 0 && !string.IsNullOrEmpty(att))
                    {
                        string cuerpo = ConstruyeCuerpoEmailRecibidoReserva(reserva.Cliente.VC_DENOMINACION, reserva.Referencia);
                        bool res = Send(direccionesNeuatlan, null, direccionesCliente, string.Format("B2B NA: Su reserva Nro. {0} ha sido RECIBIDA", reserva.Referencia), cuerpo, att);
                    }
                }
            }

            return error;

        }

        private string ConstruyeCuerpoEmailRecibidoReserva(string cliente, string nReserva)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su reserva&nbsp;<b><i>Nro. token_n_reserva</i></b>&nbsp;ha sido “<b>RECIBIDA”</b>&nbsp;en nuestro&nbsp;<b>Call Center de Atención al Cliente</b> y se encuentra en estado <b>PENDIENTE DE CONFIRMACIÓN</b>, por lo que en breves minutos recibirá un mensaje electrónico, confirmando la disponibilidad de los artículos solicitados y le &nbsp;indicará si su reserva ha sido “<b>ACEPTADA</b>.”<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_reserva", nReserva);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ConstruyeCuerpoEmailAceptadoReserva(string cliente, string nreserva, string fechaEnvio)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su reserva&nbsp;<b><i>Nro. token_n_reserva</i></b>&nbsp;ha sido <b>“ACEPTADA”</b>&nbsp;en nuestro&nbsp;<b>Call Center de Atención al Cliente</b> con fecha programada de env&iacute;o para el d&iacute;a token_fecha_envio.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_reserva", nreserva).Replace("token_fecha_envio", fechaEnvio);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ConstruyeCuerpoEmailEntregadoReserva(string cliente, string nreserva, string fechaEntrega)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su reserva&nbsp;<b><i>Nro. token_n_reserva</i></b>&nbsp;ha sido <b>“ENTREGADA”</b> con fecha token_fecha_entrega. En caso de existir alguna observaci&oacute;n no dude en ponerse en contacto con nuestro <b>Call Center de Atención al Cliente</b> al tel&eacute;fono 902 402 044.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_reserva", nreserva).Replace("token_fecha_entrega", fechaEntrega);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ConstruyeCuerpoEmailAnuladoReserva(string cliente, string nreserva)
        {
            try
            {
                string plantilla = "<span class='Apple-style-span' style='border-collapse: collapse; font-family: arial, sans-serif; '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Señores:&nbsp;<b>token_nombre_cliente<u></u><u></u></b></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Su reserva&nbsp;<b><i>Nro. token_n_reserva</i></b>&nbsp;ha sido <b>“ANULADA”</b>&nbsp;en nuestro&nbsp;<b>Call Center de Atención al Cliente</b>.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '>Gracias por confiar en nosotros….<br/><br/>Mensaje generado automáticamente por el B2B de Neumáticos Atlántico.<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u>&nbsp;<u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><span style='font-size: 11pt; color: rgb(31, 73, 125); '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; '><b><span style='font-size: 7.5pt; color: navy; '><img width='216' height='43' src='http://82.159.238.192/B2bAqua20Ws/logo_email.png' alt='Descripción: Descripción: Descripción: cid:image001.jpg@01CAF342.A35D43F0'></span></b><b><span style='font-size: 7.5pt; color: navy; '><u></u><u></u></span></b></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 10pt; color: navy; '>Dpto. Atención al Cliente<u></u><u></u></span></b></p><div class='im' style='color: rgb(80, 0, 80); '><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span lang='ES-TRAD' style='font-size: 10pt; color: navy; '>NEUMATICOS ATLANTICO,&nbsp;S.L</span></b><b><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '>.</span></b><span lang='ES-TRAD' style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='EN-US' style='font-size: 9pt; color: navy; '>C/ Gestur, S/N. Edif..&nbsp;</span><span style='font-size: 9pt; color: navy; '>Atlanticoautocentros<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Polg. La Campana. Cod. Postal 38109<u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>El Rosario - Santa Cruz de Tenerife</span><span style='color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span lang='ES-TRAD' style='font-size: 9pt; color: navy; '>Tlfnos: 922 615061/626282</span><span lang='ES-TRAD' style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; color: navy; '>Fax: 922 623549</span><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '>e-mail:&nbsp;</span><u><span style='font-size: 9pt; font-family: Garamond, serif; color: blue; '><a href='mailto:b2b@neuatlan.com' target='_blank' style='color: rgb(66, 99, 171); '>b2b@neuatlan.com</a></span></u><span style='font-size: 9pt; font-family: Garamond, serif; color: navy; '><u></u><u></u></span></p><div class='im' style='color: rgb(80, 0, 80); '><div class='MsoNormal' align='center' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: center; '><span style='font-size: 11pt; color: navy; '><hr size='2' width='100%' align='center'></span></div><p class='MsoNormal' style='margin-top: 0px; margin-right: 0cm; margin-bottom: 5pt; margin-left: 0cm; '><b><span style='font-size: 7.5pt; color: navy; '>Este mensaje y los ficheros anexos son confidenciales. Los mismos contienen información reservada que no puede ser difundida. Si usted ha recibido este correo por error, tenga la amabilidad de eliminarlo de su sistema y avisar al remitente mediante reenvío a su dirección electrónica; no deberá copiar el mensaje ni divulgar su contenido a ninguna persona.</span></b><span style='font-size: 11pt; color: navy; '><u></u><u></u></span></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Su dirección de correo electrónica junto con sus datos personales constan en un fichero titularidad de NEUMÁTICOS ATLÁNTICO, S.L.&nbsp; cuya finalidad es la de mantener el contacto con usted y hacerles llegar las propuestas de servicios o productos. Si quiere saber qué información disponemos de usted, modificarla, oponerse y, en su caso, cancelarla, puede hacerlo enviando un escrito al efecto, acompañado de una fotocopia de su DNI a la siguiente dirección: NEUMÁTICOS ATLÁNTICO, S.L., calle Prolongación de Sau Paulo, s/n, Urbanización Industrial La Campana, calle Gestur sin número, 38109, El Rosario- Tenerife.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo, se le advierte que toda la información personal contenida en este mensaje se encuentra protegida por la Ley 15/1999, de 13 de Diciembre de protección de datos de carácter personal, quedando totalmente prohibido su uso y/o tratamiento, así como la cesión de aquella a terceros al margen de lo dispuesto en la citada ley protectora de datos personales y de su normativa de desarrollo.<u></u><u></u></span></b></p><p class='MsoNormal' style='margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; text-align: justify; '><b><span style='font-size: 7.5pt; color: navy; '>Asimismo es su responsabilidad comprobar que este mensaje o sus archivos adjuntos no contengan virus informático.</span></b></p></div></span>";
                return plantilla.Replace("token_nombre_cliente", cliente).Replace("token_n_reserva", nreserva);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet ProductosReservaGetData(long idReserva)
        {
            return DAL.ProductosReservaGetData(idReserva);
        }

        public DataSet ReservasData(long? idReserva, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE)
        {
            return DAL.ReservasData(idReserva, idEstado, fechaDesde, fechaHasta, VC_CLIENTE);
        }

        public int ActualizarEstadoReserva(long idReserva, DateTime? fecha)
        {
            try
            {
                int resul = DAL.ActualizarEstadoReserva(idReserva, fecha);
                if (resul <= 0) return resul;
                // envio email al usuario
                #region
                DataSet ds = DetallesReserva(idReserva);
                if (ds == null || ds.Tables.Count < 2) return -1; //2 porque devuelvo 2 tables

                Reserva reserva = null;
                ds.Tables[0].AsEnumerable().ToList().ForEach(
                    p => reserva = new Reserva()
                    {
                        idEstado = p.Field<int>("idEstado"),
                        Codigo = p.Field<string>("Codigo"),
                        Descripcion = p.Field<string>("Descripcion"),
                        idReserva = p.Field<long>("idReserva"),
                        Referencia = p.Field<string>("Referencia"),
                        VF_AlBARAN = p.Field<string>("VF_AlBARAN"),
                        Fecha = p.Field<DateTime?>("Fecha"),
                        FechaEnvio = p.Field<DateTime?>("FechaEnvio"),
                        FechaEntrega = p.Field<DateTime?>("FechaEntrega"),
                        PorAgencia = p.Field<bool?>("PorAgencia"),
                        DirEnvio = p.Field<string>("DirEnvio"),
                        BaseImponible = p.Field<decimal>("BaseImponible"),
                        Descuento = p.Field<decimal>("Descuento"),
                        NFU = p.Field<decimal>("NFU"),
                        //IGIC = p.Field<decimal>("IGIC"),
                        ImporteDescuento = p.Field<decimal>("ImporteDescuento"),
                        PrecioConDescuento = p.Field<decimal>("PrecioConDescuento"),
                        ImporteIGIC = p.Field<decimal>("ImporteIGIC"),
                        Total = p.Field<decimal>("Total"),
                        Fichero = p.Field<string>("Fichero"),
                        Cliente = new Cliente()
                        {
                            VC_CIF = p.Field<string>("VC_CIF"),
                            VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                            VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                            VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                            VC_TFNO = p.Field<string>("VC_TFNO"),
                            VC_FAX = p.Field<string>("VC_FAX"),
                            VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                            VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                            VC_POBLACION = p.Field<string>("VC_POBLACION"),
                            VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                            VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                            VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                            VC_ZONA = p.Field<string>("VC_ZONA"),
                            VC_PVP = p.Field<int>("VC_PVP"),
                            VC_HORA = p.Field<DateTime?>("VC_HORA")
                        }
                    }
                );
                reserva.Productos = new List<Producto>();
                ds.Tables[1].AsEnumerable().ToList().ForEach(
                    p => reserva.Productos.Add(new Producto()
                    {
                        ID = p.Field<long>("ID"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA"),
                        Cantidad = p.Field<int>("Cantidad"),
                        PrecioUnidad = p.Field<decimal>("PrecioUnidad"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        Ecotasa = p.Field<decimal>("Ecotasa"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                    }
                    ));

                Usuario usuario = PerfilUsuario(reserva.Cliente.VC_CLIENTE);
                List<string> direccionesCliente = new List<string>();
                if (usuario != null && usuario.NotificarPedido.Value && !string.IsNullOrEmpty(usuario.Email)) direccionesCliente.Add(usuario.Email);

                if (direccionesCliente.Count > 0)
                {
                    string cuerpo = "";
                    string estado = "";
                    switch (reserva.idEstado)
                    {
                        case 2:
                            cuerpo = ConstruyeCuerpoEmailAceptadoReserva(reserva.Cliente.VC_DENOMINACION, reserva.Referencia, reserva.FechaEnvio.Value.ToShortDateString());
                            estado = "ACEPTADA";
                            break;
                        case 3:
                            cuerpo = ConstruyeCuerpoEmailEntregadoReserva(reserva.Cliente.VC_DENOMINACION, reserva.Referencia, reserva.FechaEntrega.Value.ToShortDateString());
                            estado = "ENTREGADA";
                            break;
                        default:
                            cuerpo = "";
                            estado = "";
                            break;
                    }
                    if (!string.IsNullOrEmpty(cuerpo) && !string.IsNullOrEmpty(estado))
                    {
                        List<string> lsBcc = null;
                        //string bccDir = ConfigurationManager.AppSettings["emailBcc"];
                        //if (!string.IsNullOrEmpty(bccDir))
                        //{
                        //    lsBcc = new List<string>();
                        //    lsBcc.Add(bccDir);
                        //}
                        bool res = Send(direccionesCliente, null, lsBcc, string.Format("B2B NA: Su reserva Nro. {0} ha sido {1}", reserva.Referencia, estado), cuerpo, null);
                    }
                }
                #endregion
                return resul;
            }
            catch
            {
                return -1;
            }
        }

        public int AnularReserva(long idReserva)
        {
            try
            {
                int resul = DAL.AnularReserva(idReserva);
                if (resul <= 0) return resul;
                // envio email al usuario

                DataSet ds = DetallesReserva(idReserva);
                if (ds == null || ds.Tables.Count < 2) return -1; //2 porque devuelvo 2 tables

                Reserva reserva = null;
                ds.Tables[0].AsEnumerable().ToList().ForEach(
                    p => reserva = new Reserva()
                    {
                        idEstado = p.Field<int>("idEstado"),
                        Codigo = p.Field<string>("Codigo"),
                        Descripcion = p.Field<string>("Descripcion"),
                        idReserva = p.Field<long>("idReserva"),
                        Referencia = p.Field<string>("Referencia"),
                        VF_AlBARAN = p.Field<string>("VF_AlBARAN"),
                        Fecha = p.Field<DateTime?>("Fecha"),
                        FechaEnvio = p.Field<DateTime?>("FechaEnvio"),
                        FechaEntrega = p.Field<DateTime?>("FechaEntrega"),
                        PorAgencia = p.Field<bool?>("PorAgencia"),
                        DirEnvio = p.Field<string>("DirEnvio"),
                        BaseImponible = p.Field<decimal>("BaseImponible"),
                        Descuento = p.Field<decimal>("Descuento"),
                        NFU = p.Field<decimal>("NFU"),
                        //IGIC = p.Field<decimal>("IGIC"),
                        ImporteDescuento = p.Field<decimal>("ImporteDescuento"),
                        PrecioConDescuento = p.Field<decimal>("PrecioConDescuento"),
                        ImporteIGIC = p.Field<decimal>("ImporteIGIC"),
                        Total = p.Field<decimal>("Total"),
                        Fichero = p.Field<string>("Fichero"),
                        Cliente = new Cliente()
                        {
                            VC_CIF = p.Field<string>("VC_CIF"),
                            VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                            VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                            VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                            VC_TFNO = p.Field<string>("VC_TFNO"),
                            VC_FAX = p.Field<string>("VC_FAX"),
                            VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                            VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                            VC_POBLACION = p.Field<string>("VC_POBLACION"),
                            VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                            VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                            VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                            VC_ZONA = p.Field<string>("VC_ZONA"),
                            VC_PVP = p.Field<int>("VC_PVP"),
                            VC_HORA = p.Field<DateTime?>("VC_HORA")
                        }
                    }
                );
                reserva.Productos = new List<Producto>();
                ds.Tables[1].AsEnumerable().ToList().ForEach(
                    p => reserva.Productos.Add(new Producto()
                    {
                        ID = p.Field<long>("ID"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA"),
                        Cantidad = p.Field<int>("Cantidad"),
                        PrecioUnidad = p.Field<decimal>("PrecioUnidad"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        Ecotasa = p.Field<decimal>("Ecotasa"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                    }
                    ));

                Usuario usuario = PerfilUsuario(reserva.Cliente.VC_CLIENTE);
                List<string> direccionesCliente = new List<string>();
                if (usuario != null && usuario.NotificarPedido.Value && !string.IsNullOrEmpty(usuario.Email)) direccionesCliente.Add(usuario.Email);

                if (direccionesCliente.Count > 0)
                {
                    string cuerpo = "";
                    string estado = "";
                    switch (reserva.idEstado)
                    {
                        case 2:
                            cuerpo = ConstruyeCuerpoEmailAceptadoReserva(reserva.Cliente.VC_DENOMINACION, reserva.Referencia, reserva.FechaEnvio.Value.ToShortDateString());
                            estado = "ACEPTADA";
                            break;
                        case 3:
                            cuerpo = ConstruyeCuerpoEmailEntregadoReserva(reserva.Cliente.VC_DENOMINACION, reserva.Referencia, reserva.FechaEntrega.Value.ToShortDateString());
                            estado = "ENTREGADA";
                            break;
                        case 9:
                            cuerpo = ConstruyeCuerpoEmailAnulado(reserva.Cliente.VC_DENOMINACION, reserva.Referencia);
                            estado = "ANULADA";
                            break;
                        default:
                            cuerpo = "";
                            estado = "";
                            break;
                    }
                    if (!string.IsNullOrEmpty(cuerpo) && !string.IsNullOrEmpty(estado))
                    {
                        string bccDir = ConfigurationManager.AppSettings["emailBcc"];
                        List<string> lsBcc = null;
                        if (!string.IsNullOrEmpty(bccDir))
                        {
                            lsBcc = new List<string>();
                            lsBcc.Add(bccDir);
                        }
                        bool res = Send(direccionesCliente, null, lsBcc, string.Format("B2B NA: Su reserva Nro. {0} ha sido {1}", reserva.Referencia, estado), cuerpo, null);
                    }
                }
                return resul;
            }
            catch
            {
                return -1;
            }
        }

        public DataSet EstadoReservaActualYSiguiente(int idEstado)
        {
            return DAL.EstadoReservaActualYSiguiente(idEstado);
        }

        #endregion

        #region PRODUCTOS

        public DataSet ProductosBuscadorStaff(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2)
        {
            return DAL.ProductosBuscadorStaff(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2);
        }

        public DataSet ProductosBuscador(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2)
        {
            return DAL.ProductosBuscador(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2);
        }

        public DataSet ProductosBuscadorV3(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2, 
                                           int pagina, int regPagina, ref int nFilasTotales, string ordenarPor, string ordenAscDesc)
        { 
            return DAL.ProductosBuscadorV3(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2, pagina, regPagina, ref nFilasTotales, ordenarPor, ordenAscDesc);
        }

        public DataSet ProductosGuardar(string familia, string descfam, string producto, string descripcion
                                      , string producto1, string modelo, decimal? serie, decimal? llanta, string medida
                                      , string ic, string iv, decimal? tipo_neuma, string desc_tipo
                                      , decimal pvp1, decimal pvp2, decimal pvp3, int tipo_ofer, string categoria
                                      , decimal VP_PORC_IMP, string VP_IMAGEN, int? VP_IMPORTADO
                                      , decimal VP_NIVELRUIDO, string VP_EFICOMBUSTIBLE, string VP_ADHERENCIA, decimal VP_VALORRUIDO)
        {
            return DAL.ProductosGuardar(familia, descfam, producto, descripcion, producto1, modelo, serie, llanta, medida, ic, iv, tipo_neuma, desc_tipo, pvp1, pvp2, pvp3, tipo_ofer, categoria, VP_PORC_IMP, VP_IMAGEN, VP_IMPORTADO, VP_NIVELRUIDO, VP_EFICOMBUSTIBLE, VP_ADHERENCIA, VP_VALORRUIDO);
        }

        public DataSet ProductosData(string VP_FAMILIA, string VP_PRODUCTO, DateTime? MODIFICADO)
        {
            return DAL.ProductosData(VP_FAMILIA, VP_PRODUCTO, MODIFICADO);
        }

        public DataSet ProductosDataV3(string VP_PRODUCTO)
        {
            return DAL.ProductosDataV3(VP_PRODUCTO);
        }

        #endregion

        #region PRODUCTOS PEDIDO

        private int ProductosPedidoGuardar(long idPedido, string VP_PRODUCTO, int Unidades, decimal PrecioUnidad, string VP_CATEGORIA, decimal Ecotasa, decimal VP_PORC_IMP, decimal VT_PORC_IMP)
        {
            return DAL.ProductosPedidoGuardar(idPedido, VP_PRODUCTO, Unidades, PrecioUnidad, VP_CATEGORIA, Ecotasa, VT_PORC_IMP, VP_PORC_IMP);
        }

        private int ProductosReservaGuardar(long idReserva, string VP_PRODUCTO, int Unidades, decimal PrecioUnidad, string VP_CATEGORIA, decimal Ecotasa, decimal VP_PORC_IMP, decimal VT_PORC_IMP)
        {
            return DAL.ProductosReservaGuardar(idReserva, VP_PRODUCTO, Unidades, PrecioUnidad, VP_CATEGORIA, Ecotasa, VT_PORC_IMP, VP_PORC_IMP);
        }

        public DataSet DetallesPedido(long idPedido)
        {
            return DAL.DetallesPedido(idPedido);
        }

        public DataSet DetallesReserva(long idReserva)
        {
            return DAL.DetallesReserva(idReserva);
        }

        #endregion

        #region PENDIENTES

        public DataSet PendientesPorProductoData(string producto)
        {
            return DAL.PendientesPorProductoData(producto);
        }

        public DataSet PendientesGetData(DateTime? llegada)
        {
            return DAL.PendientesGetData(llegada);
        }

        public DataSet PendientesGuardar(string articulo, DateTime llegada, string contenedor)
        {
            return DAL.PendientesGuardar(articulo, llegada, contenedor);
        }

        public int PendientesBorrarSiExiste(string articulo)
        {
            return DAL.PendientesBorrarSiExiste(articulo);
        }

        #endregion

        #region EMAIL

        public bool Send(List<string> addressesTo, List<string> addressesCc, List<string> addressesBcc, string subject, string messageBody, string att)
        {
            try
            {
                //if (string.IsNullOrEmpty(att)) 
                //    return Email.Send(addressesCc, addressesBcc, subject, messageBody);
                //else 
                return Email.Send(addressesTo, addressesCc, addressesBcc, subject, messageBody, att);
            }
            catch
            {
                return false;
            }
        }

        public bool Send(string addressesFrom, string subject, string messageBody)
        {
            try
            {
                return Email.Send(addressesFrom, subject, messageBody);
            }
            catch
            {
                return false;
            }
        }

        public bool Send(string addressesFrom, string addressTo, string subject, string messageBody)
        {
            try
            {
                return Email.Send(addressesFrom, addressTo, subject, messageBody);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region COMBOS

        public DataSet FamiliasLogoCombo()
        {
            return DAL.FamiliasLogoCombo();
        }

        public DataSet FamiliasCombo()
        {
            return DAL.FamiliasCombo();
        }

        public DataSet ModelosCombo(string familia)
        {
            return DAL.ModelosCombo(familia);
        }

        public DataSet TipoNeumaticosCombo()
        {
            return DAL.TipoNeumaticosCombo();
        }

        #endregion

        #region ECOTASA

        public DataSet EcotasasData()
        {
            return DAL.EcotasasData();
        }

        public DataSet EcotasaGuardar(string categoria, string descripcion, string detalles, decimal pvp1, decimal VT_PORC_IMP)
        {
            return DAL.EcotasaGuardar(categoria, descripcion, detalles, pvp1, VT_PORC_IMP);
        }

        public int EcotasaBorrarSiExiste(string categoria)
        {
            return DAL.EcotasaBorrarSiExiste(categoria);
        }

        #endregion

        #region STOCK

        public DataSet StockData()
        {
            return DAL.StockData();
        }

        //public DataSet StockGuardar(string familia, string descfamilia, string articulo, string descarticulo, string articulo1
        //                           , string modelo, string indvelocidad, string indcarga, string categoria
        //                           , decimal stock_a01, decimal stock_a02, decimal stock_a03, decimal stock_a04, decimal stock_a18
        //                           , decimal stock_a19, decimal stock_a22, decimal stock_a23, decimal stock_a32
        //                           , decimal stock_a44, decimal stock_a54, decimal stock_gen, decimal stock_a12
        //                           , decimal stock_a13, decimal stock_a55, decimal stock_a24
        //                           , decimal stock_a27, decimal stock_a29, decimal stock_a31, decimal stock_a43
        //                           , decimal stock_a45, decimal stock_a46, decimal stock_a47, decimal stock_a56
        //                           , decimal stock_a53, decimal stock_a60, decimal stock_a63
        //                            )
        //{
        //    return DAL.StockGuardar(familia, descfamilia, articulo, descarticulo, articulo1, modelo, indvelocidad, indcarga, categoria, stock_a01, stock_a02, stock_a03, stock_a04, stock_a18, stock_a19, stock_a22, stock_a23, stock_a32, stock_a44, stock_a54, stock_gen, stock_a12, stock_a13, stock_a55, stock_a24, stock_a27, stock_a29, stock_a31, stock_a43, stock_a45, stock_a46, stock_a47, stock_a56);
        //}

        #endregion

        #region PROMOCIONES

        public DataSet PromocionesData(Promocion promocion)
        {
            return DAL.PromocionesData(promocion);
        }

        public DataSet PromocionesGuardar(Promocion promocion)
        {
            return DAL.PromocionesGuardar(promocion);
        }

        public DataSet PromocionesEliminar(int idPromo)
        {
            return DAL.PromocionesEliminar(idPromo);
        }

        #endregion

    }
}
