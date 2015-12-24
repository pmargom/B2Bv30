using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using B2B.Types;
using System.Xml.Serialization;

namespace DataAccess
{
    public class DataAccess
    {
        private SQLServerBD mSqlDataBase;

        public SQLServerBD SqlDataBase
        {
            get
            {
                if (this.mSqlDataBase == null)
                {
                    this.mSqlDataBase = new SQLServerBD(ConfigurationManager.ConnectionStrings["sqlBD"].ConnectionString);
                }
                return this.mSqlDataBase;
            }
        }

        public DataAccess()
        {
        }


        #region SQL Server Sync

        public DataSet SqlCleanTables()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_CleanTables";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet SqlGuardar(string DBTable, string parametros)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_Guardar";
            comando.Parameters.AddWithValue("DBTable", DBTable);
            comando.Parameters.AddWithValue("parametros", parametros);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet Sql_Sincronizar(DateTime fechaNuevaSync, int tiempo)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_Sync";
            comando.Parameters.AddWithValue("fechaNuevaSync", fechaNuevaSync);
            comando.Parameters.AddWithValue("tiempo", tiempo);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet Sql_SyncUpdate(DateTime fechaSync)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_SyncUpdate";
            comando.Parameters.AddWithValue("fechaSync", fechaSync);
            return SqlDataBase.fillDataSet(comando);
        }

        public void SyncLastUpdate(bool SyncNocturna)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("SyncNocturna", SyncNocturna);
            comando.CommandText = "SP_SyncLastUpdate";
            SqlDataBase.executeUpdate(comando);
        }

        public DataSet SyncLastDate(bool SyncNocturna)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("SyncNocturna", SyncNocturna);
            comando.CommandText = "SP_SyncLastDate";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region MENSAJES

        public int MensajeMarcarComoLeido(int idMensaje)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_MensajeMarcarComoLeido";
            comando.Parameters.AddWithValue("idMensaje", idMensaje);
            return SqlDataBase.executeUpdate(comando);
        }

        public int MensajeGuardar(Mensaje mensaje)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_MensajeGuardar";
            comando.Parameters.AddWithValue("idBuzon", mensaje.Buzon.idBuzon);
            comando.Parameters.AddWithValue("RemitenteIdUsuario", mensaje.RemitenteIdUsuario);
            comando.Parameters.AddWithValue("DestinatarioIdUsuario", mensaje.DestinatarioIdUsuario);
            comando.Parameters.AddWithValue("Asunto", mensaje.Asunto);
            comando.Parameters.AddWithValue("Contenido", mensaje.Contenido);
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet MensajesData(Mensaje mensaje)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_MensajesData";
            if (mensaje.RemitenteIdUsuario != 0) comando.Parameters.AddWithValue("RemitenteIdUsuario", mensaje.RemitenteIdUsuario);
            if (mensaje.DestinatarioIdUsuario != 0) comando.Parameters.AddWithValue("DestinatarioIdUsuario", mensaje.DestinatarioIdUsuario);
            if (mensaje.Leido != null) comando.Parameters.AddWithValue("Leido", mensaje.Leido);
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region FAMILIAS LOGOS

        public DataSet FamiliasLogoData(Familia familia)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FamiliasLogoData";
            if (!string.IsNullOrEmpty(familia.VF_FAMILIA)) comando.Parameters.AddWithValue("VF_FAMILIA", familia.VF_FAMILIA);
            if (!string.IsNullOrEmpty(familia.VF_DESCFAM)) comando.Parameters.AddWithValue("VF_DESCFAM", familia.VF_DESCFAM);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet FamiliasLogoGuardar(Familia familia)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FamiliasLogoGuardar";
            comando.Parameters.AddWithValue("VF_FAMILIA", familia.VF_FAMILIA);
            comando.Parameters.AddWithValue("VF_DESCFAM", familia.VF_DESCFAM);
            comando.Parameters.AddWithValue("VF_LOGO", familia.VF_LOGO);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet FamiliasLogoEliminar(string VF_FAMILIA)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FamiliasLogoEliminar";
            comando.Parameters.AddWithValue("VF_FAMILIA", VF_FAMILIA);
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region PEDIDOS POR CLIENTE

        public DataSet PedidosXClientesData(DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (FechaDesde != null) comando.Parameters.AddWithValue("FechaDesde", FechaDesde);
            if (FechaHasta != null) comando.Parameters.AddWithValue("FechaHasta", FechaHasta);
            if (!string.IsNullOrEmpty(VC_CLIENTE)) comando.Parameters.AddWithValue("VC_CLIENTE_DESDE", VC_CLIENTE);
            comando.CommandText = "SP_PedidosXClientesData";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region CLIENTES VS PEDIDOS

        public DataSet ClientesVsPedidosData(DateTime? FechaDesde, DateTime? FechaHasta, string clienteDesde, string clienteHasta)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (FechaDesde != null) comando.Parameters.AddWithValue("FechaDesde", FechaDesde);
            if (FechaHasta != null) comando.Parameters.AddWithValue("FechaHasta", FechaHasta);
            if (!string.IsNullOrEmpty(clienteDesde)) comando.Parameters.AddWithValue("clienteDesde", clienteDesde);
            if (!string.IsNullOrEmpty(clienteHasta)) comando.Parameters.AddWithValue("clienteHasta", clienteHasta);
            comando.CommandText = "SP_ClientesVsPedidosData";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region SESIONES

        public DataSet SesionesData(Sesion sesion)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (sesion.FechaInicio != null) comando.Parameters.AddWithValue("FechaInicio", sesion.FechaInicio);
            if (sesion.FechaFin != null) comando.Parameters.AddWithValue("FechaFin", sesion.FechaFin);
            if (sesion.idUsuario != null) comando.Parameters.AddWithValue("idUsuario", sesion.idUsuario);
            if (!string.IsNullOrEmpty(sesion.NombreUsuario)) comando.Parameters.AddWithValue("NombreUsuario", sesion.NombreUsuario);
            if (!string.IsNullOrEmpty(sesion.VC_CLIENTE)) comando.Parameters.AddWithValue("clienteDesde", sesion.VC_CLIENTE);
            if (sesion.NPedidos != null) comando.Parameters.AddWithValue("NPedidos", sesion.NPedidos);
            comando.CommandText = "SP_SesionesData";
            return SqlDataBase.fillDataSet(comando);
        }

        public int SesionesGuardar(ref Sesion sesion, bool EsLogin)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (sesion.FechaInicio != null) comando.Parameters.AddWithValue("FechaInicio", sesion.FechaInicio);
            if (sesion.FechaFin != null) comando.Parameters.AddWithValue("FechaFin", sesion.FechaFin);
            if (sesion.idUsuario != null) comando.Parameters.AddWithValue("idUsuario", sesion.idUsuario);
            if (!string.IsNullOrEmpty(sesion.NombreUsuario)) comando.Parameters.AddWithValue("NombreUsuario", sesion.NombreUsuario);
            if (!string.IsNullOrEmpty(sesion.VC_CLIENTE)) comando.Parameters.AddWithValue("VC_CLIENTE", sesion.VC_CLIENTE);
            if (sesion.NPedidos != null) comando.Parameters.AddWithValue("NPedidos", sesion.NPedidos);
            comando.Parameters.AddWithValue("EsLogin", EsLogin);
            comando.Parameters.Add(new SqlParameter()
            {
                SqlDbType = SqlDbType.BigInt,
                Direction = ParameterDirection.Output,
                ParameterName = "idSesion"
            });

            comando.CommandText = "SP_SesionesGuardar";

            int result = SqlDataBase.executeUpdate(comando);
            sesion.idSesion = (long)comando.Parameters["idPedido"].Value;
            return result;

        }

        #endregion

        #region BUSQUEDAS FALLIDAS

        public DataSet BusquedasFallidasData(string Referencia, DateTime? FechaDesde, DateTime? FechaHasta, string VC_CLIENTE)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Referencia)) comando.Parameters.AddWithValue("Referencia", Referencia);
            if (FechaDesde != null) comando.Parameters.AddWithValue("FechaDesde", FechaDesde);
            if (FechaHasta != null) comando.Parameters.AddWithValue("FechaHasta", FechaHasta);
            if (!string.IsNullOrEmpty(VC_CLIENTE)) comando.Parameters.AddWithValue("VC_CLIENTE", VC_CLIENTE);
            comando.CommandText = "SP_BusquedasFallidasData";
            return SqlDataBase.fillDataSet(comando);
        }

        public int BusquedasFallidasGuardar(string Referencia, string VC_CLIENTE)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("Referencia", Referencia);
            comando.Parameters.AddWithValue("VC_CLIENTE", VC_CLIENTE);
            comando.CommandText = "SP_BusquedasFallidasGuardar";
            return SqlDataBase.executeUpdate(comando);
        }

        public int BusquedasFallidasEliminar(string Referencia)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("Referencia", Referencia);
            comando.CommandText = "SP_BusquedasFallidasEliminar";
            return SqlDataBase.executeUpdate(comando);
        }
        #endregion

        #region CONFIGURACION

        public DataSet ConfigData(string parametro)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(parametro)) comando.Parameters.AddWithValue("parametro", parametro);
            comando.CommandText = "SP_ConfigData";
            return SqlDataBase.fillDataSet(comando);
        }

        public int ConfiguracionGuardar(string parametro, string valor)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("parametro", parametro);
            comando.Parameters.AddWithValue("valor", valor);
            comando.CommandText = "SP_ConfiguracionGuardar";
            return SqlDataBase.executeUpdate(comando);
        }

        #endregion

        #region FACTURAS

        public DataSet FacturasData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturasData";
            if (!string.IsNullOrEmpty(cliente)) comando.Parameters.AddWithValue("cliente", cliente);
            if (fechaDesde != null) comando.Parameters.AddWithValue("fechaDesde", fechaDesde);
            if (fechaHasta != null) comando.Parameters.AddWithValue("fechaHasta", fechaHasta);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet FacturasDataDesglosada(string cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturasDataDesglosada";
            if (!string.IsNullOrEmpty(cliente)) comando.Parameters.AddWithValue("@cliente", cliente);
            if (fechaDesde != null) comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            if (fechaHasta != null) comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);
            return SqlDataBase.fillDataSet(comando);
        }

        public int FacturaActualizarFichero(string VF_FACTURA, string fichero)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturaActualizarFichero";
            comando.Parameters.AddWithValue("VF_FACTURA", VF_FACTURA);
            comando.Parameters.AddWithValue("fichero", fichero);
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet FacturasGuardar(string cliente, string nombre_cliente, string factura, DateTime? fecha_fact, string albaran
                                     , DateTime? fecha_alb, string articulo, string descarticulo, string categoria, decimal unidades
                                     , decimal pventa, decimal porc_dcto, decimal imp_dcto, decimal neto, decimal porc_igic, decimal VF_LINEA)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturasGuardar";
            comando.Parameters.AddWithValue("cliente", cliente);
            comando.Parameters.AddWithValue("nombre_cliente", nombre_cliente);
            comando.Parameters.AddWithValue("factura", factura);
            if (fecha_fact != null) comando.Parameters.AddWithValue("fecha_fact", fecha_fact);
            comando.Parameters.AddWithValue("albaran", albaran);
            if (fecha_alb != null) comando.Parameters.AddWithValue("fecha_alb", fecha_alb);
            comando.Parameters.AddWithValue("articulo", articulo);
            comando.Parameters.AddWithValue("descarticulo", descarticulo);
            comando.Parameters.AddWithValue("categoria", categoria);
            comando.Parameters.AddWithValue("unidades", unidades);
            comando.Parameters.AddWithValue("pventa", pventa);
            comando.Parameters.AddWithValue("porc_dcto", porc_dcto);
            comando.Parameters.AddWithValue("imp_dcto", imp_dcto);
            comando.Parameters.AddWithValue("neto", neto);
            comando.Parameters.AddWithValue("porc_igic", porc_igic);
            comando.Parameters.AddWithValue("VF_LINEA", VF_LINEA);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet FacturaData(int idFactura)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturaData";
            comando.Parameters.AddWithValue("idFactura", idFactura);
            return SqlDataBase.fillDataSet(comando);
        }

        public int FacturasPorClienteBorrar(string cliente)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturasPorClienteBorrar";
            comando.Parameters.AddWithValue("cliente", cliente);
            return SqlDataBase.executeUpdate(comando);
        }

        public int FacturasBorrarSiExiste(string factura, string albaran, decimal VF_LINEA)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FacturasBorrarSiExiste";
            comando.Parameters.AddWithValue("factura", factura);
            comando.Parameters.AddWithValue("albaran", albaran);
            comando.Parameters.AddWithValue("linea", VF_LINEA);
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet DatosFacturaPDF(string VF_FACTURA)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_DatosFacturaPDF";
            comando.Parameters.AddWithValue("VF_FACTURA", VF_FACTURA);
            return SqlDataBase.fillDataSet(comando);
        }
       
        #endregion

        #region ALBARANES

        public DataSet AlbaranesData(string cliente, DateTime? fechaDesde, DateTime? fechaHasta, bool? showAll)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_AlbaranesData";
            if (!string.IsNullOrEmpty(cliente)) comando.Parameters.AddWithValue("cliente", cliente);
            if (fechaDesde != null) comando.Parameters.AddWithValue("fechaDesde", fechaDesde);
            if (fechaHasta != null) comando.Parameters.AddWithValue("fechaHasta", fechaHasta);
            if (showAll != null) comando.Parameters.AddWithValue("showAll", showAll);
            return SqlDataBase.fillDataSet(comando);
        }

        public int AlbaranActualizarFichero(string VF_ALBARAN, string fichero)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_AlbaranActualizarFichero";
            comando.Parameters.AddWithValue("VF_ALBARAN", VF_ALBARAN);
            comando.Parameters.AddWithValue("fichero", fichero);
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet DatosAlbaranPDF(string VF_ALBARAN)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_DatosAlbaranPDF";
            comando.Parameters.AddWithValue("VF_ALBARAN", VF_ALBARAN);
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region VENCIMIENTOS

        public DataSet VencimientosData(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_VencimientosData";
            if (cliente != null) comando.Parameters.AddWithValue("@cliente", cliente);
            if (fechaDesde != null) comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            if (fechaHasta != null) comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet VencimientosDataDesglosada(decimal? cliente, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_VencimientosDataDesglosada";
            if (cliente != null) comando.Parameters.AddWithValue("@cliente", cliente);
            if (fechaDesde != null) comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            if (fechaHasta != null) comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet VencimientosGuardar(decimal cliente, string factura, long documento, string tipo_doc,
                                           DateTime emision, DateTime vencimiento, decimal importe, decimal estado)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_VencimientosGuardar";
            comando.Parameters.AddWithValue("cliente", cliente);
            comando.Parameters.AddWithValue("factura", factura);
            comando.Parameters.AddWithValue("documento", documento);
            comando.Parameters.AddWithValue("tipo_doc", tipo_doc);
            comando.Parameters.AddWithValue("emision", emision);
            comando.Parameters.AddWithValue("vencimiento", vencimiento);
            comando.Parameters.AddWithValue("importe", importe);
            comando.Parameters.AddWithValue("estado", estado);
            return SqlDataBase.fillDataSet(comando);
        }

        public int EfectoActualizarFichero(string VE_DOCUMENTO, string fichero)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_EfectoActualizarFichero";
            comando.Parameters.AddWithValue("VE_DOCUMENTO", VE_DOCUMENTO);
            comando.Parameters.AddWithValue("fichero", fichero);
            return SqlDataBase.executeUpdate(comando);
        }

        public int VencimientosBorrarSiExiste(long documento)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_VencimientosBorrarSiExiste";
            comando.Parameters.AddWithValue("documento", documento);
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet DatosEfectoPDF(string VE_DOCUMENTO)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_DatosEfectoPDF";
            comando.Parameters.AddWithValue("VE_DOCUMENTO", VE_DOCUMENTO);
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        //#region MENSAJES

        //public DataSet MensajesData(int? idBuzon, int? idMensaje, DateTime? FechaCreacionDesde, DateTime? FechaCreacionHasta,
        //                            DateTime? FechaEnvioDesde, DateTime? FechaEnvioHasta, int? idUsuarioRemitente,
        //                            int? idUsuarioDestinatario, bool? Leido)
        //{
        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "SP_MensajesData";
        //    if (idBuzon != null) comando.Parameters.AddWithValue("idBuzon", idBuzon);
        //    if (idMensaje != null) comando.Parameters.AddWithValue("idMensaje", idMensaje);
        //    if (FechaCreacionDesde != null) comando.Parameters.AddWithValue("FechaCreacionDesde", FechaCreacionDesde);
        //    if (FechaCreacionHasta != null) comando.Parameters.AddWithValue("FechaCreacionHasta", FechaCreacionHasta);
        //    if (FechaEnvioDesde != null) comando.Parameters.AddWithValue("FechaEnvioDesde", FechaEnvioDesde);
        //    if (FechaEnvioHasta != null) comando.Parameters.AddWithValue("FechaEnvioHasta", FechaEnvioDesde);
        //    if (idUsuarioRemitente != null) comando.Parameters.AddWithValue("idUsuarioRemitente", idUsuarioRemitente);
        //    if (idUsuarioDestinatario != null) comando.Parameters.AddWithValue("idUsuarioDestinatario", idUsuarioDestinatario);
        //    if (Leido != null) comando.Parameters.AddWithValue("Leido", Leido);
        //    return SqlDataBase.fillDataSet(comando);
        //}

        //public int MensajeGuardar(int idBuzon, int RemitenteIdUsuario, int DestinatarioIdUsuario, string Asunto, string Contenido)
        //{
        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "SP_MensajeGuardar";
        //    comando.Parameters.AddWithValue("idBuzon", idBuzon);
        //    comando.Parameters.AddWithValue("RemitenteIdUsuario", RemitenteIdUsuario);
        //    comando.Parameters.AddWithValue("DestinatarioIdUsuario", DestinatarioIdUsuario);
        //    comando.Parameters.AddWithValue("Asunto", Asunto);
        //    comando.Parameters.AddWithValue("Contenido", Contenido);
        //    return SqlDataBase.executeUpdate(comando);
        //}

        //public DataSet MensajeGuardarEnviarPorEmail(int idBuzon, int RemitenteIdUsuario, int DestinatarioIdUsuario, string Asunto, string Contenido)
        //{
        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "SP_MensajeGuardar";
        //    comando.Parameters.AddWithValue("idBuzon", idBuzon);
        //    comando.Parameters.AddWithValue("RemitenteIdUsuario", RemitenteIdUsuario);
        //    comando.Parameters.AddWithValue("DestinatarioIdUsuario", DestinatarioIdUsuario);
        //    comando.Parameters.AddWithValue("Asunto", Asunto);
        //    comando.Parameters.AddWithValue("Contenido", Contenido);
        //    return SqlDataBase.fillDataSet(comando);
        //}

        //public int MensajeGuardar(int idMensaje, DateTime? FechaEnvio)
        //{
        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "SP_MensajeGuardar";
        //    comando.Parameters.AddWithValue("idMensaje", idMensaje);
        //    if (FechaEnvio != null) comando.Parameters.AddWithValue("FechaEnvio", FechaEnvio);
        //    return SqlDataBase.executeUpdate(comando);
        //}

        //public int MensajeMarcarComoLeido(int idMensaje)
        //{
        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "SP_MensajeMarcarComoLeido";
        //    comando.Parameters.AddWithValue("idMensaje", idMensaje);
        //    return SqlDataBase.executeUpdate(comando);
        //}

        //#endregion

        #region BUZON

        public DataSet BuzonesCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_listaBuzonesCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region CLIENTES

        public DataSet ClientesCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_listaClientesCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ClientesData()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ClientesGetData";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ClientesGuardar(string cliente, string nombre, string denominacion, string cif, string direccion
                                     , string direccionb, string tfno, string fax, string poblacion, string provincia
                                     , string codpostal, string formapago, string zona, int pvp, string VC_CODFORMAPAGO
                                     , string VC_BANCO, string VC_BANCO_DIRECCION, string VC_BANCO_POBLACION, string VC_BANCO_CUENTA)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ClientesGuardar";
            comando.Parameters.AddWithValue("cliente", cliente);
            comando.Parameters.AddWithValue("nombre", nombre);
            comando.Parameters.AddWithValue("denominacion", denominacion);
            comando.Parameters.AddWithValue("cif", cif);
            comando.Parameters.AddWithValue("direccion", direccion);
            comando.Parameters.AddWithValue("direccionb", direccionb);
            comando.Parameters.AddWithValue("tfno", tfno);
            comando.Parameters.AddWithValue("fax", fax);
            comando.Parameters.AddWithValue("poblacion", poblacion);
            comando.Parameters.AddWithValue("provincia", provincia);
            comando.Parameters.AddWithValue("codpostal", codpostal);
            comando.Parameters.AddWithValue("formapago", formapago);
            comando.Parameters.AddWithValue("zona", zona);
            comando.Parameters.AddWithValue("pvp", pvp);
            comando.Parameters.AddWithValue("VC_CODFORMAPAGO", VC_CODFORMAPAGO);
            comando.Parameters.AddWithValue("VC_BANCO", VC_BANCO);
            comando.Parameters.AddWithValue("VC_BANCO_DIRECCION", VC_BANCO_DIRECCION);
            comando.Parameters.AddWithValue("VC_BANCO_POBLACION", VC_BANCO_POBLACION);
            comando.Parameters.AddWithValue("VC_BANCO_CUENTA", VC_BANCO_CUENTA);
            return SqlDataBase.fillDataSet(comando);
        }

        public int ClientesBorrarSiExiste(string cliente)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ClientesBorrarSiExiste";
            comando.Parameters.AddWithValue("cliente", cliente);
            DataSet ds = SqlDataBase.fillDataSet(comando);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return -1;
            int existe = -1;
            Int32.TryParse(ds.Tables[0].Rows[0][0].ToString(), out existe);
            return existe;
        }

        #endregion

        #region USUARIOS

        public DataSet ObtenerDatosUsuario (int idUsuario)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_UsuariosById";
            comando.Parameters.AddWithValue("idUsuario", idUsuario);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet UsuariosData(bool? showAdmin, bool? showStaff, bool? showClientes, bool? bloqueado)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_UsuariosGetData";
            comando.Parameters.AddWithValue("showAdmin", showAdmin);
            comando.Parameters.AddWithValue("showStaff", showStaff);
            comando.Parameters.AddWithValue("showClientes", showClientes);
            comando.Parameters.AddWithValue("bloqueado", bloqueado);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet Login(string userName, string pass)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_Login";
            comando.Parameters.AddWithValue("userName", userName);
            comando.Parameters.AddWithValue("pass", pass);
            return SqlDataBase.fillDataSet(comando);
        }

        public int Logout(long idSesion)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_Logout";
            comando.Parameters.AddWithValue("idSesion", idSesion);
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet PerfilUsuario(string VC_CLIENTE)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PerfilUsuario";
            comando.Parameters.AddWithValue("VC_CLIENTE", VC_CLIENTE);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet UsuariosGuardar(int? idUsuario, string login, string pass, int? tipo, string VC_CLIENTE, bool? bloqueado,
                    string AlmacenPreferido, bool? ConfirmarAuto, string Email, bool? GenerarPdfPedido, bool? MostrarMinuaturas,
                    bool? NotificarPedido, int? NRuedasPreselec, decimal DtoB2B, string TipoBusqueda)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_UsuariosGuardar";
            if (idUsuario != null) comando.Parameters.AddWithValue("idUsuario", idUsuario);
            if (!string.IsNullOrEmpty(login)) comando.Parameters.AddWithValue("login", login);
            if (!string.IsNullOrEmpty(pass)) comando.Parameters.AddWithValue("pass", pass);
            if (tipo != null) comando.Parameters.AddWithValue("tipo", tipo);
            if (VC_CLIENTE != null) comando.Parameters.AddWithValue("VC_CLIENTE", VC_CLIENTE);
            if (bloqueado != null) comando.Parameters.AddWithValue("bloqueado", bloqueado);
            if (!string.IsNullOrEmpty(AlmacenPreferido)) comando.Parameters.AddWithValue("AlmacenPreferido", AlmacenPreferido);
            if (ConfirmarAuto != null) comando.Parameters.AddWithValue("ConfirmarAuto", ConfirmarAuto);
            if (!string.IsNullOrEmpty(Email)) comando.Parameters.AddWithValue("Email", Email);
            if (GenerarPdfPedido != null) comando.Parameters.AddWithValue("GenerarPdfPedido", GenerarPdfPedido);
            if (MostrarMinuaturas != null) comando.Parameters.AddWithValue("MostrarMinuaturas", MostrarMinuaturas);
            if (NotificarPedido != null) comando.Parameters.AddWithValue("NotificarPedido", NotificarPedido);
            if (NRuedasPreselec != null) comando.Parameters.AddWithValue("NRuedasPreselec", NRuedasPreselec);
            if (!string.IsNullOrEmpty(TipoBusqueda)) comando.Parameters.AddWithValue("TipoBusqueda", TipoBusqueda);
            comando.Parameters.AddWithValue("DtoB2B", DtoB2B);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet UsuariosEliminar(int idUsuario)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_UsuariosEliminar";
            if (idUsuario != null) comando.Parameters.AddWithValue("idUsuario", idUsuario);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet UsuarioTipoBusquedaGuardar(int idUsuario, string tipoBusqueda)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_UsuarioTipoBusquedaGuardar";
            if (idUsuario != null) comando.Parameters.AddWithValue("idUsuario", idUsuario);
            if (!string.IsNullOrEmpty(tipoBusqueda)) comando.Parameters.AddWithValue("tipoBusqueda", tipoBusqueda);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet PerfilUsuarioGuardar(int? idUsuario, string AlmacenPreferido, bool? ConfirmarAuto, string Email,
            bool? GenerarPdfPedido, bool? MostrarMinuaturas, bool? NotificarPedido, int? NRuedasPreselec)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PerfilUsuarioGuardar";
            if (idUsuario != null) comando.Parameters.AddWithValue("idUsuario", idUsuario);
            if (!string.IsNullOrEmpty(AlmacenPreferido)) comando.Parameters.AddWithValue("AlmacenPreferido", AlmacenPreferido);
            if (ConfirmarAuto != null) comando.Parameters.AddWithValue("ConfirmarAuto", ConfirmarAuto);
            if (!string.IsNullOrEmpty(Email)) comando.Parameters.AddWithValue("Email", Email);
            if (GenerarPdfPedido != null) comando.Parameters.AddWithValue("GenerarPdfPedido", GenerarPdfPedido);
            if (MostrarMinuaturas != null) comando.Parameters.AddWithValue("MostrarMinuaturas", MostrarMinuaturas);
            if (NotificarPedido != null) comando.Parameters.AddWithValue("NotificarPedido", NotificarPedido);
            if (NRuedasPreselec != null) comando.Parameters.AddWithValue("NRuedasPreselec", NRuedasPreselec);
            return SqlDataBase.fillDataSet(comando);
        }

        public int CambiarPassword(string login, string pass, string newPass)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_CambiarPassword";
            comando.Parameters.AddWithValue("username", login);
            comando.Parameters.AddWithValue("pass", pass);
            comando.Parameters.AddWithValue("newPass", newPass);
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet listaRolesCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_listaRolesCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet listaTiposUsuario()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_listaTiposUsuario";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet listaUsuariosCombo(string tipo)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(tipo)) comando.Parameters.AddWithValue("tipo", tipo);
            comando.CommandText = "SP_listaUsuariosCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet listaUsuariosPorTipo(string tipo)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(tipo)) comando.Parameters.AddWithValue("tipo", tipo);
            comando.CommandText = "SP_listaUsuariosPorTipo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet PermisosData(int? idUsuario, int? idRole, string CodModulo)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PermisosData";
            if (idUsuario != null) comando.Parameters.AddWithValue("idUsuario", idUsuario);
            if (idUsuario != null) comando.Parameters.AddWithValue("idRole", idRole);
            if (!string.IsNullOrEmpty(CodModulo)) comando.Parameters.AddWithValue("CodModulo", CodModulo);
            return SqlDataBase.fillDataSet(comando);
        }

        public int PermisosGuardar(int idUsuario, string xmlPermisos)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PermisosGuardar";
            comando.Parameters.AddWithValue("idUsuario", idUsuario);
            comando.Parameters.AddWithValue("xmlPermisos", xmlPermisos);
            return SqlDataBase.executeUpdate(comando);
        }

        #endregion

        #region PEDIDOS

        public DataSet EstadosCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_listaEstadossCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet PedidosGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                                        string dirEnvio, decimal? baseImp, decimal? descuento,
                                        decimal? nfu, decimal? igic, string observaciones, ref long idPedido)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idUsuario", idUsuario);
            comando.Parameters.AddWithValue("FechaEnvio", fechaEnvio);
            comando.Parameters.AddWithValue("FechaEntrega", fechaEntrega);
            comando.Parameters.AddWithValue("PorAgencia", porAgencia);
            comando.Parameters.AddWithValue("DirEnvio", dirEnvio);
            comando.Parameters.AddWithValue("BaseImponible", baseImp);
            comando.Parameters.AddWithValue("Descuento", descuento);
            comando.Parameters.AddWithValue("NFU", nfu);
            comando.Parameters.AddWithValue("IGIC", igic);
            comando.Parameters.AddWithValue("Observaciones", observaciones.ToLowerInvariant());
            comando.Parameters.Add(new SqlParameter()
            {
                SqlDbType = SqlDbType.BigInt,
                Direction = ParameterDirection.Output,
                ParameterName = "idPedido"
            });
            comando.CommandText = "SP_PedidosGuardar";
            DataSet dsResult = SqlDataBase.fillDataSet(comando);
            idPedido = (long)comando.Parameters["idPedido"].Value;
            return dsResult;
        }

        public DataSet ProductosPedidoGetData(long idPedido)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idPedido", idPedido);
            comando.CommandText = "SP_ProductosPedidoGetData";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet PedidosData(long? idPedido, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE, string AlmacenPreferido)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (idPedido != null) comando.Parameters.AddWithValue("idPedido", idPedido);
            if (idEstado != null) comando.Parameters.AddWithValue("idEstado", idEstado);
            if (fechaDesde != null) comando.Parameters.AddWithValue("fechaDesde", fechaDesde);
            if (fechaHasta != null) comando.Parameters.AddWithValue("fechaHasta", fechaHasta);
            if (!string.IsNullOrEmpty(VC_CLIENTE)) comando.Parameters.AddWithValue("VC_CLIENTE", VC_CLIENTE);
            if (!string.IsNullOrEmpty(AlmacenPreferido)) comando.Parameters.AddWithValue("AlmacenPreferido", AlmacenPreferido);
            comando.CommandText = "SP_PedidosGetData";
            return SqlDataBase.fillDataSet(comando);
        }

        public int ActualizarEstadoPedido(long idPedido, DateTime? fecha)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idPedido", idPedido);
            if (fecha != null) comando.Parameters.AddWithValue("fecha", fecha);
            comando.CommandText = "SP_ActualizarEstadoPedido";
            return SqlDataBase.executeUpdate(comando);
        }

        public int AnularPedido(long idPedido)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idPedido", idPedido);
            comando.CommandText = "SP_AnularPedido";
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet EstadoActualYSiguiente(int idEstado)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idEstado", idEstado);
            comando.CommandText = "SP_EstadoActualYSiguiente";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region RESERVAS

        public DataSet EstadosReservaCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_listaEstadosReservaCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ReservasGuardar(int idUsuario, DateTime? fechaEnvio, DateTime? fechaEntrega, bool? porAgencia,
                                        string dirEnvio, decimal? baseImp, decimal? descuento,
                                        decimal? nfu, decimal? igic, string observaciones, ref long idReserva)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idUsuario", idUsuario);
            comando.Parameters.AddWithValue("FechaEnvio", fechaEnvio);
            comando.Parameters.AddWithValue("FechaEntrega", fechaEntrega);
            comando.Parameters.AddWithValue("PorAgencia", porAgencia);
            comando.Parameters.AddWithValue("DirEnvio", dirEnvio);
            comando.Parameters.AddWithValue("BaseImponible", baseImp);
            comando.Parameters.AddWithValue("Descuento", descuento);
            comando.Parameters.AddWithValue("NFU", nfu);
            comando.Parameters.AddWithValue("IGIC", igic);
            comando.Parameters.AddWithValue("Observaciones", observaciones.ToLowerInvariant());
            comando.Parameters.Add(new SqlParameter()
            {
                SqlDbType = SqlDbType.BigInt,
                Direction = ParameterDirection.Output,
                ParameterName = "idReserva"
            });
            comando.CommandText = "SP_ReservasGuardar";
            DataSet dsResult = SqlDataBase.fillDataSet(comando);
            idReserva = (long)comando.Parameters["idReserva"].Value;
            return dsResult;
        }

        public DataSet ProductosReservaGetData(long idReserva)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idReserva", idReserva);
            comando.CommandText = "SP_ProductosReservaGetData";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ReservasData(long? idReserva, int? idEstado, DateTime? fechaDesde, DateTime? fechaHasta, string VC_CLIENTE)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            if (idReserva != null) comando.Parameters.AddWithValue("idReserva", idReserva);
            if (idEstado != null) comando.Parameters.AddWithValue("idEstado", idEstado);
            if (fechaDesde != null) comando.Parameters.AddWithValue("fechaDesde", fechaDesde);
            if (fechaHasta != null) comando.Parameters.AddWithValue("fechaHasta", fechaHasta);
            if (!string.IsNullOrEmpty(VC_CLIENTE)) comando.Parameters.AddWithValue("VC_CLIENTE", VC_CLIENTE);
            comando.CommandText = "SP_ReservasGetData";
            return SqlDataBase.fillDataSet(comando);
        }

        public int ActualizarEstadoReserva(long idReserva, DateTime? fecha)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idPedido", idReserva);
            if (fecha != null) comando.Parameters.AddWithValue("fecha", fecha);
            comando.CommandText = "SP_ActualizarEstadoReserva";
            return SqlDataBase.executeUpdate(comando);
        }

        public int AnularReserva(long idReserva)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idReserva", idReserva);
            comando.CommandText = "SP_AnularReserva";
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet EstadoReservaActualYSiguiente(int idEstado)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idEstado", idEstado);
            comando.CommandText = "SP_EstadoReservaActualYSiguiente";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region PRODUCTOS

        public DataSet ProductosBuscadorStaff(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, string ic, string iv, string referencia2)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ProductosBuscadorStaff";
            comando.CommandTimeout = 60;
            if (idUsuario > 0) comando.Parameters.AddWithValue("idUsuario", idUsuario);
            if (!string.IsNullOrEmpty(referencia)) comando.Parameters.AddWithValue("referencia", referencia);
            if (!string.IsNullOrEmpty(familia)) comando.Parameters.AddWithValue("familia", familia);
            if (!string.IsNullOrEmpty(modelo)) comando.Parameters.AddWithValue("modelo", modelo);
            if (tipoNeuma != null) comando.Parameters.AddWithValue("tipoNeuma", tipoNeuma);
            if (!string.IsNullOrEmpty(ic)) comando.Parameters.AddWithValue("ic", ic);
            if (!string.IsNullOrEmpty(iv)) comando.Parameters.AddWithValue("iv", iv);
            if (!string.IsNullOrEmpty(referencia2)) comando.Parameters.AddWithValue("referencia2", referencia2);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ProductosBuscador(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma, 
            string ic, string iv,string referencia2)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ProductosBuscador";
            comando.CommandTimeout = 60;
            comando.Parameters.AddWithValue("idUsuario", idUsuario);
            if (!string.IsNullOrEmpty(referencia)) comando.Parameters.AddWithValue("referencia", referencia);
            if (!string.IsNullOrEmpty(familia)) comando.Parameters.AddWithValue("familia", familia);
            if (!string.IsNullOrEmpty(modelo)) comando.Parameters.AddWithValue("modelo", modelo);
            if (tipoNeuma != null) comando.Parameters.AddWithValue("tipoNeuma", tipoNeuma);
            if (!string.IsNullOrEmpty(ic)) comando.Parameters.AddWithValue("ic", ic);
            if (!string.IsNullOrEmpty(iv)) comando.Parameters.AddWithValue("iv", iv);
            if (!string.IsNullOrEmpty(referencia2)) comando.Parameters.AddWithValue("referencia2", referencia2);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ProductosBuscadorV3(int idUsuario, string referencia, string familia, string modelo, int? tipoNeuma,
            string ic, string iv, string referencia2, int pagina, int regPagina, ref int nFilasTotales, string ordenarPor, string ordenAscDesc)
        {
            int inicio = ((pagina - 1) * regPagina) + 1;
            int fin = inicio + regPagina;

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ProductosBuscadorV3";
            comando.CommandTimeout = 60;
            comando.Parameters.AddWithValue("idUsuario", idUsuario);
            if (!string.IsNullOrEmpty(referencia)) comando.Parameters.AddWithValue("referencia", referencia);
            if (!string.IsNullOrEmpty(familia)) comando.Parameters.AddWithValue("familia", familia);
            if (!string.IsNullOrEmpty(modelo)) comando.Parameters.AddWithValue("modelo", modelo);
            if (tipoNeuma != null) comando.Parameters.AddWithValue("tipoNeuma", tipoNeuma);
            if (!string.IsNullOrEmpty(ic)) comando.Parameters.AddWithValue("ic", ic);
            if (!string.IsNullOrEmpty(iv)) comando.Parameters.AddWithValue("iv", iv);
            if (!string.IsNullOrEmpty(referencia2)) comando.Parameters.AddWithValue("referencia2", referencia2);
            comando.Parameters.AddWithValue("RegistroInicio", inicio);
            comando.Parameters.AddWithValue("RegistroFin", fin);
            comando.Parameters.AddWithValue("nFilas", nFilasTotales);
            comando.Parameters.AddWithValue("ordenarPor", ordenarPor);
            comando.Parameters.AddWithValue("ordenAscDesc", ordenAscDesc);
            DataSet ds = SqlDataBase.fillDataSet(comando);
            return ds;
        }

        public DataSet ProductosGuardar(string familia, string descfam, string producto, string descripcion
                                      , string producto1, string modelo, decimal? serie, decimal? llanta, string medida
                                      , string ic, string iv, decimal? tipo_neuma, string desc_tipo
                                      , decimal pvp1, decimal pvp2, decimal pvp3, int tipo_ofer, string categoria
                                      , decimal VP_PORC_IMP, string VP_IMAGEN, int? VP_IMPORTADO
                                      , decimal VP_NIVELRUIDO, string VP_EFICOMBUSTIBLE, string VP_ADHERENCIA, decimal VP_VALORRUIDO)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ProductosGuardar";
            comando.Parameters.AddWithValue("familia", familia);
            comando.Parameters.AddWithValue("descfam", (string.IsNullOrEmpty(descfam) ? "" : descfam));
            comando.Parameters.AddWithValue("producto", (string.IsNullOrEmpty(producto) ? "" : producto));
            comando.Parameters.AddWithValue("descripcion", (string.IsNullOrEmpty(descripcion) ? "" : descripcion));
            comando.Parameters.AddWithValue("producto1", (string.IsNullOrEmpty(producto1) ? "" : producto1));
            comando.Parameters.AddWithValue("modelo", (string.IsNullOrEmpty(modelo) ? "" : modelo));
            comando.Parameters.AddWithValue("serie", serie);
            comando.Parameters.AddWithValue("llanta", llanta);
            comando.Parameters.AddWithValue("medida", medida);
            comando.Parameters.AddWithValue("ic", (string.IsNullOrEmpty(ic) ? "" : ic));
            comando.Parameters.AddWithValue("iv", (string.IsNullOrEmpty(iv) ? "" : iv));
            comando.Parameters.AddWithValue("tipo_neuma", tipo_neuma);
            comando.Parameters.AddWithValue("desc_tipo", (string.IsNullOrEmpty(desc_tipo) ? "" : desc_tipo));
            comando.Parameters.AddWithValue("pvp1", pvp1);
            comando.Parameters.AddWithValue("pvp2", pvp2);
            comando.Parameters.AddWithValue("pvp3", pvp3);
            comando.Parameters.AddWithValue("tipo_ofer", tipo_ofer);
            comando.Parameters.AddWithValue("categoria", (string.IsNullOrEmpty(categoria) ? "" : categoria));
            comando.Parameters.AddWithValue("VP_PORC_IMP", VP_PORC_IMP);
            if (!string.IsNullOrEmpty(VP_IMAGEN)) comando.Parameters.AddWithValue("VP_IMAGEN", VP_IMAGEN);
            if (VP_IMPORTADO != null) comando.Parameters.AddWithValue("VP_IMPORTADO", VP_IMPORTADO);
            comando.Parameters.AddWithValue("VP_NIVELRUIDO", VP_NIVELRUIDO);
            comando.Parameters.AddWithValue("VP_EFICOMBUSTIBLE", VP_EFICOMBUSTIBLE);
            comando.Parameters.AddWithValue("VP_ADHERENCIA", VP_ADHERENCIA);
            comando.Parameters.AddWithValue("VP_VALORRUIDO", VP_VALORRUIDO);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ProductosData(string VP_FAMILIA, string VP_PRODUCTO, DateTime? MODIFICADO)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ProductosGetData";
            if (!string.IsNullOrEmpty(VP_FAMILIA)) comando.Parameters.AddWithValue("VP_FAMILIA", VP_FAMILIA);
            if (!string.IsNullOrEmpty(VP_PRODUCTO)) comando.Parameters.AddWithValue("VP_PRODUCTO", VP_PRODUCTO);
            if (MODIFICADO != null) comando.Parameters.AddWithValue("MODIFICADO", MODIFICADO);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ProductosDataV3(string VP_PRODUCTO)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ProductosGetData";
            if (!string.IsNullOrEmpty(VP_PRODUCTO)) comando.Parameters.AddWithValue("VP_PRODUCTO", VP_PRODUCTO);
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region PRODUCTOS PEDIDO

        public int ProductosPedidoGuardar(long idPedido, string VP_PRODUCTO, int Unidades, decimal PrecioUnidad, string VP_CATEGORIA, decimal Ecotasa, decimal VT_PORC_IMP, decimal VP_PORC_IMP)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idPedido", idPedido);
            comando.Parameters.AddWithValue("VP_PRODUCTO", VP_PRODUCTO);
            comando.Parameters.AddWithValue("Unidades", Unidades);
            comando.Parameters.AddWithValue("PrecioUnidad", PrecioUnidad);
            if (!string.IsNullOrEmpty(VP_CATEGORIA)) comando.Parameters.AddWithValue("VP_CATEGORIA", VP_CATEGORIA);
            comando.Parameters.AddWithValue("Ecotasa", Ecotasa);
            comando.Parameters.AddWithValue("VT_PORC_IMP", VT_PORC_IMP);
            comando.Parameters.AddWithValue("VP_PORC_IMP", VP_PORC_IMP);
            comando.CommandText = "SP_ProductosPedidoGuardar";
            return SqlDataBase.executeUpdate(comando);
        }

        public int ProductosReservaGuardar(long idReserva, string VP_PRODUCTO, int Unidades, decimal PrecioUnidad, string VP_CATEGORIA, decimal Ecotasa, decimal VT_PORC_IMP, decimal VP_PORC_IMP)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idReserva", idReserva);
            comando.Parameters.AddWithValue("VP_PRODUCTO", VP_PRODUCTO);
            comando.Parameters.AddWithValue("Unidades", Unidades);
            comando.Parameters.AddWithValue("PrecioUnidad", PrecioUnidad);
            if (!string.IsNullOrEmpty(VP_CATEGORIA)) comando.Parameters.AddWithValue("VP_CATEGORIA", VP_CATEGORIA);
            comando.Parameters.AddWithValue("Ecotasa", Ecotasa);
            comando.Parameters.AddWithValue("VT_PORC_IMP", VT_PORC_IMP);
            comando.Parameters.AddWithValue("VP_PORC_IMP", VP_PORC_IMP);
            comando.CommandText = "SP_ProductosReservaGuardar";
            return SqlDataBase.executeUpdate(comando);
        }

        public DataSet DetallesPedido(long idPedido)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idPedido", idPedido);
            comando.CommandText = "SP_DetallesPedido";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet DetallesReserva(long idReserva)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("idReserva", idReserva);
            comando.CommandText = "SP_DetallesReserva";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region PENDIENTES

        public DataSet PendientesPorProductoData(string producto)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PendientesPorProducto";
            comando.Parameters.AddWithValue("idProducto", producto);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet PendientesGetData(DateTime? llegada)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PendientesGetData";
            if (llegada != null) comando.Parameters.AddWithValue("llegada", llegada);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet PendientesGuardar(string articulo, DateTime llegada, string contenedor)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PendientesGuardar";
            comando.Parameters.AddWithValue("articulo", articulo);
            comando.Parameters.AddWithValue("llegada", llegada);
            comando.Parameters.AddWithValue("contenedor", contenedor);
            return SqlDataBase.fillDataSet(comando);
        }

        public int PendientesBorrarSiExiste(string articulo)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PendientesBorrarSiExiste";
            comando.Parameters.AddWithValue("articulo", articulo);
            return SqlDataBase.executeUpdate(comando);
        }

        #endregion
        
        #region COMBOS

        public DataSet FamiliasLogoCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FamiliasLogoCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet FamiliasCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_FamiliasCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet ModelosCombo(string familia)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_ModelosCombo";
            if (!string.IsNullOrEmpty(familia))comando.Parameters.AddWithValue("familia", familia);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet TipoNeumaticosCombo()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_TipoNeumaticosCombo";
            return SqlDataBase.fillDataSet(comando);
        }

        #endregion

        #region ECOTASA

        public DataSet EcotasasData()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_EcotasasGetData";
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet EcotasaGuardar(string categoria, string descripcion, string detalles, decimal pvp1, decimal VT_PORC_IMP)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_EcotasaGuardar";
            //comando.Parameters.AddWithValue("producto", producto);
            comando.Parameters.AddWithValue("categoria", categoria);
            comando.Parameters.AddWithValue("descripcion", descripcion);
            comando.Parameters.AddWithValue("detalles", detalles);
            comando.Parameters.AddWithValue("pvp1", pvp1);
            comando.Parameters.AddWithValue("VT_PORC_IMP", VT_PORC_IMP);
            return SqlDataBase.fillDataSet(comando);
        }

        public int EcotasaBorrarSiExiste(string categoria)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_EcotasaBorrarSiExiste";
            comando.Parameters.AddWithValue("categoria", categoria);
            return SqlDataBase.executeUpdate(comando);
        }

        #endregion

        #region STOCK

        public DataSet StockData()
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_StockGetData";
            return SqlDataBase.fillDataSet(comando);
        }

        //public DataSet StockGuardar(string familia, string descfamilia, string articulo, string descarticulo, string articulo1
        //                           , string modelo, string indvelocidad, string indcarga, string categoria
        //                           , decimal stock_a01, decimal stock_a02, decimal stock_a03, decimal stock_a04, decimal stock_a18
        //                           , decimal stock_a19, decimal stock_a22, decimal stock_a23, decimal stock_a32
        //                           , decimal stock_a44, decimal stock_a54, decimal stock_gen, decimal stock_a12
        //                           , decimal stock_a13, decimal stock_a55, decimal stock_a24
        //                           , decimal stock_a27, decimal stock_a29, decimal stock_a31, decimal stock_a43
        //                           , decimal stock_a45, decimal stock_a46, decimal stock_a47, decimal stock_a56
        //                            )
        //{
        //    SqlCommand comando = new SqlCommand();
        //    comando.CommandType = CommandType.StoredProcedure;
        //    comando.CommandText = "SP_StockGuardar";
        //    comando.Parameters.AddWithValue("familia", familia);
        //    comando.Parameters.AddWithValue("descfamilia", descfamilia);
        //    comando.Parameters.AddWithValue("articulo", articulo);
        //    comando.Parameters.AddWithValue("descarticulo", descarticulo);
        //    comando.Parameters.AddWithValue("articulo1", articulo1);
        //    comando.Parameters.AddWithValue("modelo", modelo);
        //    comando.Parameters.AddWithValue("indvelocidad", indvelocidad);
        //    comando.Parameters.AddWithValue("indcarga", indcarga);
        //    comando.Parameters.AddWithValue("categoria", categoria);
        //    comando.Parameters.AddWithValue("stock_a01", stock_a01);
        //    comando.Parameters.AddWithValue("stock_a02", stock_a02);
        //    comando.Parameters.AddWithValue("stock_a03", stock_a03);
        //    comando.Parameters.AddWithValue("stock_a04", stock_a04);
        //    comando.Parameters.AddWithValue("stock_a18", stock_a18);
        //    comando.Parameters.AddWithValue("stock_a19", stock_a19);
        //    comando.Parameters.AddWithValue("stock_a22", stock_a22);
        //    comando.Parameters.AddWithValue("stock_a23", stock_a23);
        //    comando.Parameters.AddWithValue("stock_a32", stock_a32);
        //    comando.Parameters.AddWithValue("stock_a44", stock_a44);
        //    comando.Parameters.AddWithValue("stock_a54", stock_a54);
        //    comando.Parameters.AddWithValue("stock_gen", stock_gen);
        //    comando.Parameters.AddWithValue("stock_a12", stock_a12);
        //    comando.Parameters.AddWithValue("stock_a13", stock_a13);
        //    comando.Parameters.AddWithValue("stock_a55", stock_a55);
        //    comando.Parameters.AddWithValue("stock_a24", stock_a24);
        //    comando.Parameters.AddWithValue("stock_a27", stock_a27);
        //    comando.Parameters.AddWithValue("stock_a29", stock_a29);
        //    comando.Parameters.AddWithValue("stock_a31", stock_a31);
        //    comando.Parameters.AddWithValue("stock_a43", stock_a43);
        //    comando.Parameters.AddWithValue("stock_a45", stock_a45);
        //    comando.Parameters.AddWithValue("stock_a46", stock_a46);
        //    comando.Parameters.AddWithValue("stock_a47", stock_a47);
        //    comando.Parameters.AddWithValue("stock_a56", stock_a56);
        //    return SqlDataBase.fillDataSet(comando);
        //}

        #endregion

        #region PROMOCIONES

        public DataSet PromocionesData(Promocion promocion)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PromocionesData";
            if (promocion.idPromo != null) comando.Parameters.AddWithValue("idPromo", promocion.idPromo);
            if (promocion.Activa != null) comando.Parameters.AddWithValue("Activa", promocion.Activa);
            if (promocion.FechaInicio != null) comando.Parameters.AddWithValue("FechaInicio", promocion.FechaInicio);
            if (promocion.FechaFin != null) comando.Parameters.AddWithValue("FechaFin", promocion.FechaFin);
            if (!string.IsNullOrEmpty(promocion.Titulo)) comando.Parameters.AddWithValue("Titulo", promocion.Titulo);
            if (!string.IsNullOrEmpty(promocion.Descripcion)) comando.Parameters.AddWithValue("Descripcion", promocion.Descripcion);
            return SqlDataBase.fillDataSet(comando);
        }

        public DataSet PromocionesGuardar(Promocion promocion)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PromocionesGuardar";
            comando.Parameters.AddWithValue("idPromo", promocion.idPromo);
            comando.Parameters.AddWithValue("FechaCreacion", promocion.FechaCreacion);
            comando.Parameters.AddWithValue("Titulo", promocion.Titulo);
            comando.Parameters.AddWithValue("Descripcion", promocion.Descripcion);
            comando.Parameters.AddWithValue("URL", promocion.Url);
            comando.Parameters.AddWithValue("FechaInicio", promocion.FechaInicio);
            comando.Parameters.AddWithValue("FechaFin", promocion.FechaFin);
            comando.Parameters.AddWithValue("Activa", promocion.Activa);
            comando.Parameters.AddWithValue("BannerPeq", promocion.BannerPeq);
            comando.Parameters.AddWithValue("BannerGra", promocion.BannerGra);
            comando.Parameters.AddWithValue("TipoBusqueda", GetXMLFromTipoBusqueda(promocion.TipoBusqueda));
            return SqlDataBase.fillDataSet(comando);
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

        public DataSet PromocionesEliminar(int idPromo)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_PromocionesEliminar";
            comando.Parameters.AddWithValue("idPromo", idPromo);
            return SqlDataBase.fillDataSet(comando);
        }
        
        #endregion

    }
}
