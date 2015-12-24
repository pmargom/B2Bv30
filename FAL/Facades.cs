using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using B2B.Types;
using B2B.Generic;
using System.Xml.Serialization;
using System.Configuration;
using System.IO;

namespace Facades
{
    public class Facades
    {
        private BusinessRules.BusinessRules mBusinessRules;

        public BusinessRules.BusinessRules BAL
        {
            get
            {
                if (this.mBusinessRules == null)
                {
                    this.mBusinessRules = new BusinessRules.BusinessRules();
                }
                return this.mBusinessRules;
            }
        }

        public Facades()
        {
        }

        #region MENSAJES 

        public string MensajeMarcarComoLeido(int idMensaje)
        {
            try
            {
                string error = "";
                int res = BAL.MensajeMarcarComoLeido(idMensaje);
                if (res <= 0) error = "Al actualizar la fecha de envío del mensaje";
                return error;
            }
            catch
            {
                return "Al guardar el mensaje";
            }
        }

        public int MensajeGuardar(Mensaje mensaje)
        {
            return BAL.MensajeGuardar(mensaje);
        }

        public List<Mensaje> MensajesData(Mensaje mensaje)
        {
            try
            {
                DataSet ds = BAL.MensajesData(mensaje);
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) return null;
                List<Mensaje> ls = new List<Mensaje>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new Mensaje()
                {
                    Asunto = p.Field<string>("Asunto"),
                    Contenido = p.Field<string>("Contenido"),
                    DestinatarioIdUsuario = p.Field<int>("DestinatarioIdUsuario"),
                    Destinatario = p.Field<string>("Destinatario"),
                    FechaEnvio = p.Field<DateTime?>("FechaEnvio"),
                    idMensaje = p.Field<int>("idMensaje"),
                    Leido = p.Field<bool>("Leido"),
                    RemitenteIdUsuario = p.Field<int>("RemitenteIdUsuario"),
                    Remitente = p.Field<string>("Remitente"),
                    Buzon = new Buzon()
                    {
                        Color = p.Field<string>("Color"),
                        Denominacion = p.Field<string>("Denominacion"),
                        idBuzon = p.Field<int>("idBuzon")
                    }
                }));
                return ls;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region SQL Server Sync

        public DataSet SqlCleanTables()
        {
            return BAL.SqlCleanTables();
        }

        public DataSet SqlGuardar(string DBTable, string parametros)
        {
            return BAL.SqlGuardar(DBTable, parametros);
        }

        public DataSet Sql_Sincronizar(DateTime fechaNuevaSync, int tiempo)
        {
            return BAL.Sql_Sincronizar(fechaNuevaSync, tiempo);
        }

        public DataSet Sql_SyncUpdate(DateTime fechaSync)
        {
            return BAL.Sql_SyncUpdate(fechaSync);
        }

        public void SyncLastUpdate(bool SyncNocturna)
        {
            BAL.SyncLastUpdate(SyncNocturna);
        }

        public DateTime? SyncLastDate(bool SyncNocturna)
        {
            try
            {
                DataSet ds = BAL.SyncLastDate(SyncNocturna);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;

                return DateTime.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region FAMILIAS LOGOS

        private void EliminarFamiliaLogo(string fileName)
        {
            try
            {
                string LogosDir = ConfigurationManager.AppSettings["LogosDir"];

                string path = string.Format(@"{0}{1}\{2}", AppDomain.CurrentDomain.BaseDirectory, LogosDir, fileName);

                if (File.Exists(path)) File.Delete(path);

            }
            catch
            {

            }
        }

        public List<Familia> FamiliasLogoData(Familia familia)
        {
            try
            {
                DataSet ds = BAL.FamiliasLogoData(familia);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Familia> ls = new List<Familia>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Familia()
                    {
                        VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        VF_FAMILIA = p.Field<string>("VF_FAMILIA"),
                        VF_LOGO = p.Field<string>("VF_LOGO")
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public string FamiliasLogoGuardar(Familia familia)
        {
            string error = string.Empty;
            try
            {
                string logoAnterior = "";

                //localizo el logo anterior si lo hubiera
                List<Familia> ls = FamiliasLogoData(new Familia() { VF_FAMILIA = familia.VF_FAMILIA });
                if (ls != null && ls.Count > 0) logoAnterior = ls[0].VF_LOGO;
               
                //guardo los datos de la familia
                DataSet ds = BAL.FamiliasLogoGuardar(familia);
                if (ds == null || ds.Tables.Count == 0) return "Error al guardar datos.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();

                // si no hay error, entonces miro a ver si tengo que eliminar el fichero del logo anterior si lo hay
                if (!string.IsNullOrEmpty(logoAnterior) && logoAnterior != familia.VF_LOGO) // está editando entonces tengo que capturar los ficheros de banner para poder eliminarlos
                {
                    EliminarFamiliaLogo(logoAnterior);
                }

                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al guardar los datos.";
            }
        }

        public string FamiliasLogoEliminar(string VF_FAMILIA)
        {
            string error = string.Empty;
            try
            {
                Familia familia = FamiliasLogoData(new Familia() { VF_FAMILIA = VF_FAMILIA })[0];

                DataSet ds = BAL.FamiliasLogoEliminar(VF_FAMILIA);

                if (ds == null || ds.Tables.Count == 0) return "Error al eliminar.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();

                // si no hay error, entonces trato de eliminar los ficheros de logo
                if (!string.IsNullOrEmpty(familia.VF_LOGO)) EliminarFamiliaLogo (familia.VF_LOGO);

                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al eliminar los datos.";
            }
        }

        #endregion

        #region CLIENTES VS PEDIDOS

        public List<ClientesVsPedidos> ClientesVsPedidosData(DateTime? FechaDesde, DateTime? FechaHasta, string clienteDesde, string clienteHasta)
        {
            try
            {
                DataSet ds = BAL.ClientesVsPedidosData(FechaDesde, FechaHasta, clienteDesde, clienteHasta);
                DataTable dt = ds.Tables[0];
                List<ClientesVsPedidos> ls = new List<ClientesVsPedidos>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new ClientesVsPedidos()
                {
                    PedidosTotales = p.Field<int>("PedidosTotales"),
                    UnidadesTotales = p.Field<int>("UnidadesTotales"),
                    idCliente = p.Field<decimal>("ID"),
                    VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                    VC_NOMBRE = p.Field<string>("VC_NOMBRE")
                }
                ));
                return ls;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region SESIONES

        public List<Sesion> SesionesData(Sesion sesion)
        {
            try
            {
                DataSet ds = BAL.SesionesData(sesion);
                DataTable dt = ds.Tables[0];
                List<Sesion> ls = new List<Sesion>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new Sesion()
                {
                    FechaInicio = p.Field<DateTime?>("FechaInicio"),
                    FechaFin = p.Field<DateTime?>("FechaFin"),
                    idSesion = p.Field<long?>("idSesion"),
                    idUsuario = p.Field<int?>("idUsuario"),
                    NombreUsuario = p.Field<string>("NombreUsuario"),
                    NPedidos = p.Field<int?>("NPedidos"),
                    VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                    VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                }
                ));
                return ls;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region PEDIDOS POR CLIENTE

        public List<PedidoXCliente> PedidosXClientesData(DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            try
            {
                DataSet ds = BAL.PedidosXClientesData(FechaDesde, FechaHasta, VC_CLIENTE);
                DataTable dt = ds.Tables[0];
                List<PedidoXCliente> ls = new List<PedidoXCliente>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new PedidoXCliente()
                {
                    Fecha = p.Field<DateTime>("Fecha"),
                    Referencia = p.Field<string>("Referencia"),
                    BaseImponible = p.Field<decimal>("BaseImponible"),
                    Codigo = p.Field<string>("Codigo"),
                    Descripcion = p.Field<string>("Descripcion"),
                    Descuento = p.Field<decimal>("Descuento"),
                    idCliente = p.Field<decimal>("ID"),
                    idEstado = p.Field<int>("idEstado"),
                    idPedido = p.Field<long>("idPedido"),
                    ImporteDescuento = p.Field<decimal>("ImporteDescuento"),
                    ImporteIGIC = p.Field<decimal>("ImporteIGIC"),
                    NFU = p.Field<decimal>("NFU"),
                    PrecioConDescuento = p.Field<decimal>("PrecioConDescuento"),
                    Total = p.Field<decimal>("Total"),
                    Unidades = p.Field<int>("Unidades"),
                    VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                    VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                    VC_NOMBRE = p.Field<string>("VC_NOMBRE")
                }
                ));
                return ls;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region BUSQUEDAS FALLIDAS

        public List<BusquedaFallida> BusquedasFallidasData(string Referencia, DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            try
            {
                DataSet ds = BAL.BusquedasFallidasData(Referencia, FechaDesde, FechaHasta, VC_CLIENTE);
                DataTable dt = ds.Tables[0];
                List<BusquedaFallida> ls = new List<BusquedaFallida>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new BusquedaFallida()
                {
                    Referencia = p.Field<string>("Referencia"),
                    Fecha = p.Field<DateTime>("Fecha"),
                    VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                    VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                    AlmacenPreferido = p.Field<string>("AlmacenPreferido")
                }
                ));
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public int BusquedasFallidasGuardar(string Referencia, string VC_CLIENTE)
        {
            return BAL.BusquedasFallidasGuardar(Referencia, VC_CLIENTE);
        }

        public int BusquedasFallidasEliminar(string Referencia)
        {
            return BAL.BusquedasFallidasEliminar(Referencia);
        }

        #endregion

        #region CONFIGURACION

        public List<ParametroConfig> ConfigData(string parametro)
        {
            try
            {
                DataSet ds = BAL.ConfigData(parametro);
                DataTable parametrosConfig = ds.Tables[0];
                List<ParametroConfig> lsParametrosConfig = new List<ParametroConfig>();
                parametrosConfig.AsEnumerable().ToList().ForEach(p => lsParametrosConfig.Add(new ParametroConfig()
                {
                    Nombre = p.Field<string>("Nombre"),
                    parametro = p.Field<string>("parametro"),
                    valor = p.Field<string>("valor")
                }
                ));
                return lsParametrosConfig;
            }
            catch
            {
                return null;
            }
        }

        public int ConfiguracionGuardar(string parametro, string valor)
        {
            return BAL.ConfiguracionGuardar(parametro, valor);
        }

        #endregion

        #region FACTURAS

        public List<Factura> FacturasData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                DataSet ds = BAL.FacturasData(cliente, fechaDesde, fechaHasta);
                DataTable facturas = ds.Tables[0];
                List<Factura> lsFacturas = new List<Factura>();
                facturas.AsEnumerable().ToList().ForEach(p => lsFacturas.Add(new Factura()
                            {
                                VF_CLIENTE = p.Field<string>("VF_CLIENTE"),
                                VF_NOMBRE_CLIENTE = p.Field<string>("VF_NOMBRE_CLIENTE"),
                                VF_FACTURA = p.Field<string>("VF_FACTURA"),
                                VF_FECHA_FACT = p.Field<DateTime?>("VF_FECHA_FACT"),
                                //VF_UNIDADES = p.Field<int>("VF_UNIDADES"),
                                //VF_PVENTA = p.Field<decimal>("VF_PVENTA"),
                                //VF_PORC_DCTO = p.Field<decimal>("VF_PORC_DCTO"),
                                //VF_IMP_DCTO = p.Field<decimal>("VF_IMP_DCTO"),
                                VF_NETO = p.Field<decimal>("VF_NETO"),
                                //VF_PORC_IGIC = p.Field<decimal>("VF_PORC_IGIC"),
                                FicheroFactura = p.Field<string>("FicheroFactura")
                                //FicheroAlbaran = p.Field<string>("FicheroAlbaran")
                            }
                ));
                return lsFacturas;
            }
            catch
            {
                return null;
            }
        }

        public List<Factura> FacturasDataDesglosada(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                DataSet ds = BAL.FacturasDataDesglosada(cliente, fechaDesde, fechaHasta);
                DataTable facturas = ds.Tables[0];
                List<Factura> lsFacturas = new List<Factura>();
                facturas.AsEnumerable().ToList().ForEach(p => lsFacturas.Add(new Factura()
                {
                    ID = p.Field<int>("ID"),
                    VF_HORA = p.Field<DateTime>("VF_HORA"),
                    VF_CLIENTE = p.Field<string>("VF_CLIENTE"),
                    VF_NOMBRE_CLIENTE = p.Field<string>("VF_NOMBRE_CLIENTE"),
                    VF_FACTURA = p.Field<string>("VF_FACTURA"),
                    VF_FECHA_FACT = p.Field<DateTime?>("VF_FECHA_FACT"),
                    VF_ALBARAN = p.Field<string>("VF_ALBARAN"),
                    VF_FECHA_ALB = p.Field<DateTime?>("VF_FECHA_ALB"),
                    VF_ARTICULO = p.Field<string>("VF_ARTICULO"),
                    VF_DESCARTICULO = p.Field<string>("VF_DESCARTICULO"),
                    VF_CATEGORIA = p.Field<string>("VF_CATEGORIA"),
                    VF_UNIDADES = p.Field<decimal>("VF_UNIDADES"),
                    VF_PVENTA = p.Field<decimal>("VF_PVENTA"),
                    VF_PORC_DCTO = p.Field<decimal>("VF_PORC_DCTO"),
                    VF_IMP_DCTO = p.Field<decimal>("VF_IMP_DCTO"),
                    VF_NETO = p.Field<decimal>("VF_NETO"),
                    VF_PORC_IGIC = p.Field<decimal>("VF_PORC_IGIC"),
                    FicheroFactura = p.Field<string>("FicheroFactura"),
                    FicheroAlbaran = p.Field<string>("FicheroAlbaran"),
                    VF_LINEA = p.Field<decimal>("VF_LINEA")
                }
                ));
                return lsFacturas;
            }
            catch
            {
                return null;
            }
        }

        public List<Factura> FacturaData(int idFactura)
        {
            try
            {
                DataSet ds = BAL.FacturaData(idFactura);
                DataTable facturas = ds.Tables[0];
                List<Factura> lsFacturas = new List<Factura>();
                facturas.AsEnumerable().ToList().ForEach(p => lsFacturas.Add(new Factura()
                            {
                                ID = p.Field<int>("ID"),
                                VF_CLIENTE = p.Field<string>("VF_CLIENTE"),
                                VF_FACTURA = p.Field<string>("VF_FACTURA"),
                                VF_ALBARAN = p.Field<string>("VF_ALBARAN"),
                                VF_FECHA_FACT = p.Field<DateTime?>("VF_FECHA_FACT"),
                                IMP_BRUTO = p.Field<decimal>("IMP_BRUTO"),
                                DTO = p.Field<decimal>("DTO"),
                                IGIC = p.Field<decimal>("IGIC"),
                                NFU = p.Field<string>("NFU"),
                                TOTAL = p.Field<decimal>("TOTAL"),
                                FicheroFactura = p.Field<string>("FicheroFactura"),
                                FicheroAlbaran = p.Field<string>("FicheroAlbaran")
                            }
                ));
                return lsFacturas;
            }
            catch
            {
                return null;
            }
        }

        public List<Factura> FacturasGuardar(string cliente, string nombre_cliente, string factura, DateTime? fecha_fact, string albaran
                             , DateTime? fecha_alb, string articulo, string descarticulo, string categoria, decimal unidades
                             , decimal pventa, decimal porc_dcto, decimal imp_dcto, decimal neto, decimal porc_igic, decimal VF_LINEA)
        {
            try
            {
                DataSet ds = BAL.FacturasGuardar(cliente, nombre_cliente, factura, fecha_fact, albaran, fecha_alb, articulo, descarticulo, categoria, unidades, pventa, porc_dcto, imp_dcto, neto, porc_igic, VF_LINEA);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Factura> ls = new List<Factura>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Factura()
                    {
                        VF_HORA = p.Field<DateTime>("VF_HORA"),
                        ID = p.Field<int>("ID"),
                        VF_ALBARAN = p.Field<string>("VF_ALBARAN"),
                        VF_ARTICULO = p.Field<string>("VF_ARTICULO"),
                        VF_FECHA_FACT = p.Field<DateTime?>("VF_FECHA_FACT"),
                        VF_CATEGORIA = p.Field<string>("VF_CATEGORIA"),
                        VF_CLIENTE = p.Field<string>("VF_CLIENTE"),
                        VF_DESCARTICULO = p.Field<string>("VF_DESCARTICULO"),
                        VF_FACTURA = p.Field<string>("VF_FACTURA"),
                        VF_FECHA_ALB = p.Field<DateTime?>("VF_FECHA_ALB"),
                        VF_IMP_DCTO = p.Field<decimal>("VF_IMP_DCTO"),
                        VF_NETO = p.Field<decimal>("VF_NETO"),
                        VF_NOMBRE_CLIENTE = p.Field<string>("VF_NOMBRE_CLIENTE"),
                        VF_PORC_DCTO = p.Field<decimal>("VF_PORC_DCTO"),
                        VF_PORC_IGIC = p.Field<decimal>("VF_PORC_IGIC"),
                        VF_PVENTA = p.Field<decimal>("VF_PVENTA"),
                        VF_UNIDADES = p.Field<decimal>("VF_UNIDADES")
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public int FacturaActualizarFichero(string VF_FACTURA)
        {
            return BAL.FacturaActualizarFichero(VF_FACTURA);
        }

        public int FacturasBorrarSiExiste(string factura, string albaran, decimal VF_LINEA)
        {
            return BAL.FacturasBorrarSiExiste(factura, albaran, VF_LINEA);
        }

        public int FacturasPorClienteBorrar(string cliente)
        {
            return BAL.FacturasPorClienteBorrar(cliente);
        }

        #endregion

        #region ALBARANES

        public List<Factura> AlbaranesData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta, bool? showAll)
        {
            try
            {
                DataSet ds = BAL.AlbaranesData(cliente, fechaDesde, fechaHasta, showAll);
                DataTable albaranes = ds.Tables[0];
                List<Factura> lsAlbaranes = new List<Factura>();
                albaranes.AsEnumerable().ToList().ForEach(p => lsAlbaranes.Add(new Factura()
                {
                    //ID = p.Field<int>("ID"),
                    VF_CLIENTE = p.Field<string>("VF_CLIENTE"),
                    VF_NOMBRE_CLIENTE = p.Field<string>("VF_NOMBRE_CLIENTE"),
                    VF_FACTURA = p.Field<string>("VF_FACTURA"),
                    VF_ALBARAN = p.Field<string>("VF_ALBARAN"),
                    VF_FECHA_ALB = p.Field<DateTime>("VF_FECHA_ALB"),
                    //IMP_BRUTO = p.Field<decimal>("IMP_BRUTO"),
                    VF_NETO = p.Field<decimal>("VF_NETO"),
                    //DTO = p.Field<decimal>("DTO"),
                    //IGIC = p.Field<decimal>("IGIC"),
                    //NFU = p.Field<string>("NFU"),
                    //TOTAL = p.Field<decimal>("TOTAL"),
                    //FicheroFactura = p.Field<string>("FicheroFactura"),
                    FicheroAlbaran = p.Field<string>("FicheroAlbaran")
                }
                ));
                return lsAlbaranes;
            }
            catch
            {
                return null;
            }
        }

        public int AlbaranActualizarFichero(string VF_ALBARAN)
        {
            return BAL.AlbaranActualizarFichero(VF_ALBARAN);
        }

        #endregion

        #region VENCIMIENTOS

        public List<Vencimiento> VencimientosData(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                DataSet ds = BAL.VencimientosData(cliente, fechaDesde, fechaHasta);
                DataTable vencimientos = ds.Tables[0];
                List<Vencimiento> lsVencimientos = new List<Vencimiento>();
                vencimientos.AsEnumerable().ToList().ForEach(p => lsVencimientos.Add(new Vencimiento()
                {
                    ID = p.Field<int>("ID"),
                    VE_CLIENTE = p.Field<decimal>("VE_CLIENTE"),
                    VE_DOCUMENTO = p.Field<long>("VE_DOCUMENTO"),
                    VE_EMISION = p.Field<DateTime>("VE_EMISION"),
                    VE_FACTURA = p.Field<string>("VE_FACTURA"),
                    VE_IMPORTE = p.Field<decimal>("VE_IMPORTE"),
                    VE_TIPO_DOC = p.Field<string>("VE_TIPO_DOC"),
                    VE_VENCIMIENTO = p.Field<DateTime>("VE_VENCIMIENTO"),
                    MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                    Fichero = p.Field<string>("Fichero"),
                    VE_ESTADO = p.Field<int>("VE_ESTADO")
                }
                ));
                return lsVencimientos;
            }
            catch
            {
                return null;
            }
        }

        public List<Vencimiento> VencimientosDataDesglosada(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                DataSet ds = BAL.VencimientosDataDesglosada(cliente, fechaDesde, fechaHasta);
                DataTable vencimientos = ds.Tables[0];
                List<Vencimiento> lsVencimientos = new List<Vencimiento>();
                vencimientos.AsEnumerable().ToList().ForEach(p => lsVencimientos.Add(new Vencimiento()
                {
                    ID = p.Field<int>("ID"),
                    VE_CLIENTE = p.Field<decimal>("VE_CLIENTE"),
                    VE_DOCUMENTO = p.Field<long>("VE_DOCUMENTO"),
                    VE_EMISION = p.Field<DateTime>("VE_EMISION"),
                    VE_FACTURA = p.Field<string>("VE_FACTURA"),
                    VE_IMPORTE = p.Field<decimal>("VE_IMPORTE"),
                    VE_TIPO_DOC = p.Field<string>("VE_TIPO_DOC"),
                    VE_VENCIMIENTO = p.Field<DateTime>("VE_VENCIMIENTO"),
                    MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                    Fichero = p.Field<string>("Fichero"),
                    VE_ESTADO = p.Field<int>("VE_ESTADO")
                }
                ));
                return lsVencimientos;
            }
            catch
            {
                return null;
            }
        }

        public List<Vencimiento> VencimientosGuardar(decimal cliente, string factura, long documento, string tipo_doc,
                           DateTime emision, DateTime vencimiento, decimal importe, decimal estado)
        {
            try
            {
                DataSet ds = BAL.VencimientosGuardar(cliente, factura, documento, tipo_doc, emision, vencimiento, importe, estado);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Vencimiento> ls = new List<Vencimiento>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Vencimiento()
                    {
                        ID = p.Field<int>("ID"),
                        VE_CLIENTE = p.Field<decimal>("VE_CLIENTE"),
                        VE_DOCUMENTO = p.Field<long>("VE_DOCUMENTO"),
                        VE_EMISION = p.Field<DateTime>("VE_EMISION"),
                        VE_FACTURA = p.Field<string>("VE_FACTURA"),
                        VE_IMPORTE = p.Field<decimal>("VE_IMPORTE"),
                        VE_TIPO_DOC = p.Field<string>("VE_TIPO_DOC"),
                        VE_VENCIMIENTO = p.Field<DateTime>("VE_VENCIMIENTO"),
                        MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                        VE_ESTADO = p.Field<int>("VE_ESTADO")
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public int EfectoActualizarFichero(string VF_DOCUMENTO)
        {
            return BAL.EfectoActualizarFichero(VF_DOCUMENTO);
        }

        public int VencimientosBorrarSiExiste(long documento)
        {
            return BAL.VencimientosBorrarSiExiste(documento);
        }

        #endregion

        #region CLIENTES

        public List<Combo> ClientesCombo()
        {
            try
            {
                DataSet ds = BAL.ClientesCombo();
                DataTable clientes = ds.Tables[0];
                if (clientes == null || clientes.Rows.Count == 0) return null;
                List<Combo> lsClientes = new List<Combo>();
                clientes.AsEnumerable().ToList().ForEach(p => lsClientes.Add(new Combo()
                {
                    ValueMember = p.Field<string>("VC_CLIENTE"),
                    DisplayMember = p.Field<string>("VC_NOMBRE"),
                }));
                return lsClientes;
            }
            catch
            {
                return null;
            }
        }

        public List<Cliente> ClientesData()
        {
            try
            {
                DataSet ds = BAL.ClientesData();
                DataTable clientes = ds.Tables[0];
                if (clientes == null || clientes.Rows.Count == 0) return null;
                List<Cliente> lsClientes = new List<Cliente>();
                clientes.AsEnumerable().ToList().ForEach(p => lsClientes.Add(new Cliente()
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
                    VC_CODFORMAPAGO = p.Field<string>("VC_CODFORMAPAGO"),
                    VC_BANCO = p.Field<string>("VC_BANCO"),
                    VC_BANCO_CUENTA = p.Field<string>("VC_BANCO_CUENTA"),
                    VC_BANCO_DIRECCION = p.Field<string>("VC_BANCO_DIRECCION"),
                    VC_BANCO_POBLACION = p.Field<string>("VC_BANCO_POBLACION")
                }));
                return lsClientes;
            }
            catch
            {
                return null;
            }
        }

        public List<Cliente> ClientesGuardar(string cliente, string nombre, string denominacion, string cif, string direccion
                     , string direccionb, string tfno, string fax, string poblacion, string provincia
                     , string codpostal, string formapago, string zona, int pvp, string VC_CODFORMAPAGO, string VC_BANCO, string VC_BANCO_DIRECCION, string VC_BANCO_POBLACION, string VC_BANCO_CUENTA)
        {
            try
            {
                DataSet ds = BAL.ClientesGuardar(cliente, nombre, denominacion, cif, direccion, direccionb, tfno, fax, poblacion, provincia, codpostal, formapago, zona, pvp, VC_CODFORMAPAGO, VC_BANCO, VC_BANCO_DIRECCION, VC_BANCO_POBLACION, VC_BANCO_CUENTA);
                DataTable clientes = ds.Tables[0];
                if (clientes == null || clientes.Rows.Count == 0) return null;
                List<Cliente> lsClientes = new List<Cliente>();
                clientes.AsEnumerable().ToList().ForEach(p => lsClientes.Add(new Cliente()
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
                    VC_BANCO = p.Field<string>("VC_BANCO"),
                    VC_BANCO_CUENTA = p.Field<string>("VC_BANCO_CUENTA"),
                    VC_BANCO_DIRECCION = p.Field<string>("VC_BANCO_DIRECCION"),
                    VC_BANCO_POBLACION = p.Field<string>("VC_BANCO_POBLACION")
                }));
                return lsClientes;
            }
            catch
            {
                return null;
            }
        }

        public int ClientesBorrarSiExiste(string cliente)
        {
            return BAL.ClientesBorrarSiExiste(cliente);
        }

        #endregion

        //#region MENSAJES

        //public List<Mensaje> MensajesData(int? idBuzon, int? idMensaje, DateTime? FechaCreacionDesde, DateTime? FechaCreacionHasta,
        //                            DateTime? FechaEnvioDesde, DateTime? FechaEnvioHasta, int? idUsuarioRemitente,
        //                            int? idUsuarioDestinatario, bool? Leido)
        //{
        //    try
        //    {
        //        DataSet ds = BAL.MensajesData(idBuzon, idMensaje, FechaCreacionDesde, FechaCreacionHasta, FechaEnvioDesde, FechaEnvioHasta,
        //                            idUsuarioRemitente, idUsuarioDestinatario, Leido);
        //        DataTable dt = ds.Tables[0];
        //        if (dt == null || dt.Rows.Count == 0) return null;
        //        List<Mensaje> ls = new List<Mensaje>();
        //        dt.AsEnumerable().ToList().ForEach(p => ls.Add(new Mensaje()
        //        {
        //            Asunto = p.Field<string>("Asunto"),
        //            Contenido = p.Field<string>("Contenido"),
        //            DestinatarioEmail = p.Field<string>("DestinatarioEmail"),
        //            DestinatarioIdUsuario = p.Field<int>("DestinatarioIdUsuario"),
        //            DestinatarioUsuario = p.Field<string>("DestinatarioUsuario"),
        //            FechaCreacion = p.Field<DateTime>("FechaCreacion"),
        //            FechaEnvio = p.Field<DateTime?>("FechaEnvio"),
        //            idMensaje = p.Field<int>("idMensaje"),
        //            Leido = p.Field<bool>("Leido"),
        //            RemitenteEmail = p.Field<string>("RemitenteEmail"),
        //            RemitenteIdUsuario = p.Field<int>("RemitenteIdUsuario"),
        //            RemitenteUsuario = p.Field<string>("RemitenteUsuario"),
        //            buzon = new Buzon()
        //            {
        //                Color = p.Field<string>("Color"),
        //                Denominacion = p.Field<string>("Denominacion"),
        //                idBuzon = p.Field<int>("idBuzon")
        //            }
        //        }));
        //        return ls;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public string MensajeGuardar(int idBuzon, int RemitenteIdUsuario, int DestinatarioIdUsuario, string Asunto, string Contenido, bool EnviarCopiaPorEmail)
        //{
        //    try
        //    {
        //        string error = "";
        //        int res = -1;

        //        if (EnviarCopiaPorEmail)
        //        {
        //            DataSet ds = BAL.MensajeGuardarEnviarPorEmail(idBuzon, RemitenteIdUsuario, DestinatarioIdUsuario, Asunto, Contenido);
        //            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                int idNuevoMensaje = ds.Tables[0].Rows[0]["idNuevoMensaje"].Parse<Int32>();
        //                string RemitenteEmail = ds.Tables[0].Rows[0]["RemitenteEmail"].ToString();
        //                string DestinatarioEmail = ds.Tables[0].Rows[0]["DestinatarioEmail"].ToString();
        //                // enviar email...
        //                if (!Send(RemitenteEmail, DestinatarioEmail, Asunto, Contenido))
        //                {
        //                    return "Al enviar copia del mensaje por email";
        //                }
        //                // el mensaje se envió, entonces actualizo la fecha de envio
        //                DateTime? fechaEnvio = DateTime.Now;
        //                res = BAL.MensajeGuardar(idNuevoMensaje, fechaEnvio);
        //                if (res <= 0) error = "Al actualizar la fecha de envío el mensaje";
        //                return error;
        //            }
        //        }
        //        else
        //        {
        //            res = BAL.MensajeGuardar(idBuzon, RemitenteIdUsuario, DestinatarioIdUsuario, Asunto, Contenido);
        //            if (res <= 0) error = "Al guardar el mensaje";
        //        }
        //        return error;
        //    }
        //    catch
        //    {
        //        return "Al guardar el mensaje";
        //    }
        //}

        //public string MensajeGuardar(int idMensaje, DateTime? FechaEnvio)
        //{
        //    try
        //    {
        //        string error = "";
        //        int res = BAL.MensajeGuardar(idMensaje, FechaEnvio);
        //        if (res <= 0) error = "Al actualizar la fecha de envío del mensaje";
        //        return error;
        //    }
        //    catch
        //    {
        //        return "Al guardar el mensaje";
        //    }
        //}

        //public string MensajeMarcarComoLeido(int idMensaje)
        //{
        //    try
        //    {
        //        string error = "";
        //        int res = BAL.MensajeMarcarComoLeido(idMensaje);
        //        if (res <= 0) error = "Al actualizar la fecha de envío del mensaje";
        //        return error;
        //    }
        //    catch
        //    {
        //        return "Al guardar el mensaje";
        //    }

        //}

        //#endregion

        #region BUZON

        public List<Combo> BuzonesCombo()
        {
            try
            {
                DataSet ds = BAL.BuzonesCombo();
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) return null;
                List<Combo> ls = new List<Combo>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new Combo()
                {
                    ValueMember = p.Field<int>("idBuzon").Parse<string>(),
                    DisplayMember = p.Field<string>("Denominacion"),
                }));
                return ls;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region USUARIOS

        public List<Combo> listaTiposUsuario()
        {
            try
            {
                DataSet ds = BAL.listaTiposUsuario();
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) return null;
                List<Combo> ls = new List<Combo>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new Combo()
                {
                    ValueMember = p.Field<int>("idTipo").ToString(),
                    DisplayMember = p.Field<string>("Denominacion"),
                }));
                return ls;
            }
            catch
            {
                return null;
            }           
        }

        public List<Combo> listaUsuariosCombo(string tipo)
        {
            try
            {
                DataSet ds = BAL.listaUsuariosCombo(tipo);
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) return null;
                List<Combo> ls = new List<Combo>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new Combo()
                {
                    ValueMember = p.Field<int>("idUsuario").ToString(),
                    DisplayMember = p.Field<string>("login"),
                }));
                return ls;
            }
            catch
            {
                return null;
            }            
        }

        public List<Combo> listaRolesCombo()
        {
            try
            {
                DataSet ds = BAL.listaRolesCombo();
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) return null;
                List<Combo> ls = new List<Combo>();
                dt.AsEnumerable().ToList().ForEach(p => ls.Add(new Combo()
                {
                    ValueMember = p.Field<int>("idTipo").ToString(),
                    DisplayMember = p.Field<string>("Denominacion"),
                }));
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public Usuario ObtenerDatosUsuario (int idUsuario)
        {
            try
            {
                DataSet ds = BAL.ObtenerDatosUsuario(idUsuario);
                DataTable usuarios = ds.Tables[0];
                if (usuarios == null || usuarios.Rows.Count == 0) return null;
                List<Usuario> lsUsuarios = new List<Usuario>();
                usuarios.AsEnumerable().ToList().ForEach(p => lsUsuarios.Add(new Usuario()
                {
                    idUsuario = p.Field<int>("idUsuario"),
                    tipo = p.Field<int>("tipo").Parse<TipoUsuario>(),
                    login = p.Field<string>("login"),
                    Bloqueado = p.Field<bool>("Bloqueado"),
                    VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                    VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                    cliente = new Cliente()
                    {
                        ID = p.Field<object>("ID").Parse<Int32>(),
                        VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                        VC_CIF = p.Field<string>("VC_CIF"),
                        VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                        VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                        VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                        VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                        VC_FAX = p.Field<string>("VC_FAX"),
                        VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                        VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                        VC_POBLACION = p.Field<string>("VC_POBLACION"),
                        VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                        VC_PVP = p.Field<object>("VC_PVP").Parse<Int32>(),
                        VC_TFNO = p.Field<string>("VC_TFNO"),
                        VC_ZONA = p.Field<string>("VC_ZONA")
                    },
                    AlmacenPreferido = p.Field<string>("AlmacenPreferido"),
                    ConfirmarAuto = p.Field<bool?>("ConfirmarAuto"),
                    Email = p.Field<string>("Email"),
                    GenerarPdfPedido = p.Field<bool?>("GenerarPdfPedido"),
                    MostrarMinuaturas = p.Field<bool?>("MostrarMinuaturas"),
                    NotificarPedido = p.Field<bool?>("NotificarPedido"),
                    NRuedasPreselec = p.Field<int?>("NRuedasPreselec"),
                    DtoB2B = p.Field<decimal>("DtoB2B"),
                    UltimoAcceso = p.Field<DateTime?>("UltimoAcceso"),
                    TipoBusqueda = GetTipoBusquedaFromXML(p.Field<string>("TipoBusqueda")),
                    Permisos = ObtenerListaPermisosPorUsuario(BAL.PermisosData(p.Field<int>("idUsuario"), null, null).Tables[0])
                }));
                return lsUsuarios[0];
            }
            catch 
            {
                return null;
            }
        }

        public List<Usuario> UsuariosData(bool? showAdmin, bool? showStaff, bool? showClientes, bool? bloqueado)
        {
            try
            {
                DataSet ds = BAL.UsuariosData(showAdmin, showStaff, showClientes, bloqueado);
                DataTable usuarios = ds.Tables[0];
                if (usuarios == null || usuarios.Rows.Count == 0) return null;
                List<Usuario> lsUsuarios = new List<Usuario>();
                usuarios.AsEnumerable().ToList().ForEach(p => lsUsuarios.Add(new Usuario()
                {
                    idUsuario = p.Field<int>("idUsuario"),
                    tipo = p.Field<int>("tipo").Parse<TipoUsuario>(),
                    login = p.Field<string>("login"),
                    Bloqueado = p.Field<bool>("Bloqueado"),
                    VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                    VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                    cliente = new Cliente()
                    {
                        ID = p.Field<object>("ID").Parse<Int32>(),
                        VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                        VC_CIF = p.Field<string>("VC_CIF"),
                        VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                        VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                        VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                        VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                        VC_FAX = p.Field<string>("VC_FAX"),
                        VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                        VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                        VC_POBLACION = p.Field<string>("VC_POBLACION"),
                        VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                        VC_PVP = p.Field<object>("VC_PVP").Parse<Int32>(),
                        VC_TFNO = p.Field<string>("VC_TFNO"),
                        VC_ZONA = p.Field<string>("VC_ZONA")
                    },
                    AlmacenPreferido = p.Field<string>("AlmacenPreferido"),
                    ConfirmarAuto = p.Field<bool?>("ConfirmarAuto"),
                    Email = p.Field<string>("Email"),
                    GenerarPdfPedido = p.Field<bool?>("GenerarPdfPedido"),
                    MostrarMinuaturas = p.Field<bool?>("MostrarMinuaturas"),
                    NotificarPedido = p.Field<bool?>("NotificarPedido"),
                    NRuedasPreselec = p.Field<int?>("NRuedasPreselec"),
                    DtoB2B = p.Field<decimal>("DtoB2B"),
                    UltimoAcceso = p.Field<DateTime?>("UltimoAcceso"),
                    TipoBusqueda = GetTipoBusquedaFromXML(p.Field<string>("TipoBusqueda")),
                    Permisos = ObtenerListaPermisosPorUsuario(BAL.PermisosData(p.Field<int>("idUsuario"), null, null).Tables[0])
                }));
                return lsUsuarios;
            }
            catch
            {
                return null;
            }
        }

        private List<Permiso> ObtenerListaPermisosPorUsuario(DataTable dt)
        {
            if (dt == null) return null;
            List<Permiso> ls = new List<Permiso>();
            dt.AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Permiso()
                    {
                        CodModulo = p.Field<string>("CodModulo"),
                        Consultar = p.Field<bool>("Consultar"),
                        Crear = p.Field<bool>("Crear"),
                        Editar = p.Field<bool>("Editar"),
                        Eliminar = p.Field<bool>("Eliminar"),
                        idRole = p.Field<int>("idRole"),
                        Modulo = new Modulo()
                        {
                            CodModulo = p.Field<string>("CodModulo"),
                            Denominacion = p.Field<string>("DenoMod")
                        },
                        Rol = new Role()
                        {
                            Denominacion = p.Field<string>("DenoRol"),
                            idRole = p.Field<int>("idRole")
                        }
                    });
                });
            return ls;
        }

        private TipoBusqueda GetTipoBusquedaFromXML(string tipoBusqueda)
        {
            try
            {
                if (string.IsNullOrEmpty(tipoBusqueda)) return new TipoBusqueda();

                XmlSerializer xs = new XmlSerializer(typeof(TipoBusqueda));
                return xs.Deserialize(new System.IO.StringReader(tipoBusqueda)) as TipoBusqueda;
            }
            catch (Exception)
            {
                return null;    
            }
        }

        private string GetXMLFromTipoBusqueda(TipoBusqueda tipoBusqueda)
        {
            try
            {
                if (tipoBusqueda == null) return null;

                XmlSerializer xs = new XmlSerializer(typeof(TipoBusqueda));
                System.IO.StringWriter sw = new System.IO.StringWriter();
                xs.Serialize(sw, tipoBusqueda);
                return sw.GetStringBuilder().ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Usuario Login(string userName, string pass)
        {
            try
            {
                DataSet ds = BAL.Login(userName, pass);
                DataTable usuarios = ds.Tables[0];
                if (usuarios == null || usuarios.Rows.Count == 0) return null;
                List<Usuario> lsUsuarios = new List<Usuario>();
                usuarios.AsEnumerable().ToList().ForEach(p => lsUsuarios.Add(new Usuario()
                {
                    idUsuario = p.Field<int>("idUsuario"),
                    tipo = p.Field<int>("tipo").Parse<TipoUsuario>(),
                    login = p.Field<string>("login"),
                    pass = p.Field<string>("pass"),
                    Bloqueado = p.Field<bool>("Bloqueado"),
                    VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                    cliente = new Cliente()
                    {
                        ID = p.Field<object>("ID").Parse<Int32>(),
                        VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                        VC_CIF = p.Field<string>("VC_CIF"),
                        VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                        VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                        VC_FAX = p.Field<string>("VC_FAX"),
                        VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                        VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                        VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                        VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                        VC_POBLACION = p.Field<string>("VC_POBLACION"),
                        VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                        VC_PVP = p.Field<object>("VC_PVP").Parse<Int32>(),
                        VC_TFNO = p.Field<string>("VC_TFNO"),
                        VC_ZONA = p.Field<string>("VC_ZONA")
                    },
                    AlmacenPreferido = p.Field<string>("AlmacenPreferido"),
                    ConfirmarAuto = p.Field<bool?>("ConfirmarAuto"),
                    Email = p.Field<string>("Email"),
                    GenerarPdfPedido = p.Field<bool?>("GenerarPdfPedido"),
                    MostrarMinuaturas = p.Field<bool?>("MostrarMinuaturas"),
                    NotificarPedido = p.Field<bool?>("NotificarPedido"),
                    NRuedasPreselec = p.Field<int?>("NRuedasPreselec"),
                    DtoB2B = p.Field<decimal>("DtoB2B"),
                    UltimoAcceso = p.Field<DateTime?>("UltimoAcceso"),
                    TipoBusqueda = GetTipoBusquedaFromXML(p.Field<string>("TipoBusqueda")),
                    idSesion = p.Field<long>("idSesion")
                }));
                //establezco los permisos del usuario
                if (lsUsuarios.Count == 1)
                {
                    List<Permiso> permisos = new List<Permiso>();
                    ds.Tables[1].AsEnumerable().ToList().ForEach(p =>
                        {
                            permisos.Add(new Permiso()
                            {
                                idRole = p.Field<int>("idRole"),
                                CodModulo = p.Field<string>("CodModulo"),
                                Consultar = p.Field<bool>("Consultar"),
                                Crear = p.Field<bool>("Crear"),
                                Editar = p.Field<bool>("Editar"),
                                Eliminar = p.Field<bool>("Eliminar"),
                                Rol = new Role()
                                {
                                    idRole = p.Field<int>("idRole"),
                                    Denominacion = p.Field<string>("DenoRol"),
                                },
                                Modulo = new Modulo()
                                {
                                    CodModulo = p.Field<string>("CodModulo"),
                                    Denominacion = p.Field<string>("DenoMod"),
                                }
                            });
                        });
                    lsUsuarios[0].Permisos = permisos;
                }
                return lsUsuarios.ToArray()[0];
            }
            catch
            {
                return null;
            }
        }

        public int Logout(long idSesion)
        {
            return BAL.Logout(idSesion);
        }

        public string UsuariosGuardar(int? idUsuario, string login, string pass, int? tipo, string VC_CLIENTE, bool? bloqueado,
                    string AlmacenPreferido, bool? ConfirmarAuto, string Email, bool? GenerarPdfPedido, bool? MostrarMinuaturas,
                    bool? NotificarPedido, int? NRuedasPreselec, decimal DtoB2B, bool EnviarCreadenciaesPorEmail
                    , TipoBusqueda TipoBusqueda)
        {
            string error = string.Empty;
            try
            {
                DataSet ds = BAL.UsuariosGuardar(idUsuario, login, pass, tipo, VC_CLIENTE, bloqueado, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
                    MostrarMinuaturas, NotificarPedido, NRuedasPreselec, DtoB2B, TipoBusqueda != null ? TipoBusqueda.Serialize() : null);
                if (ds == null || ds.Tables.Count == 0) return "Error al guardar datos.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();
                // el usuario se guardó correctamente, entonces envio credenciales por email
                if (EnviarCreadenciaesPorEmail)
                {
                    #region
                    Usuario usuario = null;
                    usuario = (from p in ds.Tables[1].AsEnumerable()
                               select new Usuario()
                                  {
                                      idUsuario = p.Field<int>("idUsuario"),
                                      tipo = p.Field<int>("tipo").Parse<TipoUsuario>(),
                                      login = p.Field<string>("login"),
                                      pass = p.Field<string>("pass"),
                                      Bloqueado = p.Field<bool>("Bloqueado"),
                                      VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                                      cliente = new Cliente()
                                      {
                                          ID = p.Field<object>("ID").Parse<Int32>(),
                                          VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                                          VC_CIF = p.Field<string>("VC_CIF"),
                                          VC_CODPOSTAL = p.Field<string>("VC_CODPOSTAL"),
                                          VC_DENOMINACION = p.Field<string>("VC_DENOMINACION"),
                                          VC_FAX = p.Field<string>("VC_FAX"),
                                          VC_FORMAPAGO = p.Field<string>("VC_FORMAPAGO"),
                                          VC_NOMBRE = p.Field<string>("VC_NOMBRE"),
                                          VC_DIRECCION = p.Field<string>("VC_DIRECCION"),
                                          VC_DIRECCIONB = p.Field<string>("VC_DIRECCIONB"),
                                          VC_POBLACION = p.Field<string>("VC_POBLACION"),
                                          VC_PROVINCIA = p.Field<string>("VC_PROVINCIA"),
                                          VC_PVP = p.Field<object>("VC_PVP").Parse<Int32>(),
                                          VC_TFNO = p.Field<string>("VC_TFNO"),
                                          VC_ZONA = p.Field<string>("VC_ZONA")
                                      },
                                      AlmacenPreferido = p.Field<string>("AlmacenPreferido"),
                                      ConfirmarAuto = p.Field<bool?>("ConfirmarAuto"),
                                      Email = p.Field<string>("Email"),
                                      GenerarPdfPedido = p.Field<bool?>("GenerarPdfPedido"),
                                      MostrarMinuaturas = p.Field<bool?>("MostrarMinuaturas"),
                                      NotificarPedido = p.Field<bool?>("NotificarPedido"),
                                      NRuedasPreselec = p.Field<int?>("NRuedasPreselec"),
                                      DtoB2B = p.Field<decimal>("DtoB2B"),
                                      UltimoAcceso = p.Field<DateTime?>("UltimoAcceso"),
                                      TipoBusqueda = GetTipoBusquedaFromXML(p.Field<string>("TipoBusqueda"))
                                  }).ToList()[0];
                    if (!BAL.EnviarCredencialesUsuario(usuario)) return "false"; // para reconocer que el usuario se creo pero que falló el envío de correo
                    #endregion
                }
                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al guardar los datos.";
            }
        }

        public string UsuariosEliminar(int idUsuario)
        {
            string error = string.Empty;
            try
            {
                DataSet ds = BAL.UsuariosEliminar(idUsuario);
                if (ds == null || ds.Tables.Count == 0) return "Error al eliminar.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();
                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al eliminar los datos.";
            }
        }

        public string UsuarioTipoBusquedaGuardar(int idUsuario, TipoBusqueda tipoBusqueda)
        {
            string error = string.Empty;
            try
            {
                DataSet ds = BAL.UsuarioTipoBusquedaGuardar(idUsuario, tipoBusqueda);
                if (ds == null || ds.Tables.Count == 0) return "Error al guardar.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();
                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al eliminar los datos.";
            }
        }

        public string PerfilUsuarioGuardar(int? idUsuario, string AlmacenPreferido, bool? ConfirmarAuto, string Email,
            bool? GenerarPdfPedido, bool? MostrarMinuaturas, bool? NotificarPedido, int? NRuedasPreselec)
        {
            string error = string.Empty;
            try
            {
                DataSet ds = BAL.PerfilUsuarioGuardar(idUsuario, AlmacenPreferido, ConfirmarAuto, Email, GenerarPdfPedido,
                                                 MostrarMinuaturas, NotificarPedido, NRuedasPreselec);
                if (ds == null || ds.Tables.Count == 0) return "Error al guardar datos.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();
                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al guardar los datos.";
            }
        }

        public int CambiarPassword(string login, string pass, string newPass)
        {
            return BAL.CambiarPassword(login, pass, newPass);
        }

        public int PermisosGuardar(int idUsuario, string xmlPermisos)
        {
            return BAL.PermisosGuardar(idUsuario, xmlPermisos);
        }

        #endregion

        #region PEDIDOS

        public List<Estado> EstadosCombo()
        {
            try
            {
                DataSet ds = BAL.EstadosCombo();
                DataTable estados = ds.Tables[0];
                if (estados == null || estados.Rows.Count == 0) return null;
                var query = from estado in estados.AsEnumerable()
                            select new
                            {
                                idEstado = estado.Field<int>("idEstado"),
                                Descripcion = estado.Field<string>("Descripcion"),
                            };
                List<Estado> lsEstados = new List<Estado>();
                foreach (var item in query)
                {
                    lsEstados.Add(new Estado()
                    {
                        idEstado = item.idEstado,
                        Descripcion = item.Descripcion,
                    });
                }
                return lsEstados;
            }
            catch
            {
                return null;
            }
        }

        public List<Producto> ProductosPedidoGetData(long idPedido)
        {
            try
            {
                DataSet ds = BAL.ProductosPedidoGetData(idPedido);
                DataTable productos = ds.Tables[0];
                if (productos == null || productos.Rows.Count == 0) return null;
                List<Producto> lsProductos = new List<Producto>();
                productos.AsEnumerable().ToList().ForEach(
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
                        Importe = "",
                        VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        VF_LOGO = p.Field<string>("VF_LOGO")
                    }
                ));
                return lsProductos;
            }
            catch
            {
                return null;
            }
        }

        public List<Pedido> PedidosData(long? idPedido, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE, string AlmacenPreferido)
        {
            try
            {
                DataSet ds = BAL.PedidosData(idPedido, idEstado, fechaDesde, fechaHasta, VC_CLIENTE, AlmacenPreferido);

                DataTable pedidos = ds.Tables[0];
                if (pedidos == null || pedidos.Rows.Count == 0) return null;
                List<Pedido> lsPedidos = new List<Pedido>();
                pedidos.AsEnumerable().ToList().ForEach(
                    p => lsPedidos.Add(new Pedido()
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
                                    VC_HORA = p.Field<DateTime?>("VC_HORA"),
                                    VC_CODFORMAPAGO = p.Field<string>("VC_CODFORMAPAGO")
                                },
                                VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                                Observaciones = p.Field<string>("Observaciones")
                                
                            }
                ));
                return lsPedidos;
            }
            catch
            {
                return null;
            }
        }

        public string PedidosGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                        string dirEnvio, decimal? baseImp, decimal? descuento,
                        decimal? nfu, decimal? igic, string observaciones, List<Producto> lsProductos)
        {
            return BAL.PedidosGuardar(idUsuario, fechaEnvio, fechaEntrega, porAgencia, dirEnvio, baseImp, descuento, nfu, igic, observaciones, lsProductos);
        }

        public int ActualizarEstadoPedido(long idPedido, DateTime? fecha)
        {
            return BAL.ActualizarEstadoPedido(idPedido, fecha);
        }

        public int AnularPedido(long idPedido)
        {
            return BAL.AnularPedido(idPedido);
        }

        public List<string> EstadoActualYSiguiente(int idEstado)
        {
            try
            {
                DataSet ds = BAL.EstadoActualYSiguiente(idEstado); ;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;

                DataTable dt = ds.Tables[0];

                List<string> ls = new List<string>();
                dt.AsEnumerable().ToList().ForEach(p =>
                    {
                        ls.Add(p.Field<int>("idEstadoActual").ToString());
                        ls.Add(p.Field<string>("CodEstadoActual"));
                        ls.Add(p.Field<string>("DescEstadoActual"));
                        ls.Add(p.Field<int>("idEstadoSiguiente").ToString());
                        ls.Add(p.Field<string>("CodEstadoSiguiente"));
                        ls.Add(p.Field<string>("DescEstadoSiguiente"));
                    });
                return ls;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region RESERVAS

        public List<Estado> EstadosReservaCombo()
        {
            try
            {
                DataSet ds = BAL.EstadosReservaCombo();
                DataTable estados = ds.Tables[0];
                if (estados == null || estados.Rows.Count == 0) return null;
                var query = from estado in estados.AsEnumerable()
                            select new
                            {
                                idEstado = estado.Field<int>("idEstado"),
                                Descripcion = estado.Field<string>("Descripcion"),
                            };
                List<Estado> lsEstados = new List<Estado>();
                foreach (var item in query)
                {
                    lsEstados.Add(new Estado()
                    {
                        idEstado = item.idEstado,
                        Descripcion = item.Descripcion,
                    });
                }
                return lsEstados;
            }
            catch
            {
                return null;
            }
        }

        public List<Producto> ProductosReservaGetData(long idReserva)
        {
            try
            {
                DataSet ds = BAL.ProductosReservaGetData(idReserva);
                DataTable productos = ds.Tables[0];
                if (productos == null || productos.Rows.Count == 0) return null;
                List<Producto> lsProductos = new List<Producto>();
                productos.AsEnumerable().ToList().ForEach(
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
                        Importe = "",
                        VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        VF_LOGO = p.Field<string>("VF_LOGO")
                    }
                ));
                return lsProductos;
            }
            catch
            {
                return null;
            }
        }

        public List<Reserva> ReservasData(long? idReserva, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE)
        {
            try
            {
                DataSet ds = BAL.ReservasData(idReserva, idEstado, fechaDesde, fechaHasta, VC_CLIENTE);

                DataTable reservas = ds.Tables[0];
                if (reservas == null || reservas.Rows.Count == 0) return null;
                List<Reserva> lsReservas = new List<Reserva>();
                reservas.AsEnumerable().ToList().ForEach(
                    p => lsReservas.Add(new Reserva()
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
                            VC_HORA = p.Field<DateTime?>("VC_HORA"),
                            VC_CODFORMAPAGO = p.Field<string>("VC_CODFORMAPAGO")
                        },
                        VC_CLIENTE = p.Field<string>("VC_CLIENTE"),
                        Observaciones = p.Field<string>("Observaciones")
                    }
                ));
                return lsReservas;
            }
            catch
            {
                return null;
            }
        }

        public string ReservasGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                        string dirEnvio, decimal? baseImp, decimal? descuento,
                        decimal? nfu, decimal? igic, string observaciones, List<Producto> lsProductos)
        {
            return BAL.ReservasGuardar(idUsuario, fechaEnvio, fechaEntrega, porAgencia, dirEnvio, baseImp, descuento, nfu, igic, observaciones, lsProductos);
        }

        public int ActualizarEstadoReserva(long idReserva, DateTime? fecha)
        {
            return BAL.ActualizarEstadoReserva(idReserva, fecha);
        }

        public int AnularReserva(long idReserva)
        {
            return BAL.AnularReserva(idReserva);
        }

        public List<string> EstadoReservaActualYSiguiente(int idEstado)
        {
            try
            {
                DataSet ds = BAL.EstadoReservaActualYSiguiente(idEstado); ;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;

                DataTable dt = ds.Tables[0];

                List<string> ls = new List<string>();
                dt.AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(p.Field<int>("idEstadoActual").ToString());
                    ls.Add(p.Field<string>("CodEstadoActual"));
                    ls.Add(p.Field<string>("DescEstadoActual"));
                    ls.Add(p.Field<int>("idEstadoSiguiente").ToString());
                    ls.Add(p.Field<string>("CodEstadoSiguiente"));
                    ls.Add(p.Field<string>("DescEstadoSiguiente"));
                });
                return ls;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region PRODUCTOS

        public List<Producto> ProductosBuscadorStaff(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2)
        {
            try
            {
                DataSet ds = BAL.ProductosBuscadorStaff(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv,referencia2);
                DataTable productos = ds.Tables[0];
                List<Producto> lsProductos = new List<Producto>();
                productos.AsEnumerable().ToList().ForEach(
                    p => lsProductos.Add(new Producto()
                    {
                        //ID = p.Field<long>("ID"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        Stock = p.Field<object>("Stock").Parse<Int32>(),
                        //Stock24 = p.Field<object>("Stock24").Parse<Int32>(),
                        PteLlegada = p.Field<string>("PteLlegada"),
                        PrecioUnidad = 0,
                        VP_PVP1 = p.Field<decimal>("VP_PVP1"),
                        VP_PVP2 = p.Field<decimal>("VP_PVP2"),
                        VP_PVP3 = p.Field<decimal>("VP_PVP3"),
                        Cantidad = 0,
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA"),
                        VP_TIPO_OFER = p.Field<int>("VP_TIPO_OFER"),
                        VP_COLOR_OFER = p.Field<string>("VP_COLOR_OFER"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        Ecotasa = p.Field<decimal>("Ecotasa"),
                        VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP"),
                        VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP"),
                        VP_IMAGEN = p.Field<string>("VP_IMAGEN"),
                        Importe = "",
                        ECOTASA_DETALLES = p.Field<string>("ECOTASA_DETALLES"),
                        VP_IMPORTADO = p.Field<int?>("VP_IMPORTADO"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                        VP_NIVELRUIDO = p.Field<decimal>("VP_NIVELRUIDO"),
                        VP_EFICOMBUSTIBLE = p.Field<string>("VP_EFICOMBUSTIBLE"),
                        VP_ADHERENCIA = p.Field<string>("VP_ADHERENCIA"),
                        VP_VALORRUIDO = p.Field<decimal>("VP_VALORRUIDO"),
                        VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        VF_LOGO = p.Field<string>("VF_LOGO"),
                        StockProducto = new Stock() 
                        {
                            VS_STOCK_A01 = p.Field<decimal>("VS_STOCK_A01"),
                            VS_STOCK_A02 = p.Field<decimal>("VS_STOCK_A02"),
                            VS_STOCK_A03 = p.Field<decimal>("VS_STOCK_A03"),
                            VS_STOCK_A04 = p.Field<decimal>("VS_STOCK_A04"),
                            VS_STOCK_A12 = p.Field<decimal>("VS_STOCK_A12"),
                            VS_STOCK_A13 = p.Field<decimal>("VS_STOCK_A13"),
                            VS_STOCK_A18 = p.Field<decimal>("VS_STOCK_A18"),
                            VS_STOCK_A19 = p.Field<decimal>("VS_STOCK_A19"),
                            VS_STOCK_A22 = p.Field<decimal>("VS_STOCK_A22"),
                            VS_STOCK_A23 = p.Field<decimal>("VS_STOCK_A23"),
                            VS_STOCK_A24 = p.Field<decimal>("VS_STOCK_A24"),
                            VS_STOCK_A27 = p.Field<decimal>("VS_STOCK_A27"),
                            VS_STOCK_A29 = p.Field<decimal>("VS_STOCK_A29"),
                            VS_STOCK_A31 = p.Field<decimal>("VS_STOCK_A31"),
                            VS_STOCK_A32 = p.Field<decimal>("VS_STOCK_A32"),
                            VS_STOCK_A43 = p.Field<decimal>("VS_STOCK_A43"),
                            VS_STOCK_A44 = p.Field<decimal>("VS_STOCK_A44"),
                            VS_STOCK_A45 = p.Field<decimal>("VS_STOCK_A45"),
                            VS_STOCK_A46 = p.Field<decimal>("VS_STOCK_A46"),
                            VS_STOCK_A47 = p.Field<decimal>("VS_STOCK_A47"),
                            VS_STOCK_A54 = p.Field<decimal>("VS_STOCK_A54"),
                            VS_STOCK_A55 = p.Field<decimal>("VS_STOCK_A55"),
                            VS_STOCK_A56 = p.Field<decimal>("VS_STOCK_A56"),
                            VS_STOCK_GEN = p.Field<decimal>("VS_STOCK_GEN"),
                            VS_STOCK_A39 = p.Field<decimal>("VS_STOCK_A39"),
                            VS_STOCK_A50 = p.Field<decimal>("VS_STOCK_A50"),
                            VS_STOCK_A53 = p.Field<decimal>("VS_STOCK_A53"),
                            VS_STOCK_A60 = p.Field<decimal>("VS_STOCK_A60"),
                            VS_STOCK_A63 = p.Field<decimal>("VS_STOCK_A63")
                        }
                    }
                ));
                return lsProductos;
            }
            catch
            {
                return null;
            }
        }

        public List<Producto> ProductosBuscador(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2)
        {
            try
            {
                DataSet ds = BAL.ProductosBuscador(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2);
                DataTable productos = ds.Tables[0];
                if (productos == null || productos.Rows.Count == 0)
                {
                    // SI SE BUSCÓ SOLO POR REFERENCIA, Y NO HAY RESULTADOS, LA CONTABILIZO
                    bool filtroSoloReferencia = !string.IsNullOrEmpty(referencia);
                    filtroSoloReferencia &= string.IsNullOrEmpty(modelo);
                    filtroSoloReferencia &= tipoNeuma == null;
                    filtroSoloReferencia &= string.IsNullOrEmpty(ic);
                    filtroSoloReferencia &= string.IsNullOrEmpty(iv);

                    if (filtroSoloReferencia)
                    {
                        Usuario usuario = ObtenerDatosUsuario(idUsuario);
                        string vc_cliente = null;
                        if (usuario != null) vc_cliente = usuario.VC_CLIENTE;
                        BAL.BusquedasFallidasGuardar(referencia, vc_cliente);
                        return null;
                    }
                }
                List<Producto> lsProductos = new List<Producto>();
                productos.AsEnumerable().ToList().ForEach(
                    p => {
                        Producto nuevoProducto = new Producto();
                        //ID = p.Field<long>("ID"),
                        nuevoProducto.VP_DESCFAM = p.Field<string>("VP_DESCFAM");
                        nuevoProducto.VP_FAMILIA = p.Field<string>("VP_FAMILIA");
                        nuevoProducto.VP_MODELO = p.Field<string>("VP_MODELO");
                        nuevoProducto.VP_PRODUCTO = p.Field<string>("VP_PRODUCTO");
                        nuevoProducto.VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION");
                        nuevoProducto.VP_IC = p.Field<string>("VP_IC");
                        nuevoProducto.VP_IV = p.Field<string>("VP_IV");
                        nuevoProducto.Stock = p.Field<object>("Stock").Parse<Int32>();
                        nuevoProducto.Stock24 = p.Field<object>("Stock24").Parse<Int32>();
                        nuevoProducto.PteLlegada = p.Field<string>("PteLlegada");
                        //nuevoProducto.Contenedor = p.Field<string>("Contenedor");
                        nuevoProducto.PrecioUnidad = p.Field<decimal>("PrecioUnidad");
                        nuevoProducto.PVP = p.Field<decimal>("PrecioUnidad"); // ES EL PRECIO QUE EL CLIENTE MAYORISTA PUEDE CALCULAR SOBRE LA MARCHA PARA DARLE PRECIO AL CLIENTE MINORISTA
                        nuevoProducto.Cantidad = 0;
                        nuevoProducto.VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1");
                        nuevoProducto.VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA");
                        nuevoProducto.VP_TIPO_OFER = p.Field<int>("VP_TIPO_OFER");
                        nuevoProducto.VP_COLOR_OFER = p.Field<string>("VP_COLOR_OFER");
                        nuevoProducto.VP_CATEGORIA = p.Field<string>("VP_CATEGORIA");
                        nuevoProducto.Ecotasa = p.Field<decimal>("Ecotasa");
                        nuevoProducto.VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP");
                        nuevoProducto.VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP");
                        nuevoProducto.VP_IMAGEN = p.Field<string>("VP_IMAGEN");
                        nuevoProducto.Importe = "";
                        nuevoProducto.ECOTASA_DETALLES = p.Field<string>("ECOTASA_DETALLES");
                        nuevoProducto.VP_IMPORTADO = p.Field<int>("VP_IMPORTADO");
                        nuevoProducto.VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO");
                        nuevoProducto.VP_NIVELRUIDO = p.Field<decimal>("VP_NIVELRUIDO");
                        nuevoProducto.VP_EFICOMBUSTIBLE = p.Field<string>("VP_EFICOMBUSTIBLE");
                        nuevoProducto.VP_ADHERENCIA = p.Field<string>("VP_ADHERENCIA");
                        nuevoProducto.VP_VALORRUIDO = p.Field<decimal>("VP_VALORRUIDO");
                        nuevoProducto.VF_DESCFAM = p.Field<string>("VF_DESCFAM");
                        nuevoProducto.VF_LOGO = p.Field<string>("VF_LOGO");
                        lsProductos.Add(nuevoProducto);
                    }
                );
                return lsProductos;
            }
            catch
            {
                return null;
            }
        }

        public List<Producto> ProductosBuscadorV3(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2,
                                                  int pagina, int regPagina, ref int nFilasTotales, string ordenarPor, string ordenAscDesc)
        {
            try
            {
                DataSet ds = BAL.ProductosBuscadorV3(idUsuario, referencia, familia, modelo, tipoNeuma, ic, iv, referencia2, pagina, regPagina, ref nFilasTotales, ordenarPor, ordenAscDesc);
                DataTable productos = ds.Tables[0];
                if (productos == null || productos.Rows.Count == 0)
                {
                    // SI SE BUSCÓ SOLO POR REFERENCIA, Y NO HAY RESULTADOS, LA CONTABILIZO
                    bool filtroSoloReferencia = !string.IsNullOrEmpty(referencia);
                    filtroSoloReferencia &= string.IsNullOrEmpty(modelo);
                    filtroSoloReferencia &= tipoNeuma == null;
                    filtroSoloReferencia &= string.IsNullOrEmpty(ic);
                    filtroSoloReferencia &= string.IsNullOrEmpty(iv);

                    if (filtroSoloReferencia)
                    {
                        Usuario usuario = ObtenerDatosUsuario(idUsuario);
                        string vc_cliente = null;
                        if (usuario != null) vc_cliente = usuario.VC_CLIENTE;
                        BAL.BusquedasFallidasGuardar(referencia, vc_cliente);
                        return null;
                    }
                }
                List<Producto> lsProductos = new List<Producto>();
                productos.AsEnumerable().ToList().ForEach(
                    p =>
                    {
                        Producto nuevoProducto = new Producto();
                        nuevoProducto.ID = p.Field<long>("ID");
                        nuevoProducto.VP_DESCFAM = p.Field<string>("VP_DESCFAM");
                        nuevoProducto.VP_FAMILIA = p.Field<string>("VP_FAMILIA");
                        nuevoProducto.VP_MODELO = p.Field<string>("VP_MODELO");
                        nuevoProducto.VP_PRODUCTO = p.Field<string>("VP_PRODUCTO");
                        nuevoProducto.VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION");
                        nuevoProducto.VP_IC = p.Field<string>("VP_IC");
                        nuevoProducto.VP_IV = p.Field<string>("VP_IV");
                        nuevoProducto.Stock = p.Field<object>("Stock").Parse<Int32>();
                        nuevoProducto.Stock24 = p.Field<object>("Stock24").Parse<Int32>();
                        nuevoProducto.PteLlegada = p.Field<string>("PteLlegada");
                        //nuevoProducto.Contenedor = p.Field<string>("Contenedor");
                        nuevoProducto.PrecioUnidad = p.Field<decimal>("PrecioUnidad");
                        nuevoProducto.PVP = p.Field<decimal>("PrecioUnidad"); // ES EL PRECIO QUE EL CLIENTE MAYORISTA PUEDE CALCULAR SOBRE LA MARCHA PARA DARLE PRECIO AL CLIENTE MINORISTA
                        nuevoProducto.Cantidad = 0;
                        nuevoProducto.VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1");
                        nuevoProducto.VP_TIPO_NEUMA = p.Field<decimal>("VP_TIPO_NEUMA");
                        nuevoProducto.VP_TIPO_OFER = p.Field<int>("VP_TIPO_OFER");
                        nuevoProducto.VP_COLOR_OFER = p.Field<string>("VP_COLOR_OFER");
                        nuevoProducto.VP_CATEGORIA = p.Field<string>("VP_CATEGORIA");
                        nuevoProducto.Ecotasa = p.Field<decimal>("Ecotasa");
                        nuevoProducto.VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP");
                        nuevoProducto.VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP");
                        nuevoProducto.VP_IMAGEN = p.Field<string>("VP_IMAGEN");
                        nuevoProducto.Importe = "";
                        nuevoProducto.ECOTASA_DETALLES = p.Field<string>("ECOTASA_DETALLES");
                        nuevoProducto.VP_IMPORTADO = p.Field<int>("VP_IMPORTADO");
                        nuevoProducto.VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO");
                        nuevoProducto.VP_NIVELRUIDO = p.Field<decimal>("VP_NIVELRUIDO");
                        nuevoProducto.VP_EFICOMBUSTIBLE = p.Field<string>("VP_EFICOMBUSTIBLE");
                        nuevoProducto.VP_ADHERENCIA = p.Field<string>("VP_ADHERENCIA");
                        nuevoProducto.VP_VALORRUIDO = p.Field<decimal>("VP_VALORRUIDO");
                        nuevoProducto.VF_DESCFAM = p.Field<string>("VF_DESCFAM");
                        nuevoProducto.VF_LOGO = p.Field<string>("VF_LOGO");
                        lsProductos.Add(nuevoProducto);
                    }
                );
                if (ds.Tables.Count > 1)                 
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out nFilasTotales);
                
                return lsProductos;
            }
            catch
            {
                return null;
            }
        }

        public List<Producto> ProductosGuardar(string familia, string descfam, string producto, string descripcion
                              , string producto1, string modelo, decimal? serie, decimal? llanta, string medida
                              , string ic, string iv, decimal? tipo_neuma, string desc_tipo
                              , decimal pvp1, decimal pvp2, decimal pvp3, int tipo_ofer, string categoria
                              , decimal VP_PORC_IMP, string VP_IMAGEN, int? VP_IMPORTADO
                              , decimal VP_NIVELRUIDO, string VP_EFICOMBUSTIBLE, string VP_ADHERENCIA, decimal VP_VALORRUIDO)
        {
            try
            {
                DataSet ds = BAL.ProductosGuardar(familia, descfam, producto, descripcion, producto1, modelo, serie, llanta, medida, ic, iv, tipo_neuma, desc_tipo, pvp1, pvp2, pvp3, tipo_ofer, categoria, VP_PORC_IMP, VP_IMAGEN, VP_IMPORTADO, VP_NIVELRUIDO, VP_EFICOMBUSTIBLE, VP_ADHERENCIA, VP_VALORRUIDO);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Producto> ls = new List<Producto>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Producto()
                    {
                        VP_COLOR_OFER = p.Field<string>("VP_COLOR_OFER"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_TIPO_NEUMA = p.Field<decimal?>("VP_TIPO_NEUMA"),
                        VP_TIPO_OFER = p.Field<int>("VP_TIPO_OFER"),
                        MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_PVP1 = p.Field<decimal>("VP_PVP1"),
                        VP_PVP2 = p.Field<decimal>("VP_PVP2"),
                        VP_PVP3 = p.Field<decimal>("VP_PVP3"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP"),
                        VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP"),
                        VP_IMPORTADO = p.Field<int?>("VP_IMPORTADO"),
                        VP_NIVELRUIDO = p.Field<decimal>("VP_NIVELRUIDO"),
                        VP_EFICOMBUSTIBLE = p.Field<string>("VP_EFICOMBUSTIBLE"),
                        VP_ADHERENCIA = p.Field<string>("VP_ADHERENCIA"),
                        VP_VALORRUIDO = p.Field<decimal>("VP_VALORRUIDO")/*,
                        VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        VF_LOGO = p.Field<string>("VF_LOGO")*/
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public List<Producto> ProductosData(string VP_FAMILIA, string VP_PRODUCTO, DateTime? MODIFICADO)
        {
            try
            {
                DataSet ds = BAL.ProductosData(VP_FAMILIA, VP_PRODUCTO, MODIFICADO);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Producto> ls = new List<Producto>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                    {
                        ls.Add(new Producto()
                        {
                            VP_COLOR_OFER = p.Field<string>("VP_COLOR_OFER"),
                            VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                            VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                            VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                            VP_IC = p.Field<string>("VP_IC"),
                            VP_IV = p.Field<string>("VP_IV"),
                            VP_MODELO = p.Field<string>("VP_MODELO"),
                            VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                            VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                            VP_TIPO_NEUMA = p.Field<decimal?>("VP_TIPO_NEUMA"),
                            VP_TIPO_OFER = p.Field<int>("VP_TIPO_OFER"),
                            MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                            VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                            VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                            VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                            VP_PVP1 = p.Field<decimal>("VP_PVP1"),
                            VP_PVP2 = p.Field<decimal>("VP_PVP2"),
                            VP_PVP3 = p.Field<decimal>("VP_PVP3"),
                            VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                            VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                            VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP"),
                            VP_IMAGEN = p.Field<string>("VP_IMAGEN"),
                            Importe = "",
                            VP_IMPORTADO = p.Field<int?>("VP_IMPORTADO"),
                            VP_NIVELRUIDO = p.Field<decimal>("VP_NIVELRUIDO"),
                            VP_EFICOMBUSTIBLE = p.Field<string>("VP_EFICOMBUSTIBLE"),
                            VP_ADHERENCIA = p.Field<string>("VP_ADHERENCIA"),
                            VP_VALORRUIDO = p.Field<decimal>("VP_VALORRUIDO"),
                            VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                            VF_LOGO = p.Field<string>("VF_LOGO")
                        });
                    });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public List<Producto> ProductosDataV3(string VP_PRODUCTO)
        {
            try
            {
                DataSet ds = BAL.ProductosDataV3(VP_PRODUCTO);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Producto> ls = new List<Producto>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Producto()
                    {
                        ID = p.Field<long>("ID"),
                        VP_COLOR_OFER = p.Field<string>("VP_COLOR_OFER"),
                        VP_DESCFAM = p.Field<string>("VP_DESCFAM"),
                        VP_DESCRIPCION = p.Field<string>("VP_DESCRIPCION"),
                        VP_FAMILIA = p.Field<string>("VP_FAMILIA"),
                        VP_IC = p.Field<string>("VP_IC"),
                        VP_IV = p.Field<string>("VP_IV"),
                        VP_MODELO = p.Field<string>("VP_MODELO"),
                        VP_PRODUCTO = p.Field<string>("VP_PRODUCTO"),
                        VP_PRODUCTO1 = p.Field<string>("VP_PRODUCTO1"),
                        VP_TIPO_NEUMA = p.Field<decimal?>("VP_TIPO_NEUMA"),
                        VP_TIPO_OFER = p.Field<int>("VP_TIPO_OFER"),
                        MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                        VP_DESC_TIPO = p.Field<string>("VP_DESC_TIPO"),
                        VP_LLANTA = p.Field<decimal?>("VP_LLANTA"),
                        VP_MEDIDA = p.Field<string>("VP_MEDIDA"),
                        VP_PVP1 = p.Field<decimal>("VP_PVP1"),
                        VP_PVP2 = p.Field<decimal>("VP_PVP2"),
                        VP_PVP3 = p.Field<decimal>("VP_PVP3"),
                        VP_SERIE = p.Field<decimal?>("VP_SERIE"),
                        VP_CATEGORIA = p.Field<string>("VP_CATEGORIA"),
                        VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP"),
                        VP_IMAGEN = p.Field<string>("VP_IMAGEN"),
                        Importe = "",
                        VP_IMPORTADO = p.Field<int?>("VP_IMPORTADO"),
                        VP_NIVELRUIDO = p.Field<decimal>("VP_NIVELRUIDO"),
                        VP_EFICOMBUSTIBLE = p.Field<string>("VP_EFICOMBUSTIBLE"),
                        VP_ADHERENCIA = p.Field<string>("VP_ADHERENCIA"),
                        VP_VALORRUIDO = p.Field<decimal>("VP_VALORRUIDO"),
                        VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        VF_LOGO = p.Field<string>("VF_LOGO")
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region PRODUCTOS PEDIDO

        public Pedido DetallesPedido(long idPedido)
        {
            try
            {
                DataSet ds = BAL.DetallesPedido(idPedido);
                if (ds == null || ds.Tables.Count < 2) return null; //2 porque devuelvo 2 tables

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
                            VC_HORA = p.Field<DateTime?>("VC_HORA"),
                            VC_CODFORMAPAGO = p.Field<string>("VC_CODFORMAPAGO")
                        },
                        Observaciones = p.Field<string>("Observaciones")
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
                        ECOTASA_DETALLES = p.Field<string>("ECOTASA_DETALLES"),
                        VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP"),
                        VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP"),
                        VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        VF_LOGO = p.Field<string>("VF_LOGO")
                    }
                    ));
                return pedido;
            }
            catch
            {
                return null;
            }
        }

        public Reserva DetallesReserva(long idReserva)
        {
            try
            {
                DataSet ds = BAL.DetallesReserva(idReserva);
                if (ds == null || ds.Tables.Count < 2) return null; //2 porque devuelvo 2 tables

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
                            VC_HORA = p.Field<DateTime?>("VC_HORA"),
                            VC_CODFORMAPAGO = p.Field<string>("VC_CODFORMAPAGO")
                        },
                        Observaciones = p.Field<string>("Observaciones")
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
                        ECOTASA_DETALLES = p.Field<string>("ECOTASA_DETALLES"),
                        VP_PORC_IMP = p.Field<decimal>("VP_PORC_IMP"),
                        VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP"),
                        //VF_DESCFAM = p.Field<string>("VF_DESCFAM"),
                        //VF_LOGO = p.Field<string>("VF_LOGO"),
                        EsReserva = true
                    }
                    ));
                return reserva;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region PENDIENTES

        public List<Pendiente> PendientesPorProductoData(string producto)
        {
            try
            {
                DataSet ds = BAL.PendientesPorProductoData(producto);
                DataTable pendientes = ds.Tables[0];
                if (pendientes == null || pendientes.Rows.Count == 0) return null;
                List<Pendiente> lsPendientes = new List<Pendiente>();
                pendientes.AsEnumerable().ToList().ForEach(
                    p => lsPendientes.Add(new Pendiente()
                    {
                        VL_ARTICULO = p.Field<string>("VL_ARTICULO"),
                        VL_CONTENEDOR = p.Field<string>("VL_CONTENEDOR"),
                        VL_LLEGADA = p.Field<DateTime>("VL_LLEGADA")
                    }
                ));
                return lsPendientes;
            }
            catch
            {
                return null;
            }
        }

        public List<Pendiente> PendientesGetData(DateTime? llegada)
        {
            try
            {
                DataSet ds = BAL.PendientesGetData(llegada);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Pendiente> ls = new List<Pendiente>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Pendiente()
                    {
                        VL_ARTICULO = p.Field<string>("VL_ARTICULO"),
                        VL_CONTENEDOR = p.Field<string>("VL_CONTENEDOR"),
                        VL_LLEGADA = p.Field<DateTime>("VL_LLEGADA"),
                        MODIFICADO = p.Field<DateTime>("MODIFICADO")
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public List<Pendiente> PendientesGuardar(string articulo, DateTime llegada, string contenedor)
        {
            try
            {
                DataSet ds = BAL.PendientesGuardar(articulo, llegada, contenedor);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Pendiente> ls = new List<Pendiente>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Pendiente()
                    {
                        VL_ARTICULO = p.Field<string>("VL_ARTICULO"),
                        VL_CONTENEDOR = p.Field<string>("VL_CONTENEDOR"),
                        VL_LLEGADA = p.Field<DateTime>("VL_LLEGADA"),
                        MODIFICADO = p.Field<DateTime>("MODIFICADO")
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public int PendientesBorrarSiExiste(string articulo)
        {
            return BAL.PendientesBorrarSiExiste(articulo);
        }

        #endregion

        #region EMAIL

        public bool Send(List<string> addressesCc, List<string> addressesBcc, string subject, string messageBody, string att)
        {
            return BAL.Send(null, addressesCc, addressesBcc, subject, messageBody, att);
        }

        public bool Send(string addressesFrom, string subject, string messageBody)
        {
            return BAL.Send(addressesFrom, subject, messageBody);
        }

        public bool Send(string addressesFrom, string addressTo, string subject, string messageBody)
        {
            return BAL.Send(addressesFrom, addressTo, subject, messageBody);
        }

        #endregion

        #region COMBOS

        public List<Combo> FamiliasLogoCombo()
        {
            try
            {
                DataSet ds = BAL.FamiliasLogoCombo();
                DataTable familias = ds.Tables[0];
                if (familias == null || familias.Rows.Count == 0) return null;
                List<Combo> resultado = new List<Combo>();
                familias.AsEnumerable().ToList().ForEach(p => resultado.Add(
                    new Combo()
                    {
                        ValueMember = p.Field<string>("VP_FAMILIA"),
                        DisplayMember = p.Field<string>("VP_DESCFAM")
                    }));
                return resultado;
            }
            catch
            {
                return null;
            }
        }

        public List<Combo> FamiliasCombo()
        {
            try
            {
                DataSet ds = BAL.FamiliasCombo();
                DataTable familias = ds.Tables[0];
                if (familias == null || familias.Rows.Count == 0) return null;
                List<Combo> resultado = new List<Combo>();
                familias.AsEnumerable().ToList().ForEach(p => resultado.Add(
                    new Combo()
                {
                    ValueMember = p.Field<string>("VP_FAMILIA"),
                    DisplayMember = p.Field<string>("VP_DESCFAM")
                }));
                return resultado;
            }
            catch
            {
                return null;
            }
        }

        public List<Combo> ModelosCombo(string familia)
        {
            try
            {
                DataSet ds = BAL.ModelosCombo(familia);
                DataTable modelos = ds.Tables[0];
                if (modelos == null || modelos.Rows.Count == 0) return null;
                List<Combo> resultado = new List<Combo>();
                modelos.AsEnumerable().ToList().ForEach(p => resultado.Add(
                    new Combo()
                    {
                        ValueMember = p.Field<string>("VP_FAMILIA"),
                        DisplayMember = p.Field<string>("VP_MODELO")
                    }));
                return resultado;
            }
            catch
            {
                return null;
            }
        }

        public List<Combo> TipoNeumaticosCombo()
        {
            try
            {
                DataSet ds = BAL.TipoNeumaticosCombo();
                DataTable tipos = ds.Tables[0];
                if (tipos == null || tipos.Rows.Count == 0) return null;
                List<Combo> resultado = new List<Combo>();
                tipos.AsEnumerable().ToList().ForEach(p => resultado.Add(
                    new Combo()
                    {
                        ValueMember = p.Field<decimal>("VP_TIPO_NEUMA").Parse<string>(),
                        DisplayMember = p.Field<string>("VP_DESC_TIPO")
                    }));
                return resultado;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region ECOTASA

        public List<Ecotasa> EcotasasData()
        {
            try
            {
                DataSet ds = BAL.EcotasasData();
                DataTable ecotasa = ds.Tables[0];
                if (ecotasa == null || ecotasa.Rows.Count == 0) return null;
                List<Ecotasa> lsEcotasa = new List<Ecotasa>();
                ecotasa.AsEnumerable().ToList().ForEach(p => lsEcotasa.Add(new Ecotasa()
                {
                    VT_CATEGORIA = p.Field<string>("VT_CATEGORIA"),
                    VT_DESCRIPCION = p.Field<string>("VT_DESCRIPCION"),
                    VT_DETALLES = p.Field<string>("VT_DETALLES"),
                    //VT_PRODUCTO = p.Field<string>("VT_PRODUCTO"),
                    VT_PVP1 = p.Field<decimal>("VT_PVP1"),
                    MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                    VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP")
                }));
                return lsEcotasa;
            }
            catch
            {
                return null;
            }
        }

        public List<Ecotasa> EcotasaGuardar(string categoria, string descripcion, string detalles, decimal pvp1, decimal VT_PORC_IMP)
        {
            try
            {
                DataSet ds = BAL.EcotasaGuardar(categoria, descripcion, detalles, pvp1, VT_PORC_IMP);
                DataTable ecotasa = ds.Tables[0];
                if (ecotasa == null || ecotasa.Rows.Count == 0) return null;
                List<Ecotasa> lsEcotasa = new List<Ecotasa>();
                ecotasa.AsEnumerable().ToList().ForEach(p => lsEcotasa.Add(new Ecotasa()
                {
                    VT_CATEGORIA = p.Field<string>("VT_CATEGORIA"),
                    VT_DESCRIPCION = p.Field<string>("VT_DESCRIPCION"),
                    VT_DETALLES = p.Field<string>("VT_DETALLES"),
                    //VT_PRODUCTO = p.Field<string>("VT_PRODUCTO"),
                    VT_PVP1 = p.Field<decimal>("VT_PVP1"),
                    MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                    VT_PORC_IMP = p.Field<decimal>("VT_PORC_IMP")
                }));
                return lsEcotasa;
            }
            catch
            {
                return null;
            }
        }

        public int EcotasaBorrarSiExiste(string categoria)
        {
            return BAL.EcotasaBorrarSiExiste(categoria);
        }

        #endregion

        #region STOCK

        public List<Stock> StockData()
        {
            try
            {
                DataSet ds = BAL.StockData();
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Stock> ls = new List<Stock>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Stock()
                    {
                        ID = p.Field<int>("ID"),
                        MODIFICADO = p.Field<DateTime>("MODIFICADO"),
                        VS_ARTICULO = p.Field<string>("VS_ARTICULO"),
                        VS_CATEGORIA = p.Field<string>("VS_CATEGORIA"),
                        VS_ARTICULO1 = p.Field<string>("VS_ARTICULO1"),
                        VS_DESCARTICULO = p.Field<string>("VS_DESCARTICULO"),
                        VS_DESCFAMILIA = p.Field<string>("VS_DESCFAMILIA"),
                        VS_INDCARGA = p.Field<string>("VS_INDCARGA"),
                        VS_INDVELOCIDAD = p.Field<string>("VS_INDVELOCIDAD"),
                        VS_MODELO = p.Field<string>("VS_MODELO"),
                        VS_FAMILIA = p.Field<string>("VS_FAMILIA"),
                        VS_STOCK_A01 = p.Field<decimal>("VS_STOCK_A01"),
                        VS_STOCK_A02 = p.Field<decimal>("VS_STOCK_A02"),
                        VS_STOCK_A03 = p.Field<decimal>("VS_STOCK_A03"),
                        VS_STOCK_A04 = p.Field<decimal>("VS_STOCK_A04"),
                        VS_STOCK_A18 = p.Field<decimal>("VS_STOCK_A18"),
                        VS_STOCK_A19 = p.Field<decimal>("VS_STOCK_A19"),
                        VS_STOCK_A22 = p.Field<decimal>("VS_STOCK_A22"),
                        VS_STOCK_A23 = p.Field<decimal>("VS_STOCK_A23"),
                        VS_STOCK_A32 = p.Field<decimal>("VS_STOCK_A32"),
                        VS_STOCK_A44 = p.Field<decimal>("VS_STOCK_A44"),
                        VS_STOCK_A54 = p.Field<decimal>("VS_STOCK_A54"),
                        VS_STOCK_GEN = p.Field<decimal>("VS_STOCK_GEN"),
                        VS_STOCK_A12 = p.Field<decimal>("VS_STOCK_A12"),
                        VS_STOCK_A13 = p.Field<decimal>("VS_STOCK_A13"),
                        VS_STOCK_A55 = p.Field<decimal>("VS_STOCK_A55"),
                        VS_STOCK_A24 = p.Field<decimal>("VS_STOCK_A24"),
                        VS_STOCK_A27 = p.Field<decimal>("VS_STOCK_A27"),
                        VS_STOCK_A29 = p.Field<decimal>("VS_STOCK_A29"),
                        VS_STOCK_A31 = p.Field<decimal>("VS_STOCK_A31"),
                        VS_STOCK_A43 = p.Field<decimal>("VS_STOCK_A43"),
                        VS_STOCK_A45 = p.Field<decimal>("VS_STOCK_A45"),
                        VS_STOCK_A46 = p.Field<decimal>("VS_STOCK_A46"),
                        VS_STOCK_A47 = p.Field<decimal>("VS_STOCK_A47"),
                        VS_STOCK_A56 = p.Field<decimal>("VS_STOCK_A56"),
                        VS_STOCK_A39 = p.Field<decimal>("VS_STOCK_A39"),
                        VS_STOCK_A50 = p.Field<decimal>("VS_STOCK_A50"),
                        VS_STOCK_A53 = p.Field<decimal>("VS_STOCK_A53"),
                        VS_STOCK_A60 = p.Field<decimal>("VS_STOCK_A60"),
                        VS_STOCK_A63 = p.Field<decimal>("VS_STOCK_A63")
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        //public List<Stock> StockGuardar(string familia, string descfamilia, string articulo, string descarticulo, string articulo1
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
        //    DataSet ds = BAL.StockGuardar(familia, descfamilia, articulo, descarticulo, articulo1, modelo, indvelocidad, indcarga, categoria, stock_a01, stock_a02, stock_a03, stock_a04, stock_a18, stock_a19, stock_a22, stock_a23, stock_a32, stock_a44, stock_a54, stock_gen, stock_a12, stock_a13, stock_a55, stock_a24, stock_a27, stock_a29, stock_a31, stock_a43, stock_a45, stock_a46, stock_a47, stock_a56);
        //    if (ds == null || ds.Tables.Count == 0) return null;
        //    List<Stock> ls = new List<Stock>();
        //    ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
        //    {
        //        ls.Add(new Stock()
        //        {
        //            ID = p.Field<int>("ID"),
        //            MODIFICADO = p.Field<DateTime>("MODIFICADO"),
        //            VS_ARTICULO = p.Field<string>("VS_ARTICULO"),
        //            VS_CATEGORIA = p.Field<string>("VS_CATEGORIA"),
        //            VS_ARTICULO1 = p.Field<string>("VS_ARTICULO1"),
        //            VS_DESCARTICULO = p.Field<string>("VS_DESCARTICULO"),
        //            VS_DESCFAMILIA = p.Field<string>("VS_DESCFAMILIA"),
        //            VS_INDCARGA = p.Field<string>("VS_INDCARGA"),
        //            VS_INDVELOCIDAD = p.Field<string>("VS_INDVELOCIDAD"),
        //            VS_MODELO = p.Field<string>("VS_MODELO"),
        //            VS_FAMILIA = p.Field<string>("VS_FAMILIA"),
        //            VS_STOCK_A01 = p.Field<decimal>("VS_STOCK_A01"),
        //            VS_STOCK_A02 = p.Field<decimal>("VS_STOCK_A02"),
        //            VS_STOCK_A03 = p.Field<decimal>("VS_STOCK_A03"),
        //            VS_STOCK_A04 = p.Field<decimal>("VS_STOCK_A04"),
        //            VS_STOCK_A18 = p.Field<decimal>("VS_STOCK_A18"),
        //            VS_STOCK_A19 = p.Field<decimal>("VS_STOCK_A19"),
        //            VS_STOCK_A22 = p.Field<decimal>("VS_STOCK_A22"),
        //            VS_STOCK_A23 = p.Field<decimal>("VS_STOCK_A23"),
        //            VS_STOCK_A32 = p.Field<decimal>("VS_STOCK_A32"),
        //            VS_STOCK_A44 = p.Field<decimal>("VS_STOCK_A44"),
        //            VS_STOCK_A54 = p.Field<decimal>("VS_STOCK_A54"),
        //            VS_STOCK_GEN = p.Field<decimal>("VS_STOCK_GEN"),
        //            VS_STOCK_A12 = p.Field<decimal>("VS_STOCK_A12"),
        //            VS_STOCK_A13 = p.Field<decimal>("VS_STOCK_A13"),
        //            VS_STOCK_A55 = p.Field<decimal>("VS_STOCK_A55"),
        //            VS_STOCK_A24 = p.Field<decimal>("VS_STOCK_A24"),
        //            VS_STOCK_A27 = p.Field<decimal>("VS_STOCK_A27"),
        //            VS_STOCK_A29 = p.Field<decimal>("VS_STOCK_A29"),
        //            VS_STOCK_A31 = p.Field<decimal>("VS_STOCK_A31"),
        //            VS_STOCK_A43 = p.Field<decimal>("VS_STOCK_A43"),
        //            VS_STOCK_A45 = p.Field<decimal>("VS_STOCK_A45"),
        //            VS_STOCK_A46 = p.Field<decimal>("VS_STOCK_A46"),
        //            VS_STOCK_A47 = p.Field<decimal>("VS_STOCK_A47"),
        //            VS_STOCK_A56 = p.Field<decimal>("VS_STOCK_A56"),
        //            VS_STOCK_A39 = p.Field<decimal>("VS_STOCK_A39"),
        //            VS_STOCK_A50 = p.Field<decimal>("VS_STOCK_A50"),
        //            VS_STOCK_A53 = p.Field<decimal>("VS_STOCK_A53"),
        //            VS_STOCK_A60 = p.Field<decimal>("VS_STOCK_A60"),
        //            VS_STOCK_A63 = p.Field<decimal>("VS_STOCK_A63")
        //        });
        //    });
        //    return ls;
        //}

        #endregion

        #region PROMOCIONES

        public List<Promocion> PromocionesData(Promocion promocion)
        {
            try
            {
                DataSet ds = BAL.PromocionesData(promocion);
                if (ds == null || ds.Tables.Count == 0) return null;
                List<Promocion> ls = new List<Promocion>();
                ds.Tables[0].AsEnumerable().ToList().ForEach(p =>
                {
                    ls.Add(new Promocion()
                    {
                        Activa = p.Field<bool?>("Activa"),
                        BannerGra = p.Field<string>("BannerGra"),
                        BannerPeq = p.Field<string>("BannerPeq"),
                        Descripcion = p.Field<string>("Descripcion"),
                        FechaCreacion = p.Field<DateTime?>("FechaCreacion"),
                        FechaFin = p.Field<DateTime?>("FechaFin"),
                        FechaInicio = p.Field<DateTime?>("FechaInicio"),
                        idPromo = p.Field<int?>("idPromo"),
                        Titulo = p.Field<string>("Titulo"),
                        Url = p.Field<string>("URL"),
                        TipoBusqueda = GetTipoBusquedaFromXML(p.Field<string>("TipoBusqueda"))
                    });
                });
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public string PromocionesGuardar(Promocion promocion)
        {
            string error = string.Empty;
            try
            {
                string bannerPeq = "";
                string bannerGra = "";
                if (promocion.idPromo != -1) // está editando entonces tengo que capturar los ficheros de banner para poder eliminarlos
                {
                    Promocion promoAeditar = PromocionesData(new Promocion() { idPromo = promocion.idPromo })[0];
                    bannerPeq = promoAeditar.BannerPeq;
                    bannerGra = promoAeditar.BannerGra;
                }
                DataSet ds = BAL.PromocionesGuardar(promocion);
                if (ds == null || ds.Tables.Count == 0) return "Error al guardar datos.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();

                // si no hay error, entonces miro a ver si tengo que eliminar los ficheros de banners
                if (promocion.idPromo != -1) // está editando entonces tengo que capturar los ficheros de banner para poder eliminarlos
                {
                    if (!string.IsNullOrEmpty(bannerPeq) && bannerPeq != promocion.BannerPeq) EliminarBanner(bannerPeq);
                    if (!string.IsNullOrEmpty(bannerGra) && bannerGra != promocion.BannerGra) EliminarBanner(bannerGra);
                }

                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al guardar los datos.";
            }
        }

        private void EliminarBanner(string fileName)
        {
            try
            {
                string BannerPromoDir = ConfigurationManager.AppSettings["BannerPromoDir"];

                string bannerPath = string.Format(@"{0}{1}\{2}", AppDomain.CurrentDomain.BaseDirectory
                                                          , ConfigurationManager.AppSettings["BannerPromoDir"]
                                                          , fileName);

                if (File.Exists(bannerPath)) File.Delete(bannerPath);

            }
            catch
            {

            }
        }

        public string PromocionesEliminar(int idPromo)
        {
            string error = string.Empty;
            try
            {
                Promocion promoAEliminar = PromocionesData(new Promocion() { idPromo = idPromo })[0];

                DataSet ds = BAL.PromocionesEliminar(idPromo);
                
                if (ds == null || ds.Tables.Count == 0) return "Error al eliminar.";
                if (ds.Tables[0].Rows[0]["ErrorCode"].ToString() != "0000") return ds.Tables[0].Rows[0]["ErrorText"].ToString();

                // si no hay error, entonces trato de eliminar los ficheros de banners
                if (!string.IsNullOrEmpty(promoAEliminar.BannerPeq)) EliminarBanner(promoAEliminar.BannerPeq);
                if (!string.IsNullOrEmpty(promoAEliminar.BannerGra)) EliminarBanner(promoAEliminar.BannerGra);

                return string.Empty;
            }
            catch
            {
                return "Se produjo un error al eliminar los datos.";
            }
        }

        #endregion
    }
}
