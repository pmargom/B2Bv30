using System;
using System.Collections.Generic;
using System.Text;
using Root.Reports;
using System.Drawing;
using System.Reflection;
using B2B.Types;
using System.Data;

namespace PDFObjects
{
    public class FacturaPDF : Report
    {
        public DataSet DatosFacturaPDF { private get; set; }
        public bool EsCopia = false;
        public string Tipo = "FACTURA";

        FontDef defGeneral;
        FontProp propCabeceras;
        FontProp propContacto;
        FontProp propPagina; //PÁGINA: y FACTURA de la cabecera
        FontProp propFactura;
        FontProp propDisclaimer;
        FontProp propDatos;

        PenProp lineaFina;
        PenProp lineaGruesa;
        BrushProp pincelFino;

        FlowLayoutManager flmConcepto;
        StaticContainerMM sc;
        double posicionLinea = 90; //guardamos la altura de la siguiente línea de factura 112-22

        public FacturaPDF()
        {
            //NuevaPagina();
            //InicializarPropiedadesReporte();
            //imagenesCabecera();
            //textoCabecera();
            //areaDatosFactura();
            //areaCuerpoFactura();
            //piedePagina();
            //datosFactura();
            //conceptosFactura();
        }

        private void AccionesPorPagina(DataRow[] filas, bool mostrarTotales)
        {
            NuevaPagina();
            posicionLinea = 90;//112-22
            //InicializarPropiedadesReporte();
            imagenesCabecera();
            //textoCabecera();
            datosDetalleCliente();
            areaDatosFactura();
            imagenMarcaAgua();
            areaCuerpoFactura();
            piedePagina();
            imagenPie();
            datosFactura();
            switch (DatosFacturaPDF.Tables.Count)
            {
                case 3: conceptosFacturaAlbaranes(filas, mostrarTotales);
                    break;
                case 4: conceptosFacturaProductos(filas, mostrarTotales);
                    if (mostrarTotales) gestionNFU();
                    break;
                default:
                    break;
            }
        }

        public void Generar()
        {
            InicializarPropiedadesReporte();

            int N_FILAS_PAGINA = 26; //22
            int nFilasTotales = DatosFacturaPDF.Tables[1].Rows.Count;
            int paginas = Convert.ToInt32(Math.Ceiling(nFilasTotales * 1.0 / N_FILAS_PAGINA * 1.0));
            DataRow[] filas = null;
            int nFilasPintadas = 0;
            int nFilas = 0;
            for (int i = 0; i < paginas - 1; i++)
            {
                filas = new DataRow[N_FILAS_PAGINA];
                nFilas = 0;
                while (nFilas < N_FILAS_PAGINA)
                {
                    filas[nFilas] = DatosFacturaPDF.Tables[1].Rows[nFilasPintadas];
                    nFilas++;
                    nFilasPintadas++;
                }
                AccionesPorPagina(filas, false);
            }
            // en la última página, muestro la info y los totales
            filas = new DataRow[nFilasTotales - nFilasPintadas];
            nFilas = 0;
            while (nFilas < N_FILAS_PAGINA && nFilasPintadas < nFilasTotales)
            {
                filas[nFilas] = DatosFacturaPDF.Tables[1].Rows[nFilasPintadas];
                nFilas++;
                nFilasPintadas++;
            }
            AccionesPorPagina(filas, true);
        }

        protected override void Create()
        {
        }

        internal void NuevaPagina()
        {
            new Root.Reports.Page(this);
        }

        private void InicializarPropiedadesReporte()
        {
            //Estilos de fuente 
            defGeneral = new FontDef(this, FontDef.StandardFont.Helvetica);

            propCabeceras = new FontProp(defGeneral, 5, Color.Black);
            propCabeceras.bBold = true;
            propContacto = new FontProp(defGeneral, 5, Color.Black);
            propPagina = new FontProp(defGeneral, 6, Color.Black);
            propPagina.bBold = true;
            propFactura = new FontProp(defGeneral, 9, Color.Black);
            propFactura.bBold = true;
            propDisclaimer = new FontProp(defGeneral, 2.65, Color.Black);
            propDisclaimer.bBold = true;
            propDatos = new FontProp(defGeneral, 6.5, Color.Black);

            //Estilos de línea
            lineaFina = new PenProp(this, 0.5, Color.Black);
            lineaGruesa = new PenProp(this, 1, Color.Black);
            pincelFino = new BrushProp(this, Color.Black);

        }

        private void datosFactura()
        {
            propDatos.bBold = true;

            #region DEMO

            //Dictionary<string, string> datosCliente = new Dictionary<string, string>();
            //datosCliente.Add("NFactura", "57449");
            //datosCliente.Add("Vendedor", "274");
            //datosCliente.Add("Fecha", "24/11/2010");
            //datosCliente.Add("Cliente", "Cliente de ejemplo");

            //page_Cur.AddMM(9, 72, new RepString(propDatos, datosCliente["NFactura"]));
            //page_Cur.AddMM(33, 72, new RepString(propDatos, datosCliente["Vendedor"]));
            //page_Cur.AddCB_MM(57, 72, new RepString(propDatos, datosCliente["Fecha"]));
            //page_Cur.AddMM(72, 72, new RepString(propDatos, datosCliente["Cliente"]));

            #endregion

            page_Cur.AddMM(9, 50, new RepString(propDatos, DatosFacturaPDF.Tables[0].Rows[0]["VF_FACTURA"].ToString())); //72-22
            page_Cur.AddMM(33, 50, new RepString(propDatos, "B2B"));
            DateTime fechaFactura = DateTime.Parse(DatosFacturaPDF.Tables[0].Rows[0]["VF_FECHA_FACT"].ToString());
            page_Cur.AddCB_MM(56, 50, new RepString(propDatos, string.Format("{0} {1}", fechaFactura.ToShortDateString(), fechaFactura.ToShortTimeString())));
            page_Cur.AddMM(72, 50, new RepString(propDatos, DatosFacturaPDF.Tables[0].Rows[0]["VC_CLIENTE"].ToString()));

        }

        private void datosDetalleCliente()
        {
            Dictionary<string, string> datosDetalleCliente = new Dictionary<string, string>();

            propDatos.bBold = true;

            //datosDetalleCliente.Add("Nombre", "Socomotor Canarias");
            //datosDetalleCliente.Add("Denominacion", "Socomotor Canarias, S. L.");
            //datosDetalleCliente.Add("Direccion", "C/ Miravala nº 13");
            //datosDetalleCliente.Add("Poblacion", "El Socorro");
            //datosDetalleCliente.Add("CodPostal", "38280");
            //datosDetalleCliente.Add("Municipio", "Tegueste");
            //datosDetalleCliente.Add("Provincia", "S/C de Tenerife");
            //datosDetalleCliente.Add("CIF", "B38939955");
            //datosDetalleCliente.Add("Telefono", "922546686");

            datosDetalleCliente.Add("Nombre", DatosFacturaPDF.Tables[0].Rows[0]["VC_DENOMINACION"].ToString());
            datosDetalleCliente.Add("Denominacion", DatosFacturaPDF.Tables[0].Rows[0]["VC_NOMBRE"].ToString());
            string dir=string.Format("{0} {1}", DatosFacturaPDF.Tables[0].Rows[0]["VC_DIRECCION"].ToString(), DatosFacturaPDF.Tables[0].Rows[0]["VC_DIRECCIONB"].ToString());
            datosDetalleCliente.Add("Direccion", dir.Length > 43 ? dir.Substring(0, 43) : dir);
            datosDetalleCliente.Add("Poblacion", DatosFacturaPDF.Tables[0].Rows[0]["VC_POBLACION"].ToString());
            datosDetalleCliente.Add("CodPostal", DatosFacturaPDF.Tables[0].Rows[0]["VC_CODPOSTAL"].ToString());
            datosDetalleCliente.Add("Municipio", DatosFacturaPDF.Tables[0].Rows[0]["VC_POBLACION"].ToString());
            datosDetalleCliente.Add("Provincia", "");
            datosDetalleCliente.Add("CIF", DatosFacturaPDF.Tables[0].Rows[0]["VC_CIF"].ToString());
            datosDetalleCliente.Add("Telefono", DatosFacturaPDF.Tables[0].Rows[0]["VC_TFNO"].ToString());

            insertarDetalleCliente(datosDetalleCliente);
        }

        private void insertarDetalleCliente(Dictionary<string, string> detalleCliente)
        {
            page_Cur.AddMM(121.5, 46.5, new RepString(propDatos, detalleCliente["Denominacion"].ToUpperInvariant())); //recuadro superior 68.5-22

            double Y = 57.5; //posición vertical de la primera línea 79.5-22
            StaticContainerMM sc = new StaticContainerMM(78, 18);
            FlowLayoutManager flmDetalle = new FlowLayoutManager();
            flmDetalle.SetContainer(sc);
            propDatos.bBold = false;

            //string lineaDetalle1 = string.Format("{0}. {1}.", detalleCliente["Direccion"], detalleCliente["Poblacion"]);
            string lineaDetalle1 = string.Format("{0}.", detalleCliente["Direccion"]);
            string lineaDetalle2 = string.Format("{0}, {1}. {2}.", detalleCliente["CodPostal"], detalleCliente["Municipio"], detalleCliente["Provincia"]);
            string lineaDetalle3 = string.Format("CIF: {0}. Teléfono: {1}.", detalleCliente["CIF"], detalleCliente["Telefono"]);

            page_Cur.AddMM(121.5, Y, new RepString(propDatos, detalleCliente["Nombre"]));
            Y += 4;
            page_Cur.AddMM(121.5, Y, new RepString(propDatos, lineaDetalle1));
            Y += 4;
            page_Cur.AddMM(121.5, Y, new RepString(propDatos, lineaDetalle2));
            Y += 4;
            page_Cur.AddMM(121.5, Y, new RepString(propDatos, lineaDetalle3));
        }

        private void conceptosFacturaProductos(DataRow[] filas, bool mostrarTotales)
        {
            propDatos.bBold = false;
            flmConcepto = new FlowLayoutManager(null);

            #region DEMO
            //Dictionary<string, string> listaConceptos = new Dictionary<string, string>();
            //listaConceptos.Add("Codigo", "145R10");
            //listaConceptos.Add("Concepto", "Neumático DUNLOP 145R10 68S SP9 TL para verano, de 20'' de diámetro ");
            //listaConceptos.Add("CAT", "CT");
            //listaConceptos.Add("Cantidad", "4");
            //listaConceptos.Add("PrUnitario", "27.49");
            //listaConceptos.Add("Dto", "-");
            //listaConceptos.Add("IGIC", "5.00 %");
            //listaConceptos.Add("Importe", "109.96");      

            //posicionLinea += insertarLineas(listaConceptos);

            //Dictionary<string, string> listaConceptos2 = new Dictionary<string, string>();
            //listaConceptos = new Dictionary<string, string>();
            //listaConceptos.Add("Codigo", "R921CH");
            //listaConceptos.Add("Concepto", "Llanta R921 8,5X20 E15 6X139,7 108 CH, disponible en dos colores diferentes");
            //listaConceptos.Add("CAT", "CT");
            //listaConceptos.Add("Cantidad", "1");
            //listaConceptos.Add("PrUnitario", "450.73");
            //listaConceptos.Add("Dto", "-");
            //listaConceptos.Add("IGIC", "5.00 %");
            //listaConceptos.Add("Importe", "450.73");

            //posicionLinea += insertarLineas(listaConceptos);
            #endregion

            List<Dictionary<string, string>> conceptos = new List<Dictionary<string, string>>();

            bool primero = true;
            foreach (DataRow fila in filas)
            {
                Dictionary<string, string> concepto = new Dictionary<string, string>();
                if (primero)
                {
                    concepto.Add("Codigo", "");
                    concepto.Add("Concepto", fila["VF_DESCARTICULO"].ToString());
                    concepto.Add("CAT", "");
                    concepto.Add("Cantidad", "");
                    concepto.Add("PrUnitario", "");
                    concepto.Add("Dto", "");
                    concepto.Add("IGIC", "");
                    concepto.Add("Importe", "");
                    primero = false;
                }
                else
                {
                    concepto.Add("Codigo", fila["VF_ARTICULO"].ToString());
                    string val = fila["VF_DESCARTICULO"].ToString();
                    concepto.Add("Concepto", string.Format("{0}", val.Length > 35 ? val.Substring(0, 35) : val));
                    concepto.Add("CAT", fila["VF_CATEGORIA"].ToString());
                    concepto.Add("Cantidad", decimal.Round(decimal.Parse(fila["VF_UNIDADES"].ToString()),3).ToString("N2"));
                    concepto.Add("PrUnitario", decimal.Round(decimal.Parse(fila["VF_PVENTA"].ToString()), 3).ToString("N2"));
                    concepto.Add("Dto", decimal.Round(decimal.Parse(fila["VF_PORC_DCTO"].ToString()),3).ToString("N2"));
                    concepto.Add("IGIC", decimal.Round(decimal.Parse(fila["VF_PORC_IGIC"].ToString()),3).ToString("N2"));
                    concepto.Add("Importe", decimal.Round(decimal.Parse(fila["Importe"].ToString()),3).ToString("N2"));
                }
                conceptos.Add(concepto);
            }

            conceptos.ForEach(p =>
            {
                posicionLinea += insertarLineas(p);
            });

            if (mostrarTotales)
            {
                page_Cur.AddCB_MM(127, 252, new RepString(propDatos, DatosFacturaPDF.Tables[0].Rows[0]["VC_CODFORMAPAGO"].ToString()));

                DataTable dt = DatosFacturaPDF.Tables[DatosFacturaPDF.Tables.Count - 1];
                decimal total = 0;
                foreach (DataRow fila in dt.Rows) total += decimal.Parse(fila["VF_NETO"].ToString());
                page_Cur.AddRightMM(200, 252, new RepString(propFactura, string.Format("{0} €", decimal.Round(total, 3).ToString("N2"))));

                double posY = 252;

                foreach (DataRow fila in DatosFacturaPDF.Tables[3].Rows)
                {
                    page_Cur.AddRightMM(45, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["BASE_IMP"].ToString()), 3).ToString("N2")));
                    page_Cur.AddRightMM(70, posY, new RepString(propDatos, string.Format("{0} %", decimal.Round(decimal.Parse(fila["VF_PORC_IGIC"].ToString()), 3).ToString("N2"))));
                    page_Cur.AddRightMM(93, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["IGIC"].ToString()),3).ToString("N2")));
                    posY += 4;
                }
            }
        }

        private void conceptosFacturaAlbaranes(DataRow[] filas, bool mostrarTotales)
        {
            propDatos.bBold = false;
            flmConcepto = new FlowLayoutManager(null);

            #region DEMO
            //Dictionary<string, string> listaConceptos = new Dictionary<string, string>();
            //listaConceptos.Add("Codigo", "145R10");
            //listaConceptos.Add("Concepto", "Neumático DUNLOP 145R10 68S SP9 TL para verano, de 20'' de diámetro ");
            //listaConceptos.Add("CAT", "CT");
            //listaConceptos.Add("Cantidad", "4");
            //listaConceptos.Add("PrUnitario", "27.49");
            //listaConceptos.Add("Dto", "-");
            //listaConceptos.Add("IGIC", "5.00 %");
            //listaConceptos.Add("Importe", "109.96");      

            //posicionLinea += insertarLineas(listaConceptos);

            //Dictionary<string, string> listaConceptos2 = new Dictionary<string, string>();
            //listaConceptos = new Dictionary<string, string>();
            //listaConceptos.Add("Codigo", "R921CH");
            //listaConceptos.Add("Concepto", "Llanta R921 8,5X20 E15 6X139,7 108 CH, disponible en dos colores diferentes");
            //listaConceptos.Add("CAT", "CT");
            //listaConceptos.Add("Cantidad", "1");
            //listaConceptos.Add("PrUnitario", "450.73");
            //listaConceptos.Add("Dto", "-");
            //listaConceptos.Add("IGIC", "5.00 %");
            //listaConceptos.Add("Importe", "450.73");

            //posicionLinea += insertarLineas(listaConceptos);
            #endregion

            List<Dictionary<string, string>> conceptos = new List<Dictionary<string, string>>();

            foreach (DataRow fila in filas)
            {
                Dictionary<string, string> concepto = new Dictionary<string, string>();
                concepto.Add("Codigo", "");
                concepto.Add("Concepto", string.Format("Albarán N.- {0} fec : {1}",
                                                        fila["VF_ALBARAN"].ToString(),
                                                        string.IsNullOrEmpty(fila["VF_FECHA_ALB"].ToString()) ? "" : DateTime.Parse(fila["VF_FECHA_ALB"].ToString()).ToShortDateString()));
                concepto.Add("CAT", "");
                concepto.Add("Cantidad", "");
                concepto.Add("PrUnitario", "");
                concepto.Add("Dto", "");
                concepto.Add("IGIC", "");
                concepto.Add("Importe", decimal.Round(decimal.Parse(fila["VF_NETO"].ToString()), 3).ToString("N2"));
                conceptos.Add(concepto);
            }

            conceptos.ForEach(p =>
            {
                posicionLinea += insertarLineas(p);
            });

            if (mostrarTotales)
            {
                page_Cur.AddCB_MM(127, 252, new RepString(propDatos, DatosFacturaPDF.Tables[0].Rows[0]["VC_CODFORMAPAGO"].ToString()));

                DataTable dt = DatosFacturaPDF.Tables[DatosFacturaPDF.Tables.Count - 1];
                decimal total = 0;
                foreach (DataRow fila in dt.Rows) total += decimal.Parse(fila["VF_NETO"].ToString());
                page_Cur.AddRightMM(200, 252, new RepString(propFactura, string.Format("{0} €", decimal.Round(total, 3).ToString("N2"))));

                double posY = 252;
                foreach (DataRow fila in DatosFacturaPDF.Tables[2].Rows)
                {
                    page_Cur.AddRightMM(45, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["BASE_IMP"].ToString()),3).ToString("N2")));
                    page_Cur.AddRightMM(70, posY, new RepString(propDatos, string.Format("{0} %", decimal.Round(decimal.Parse(fila["VF_PORC_IGIC"].ToString()),3).ToString("N2"))));
                    page_Cur.AddRightMM(93, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["IGIC"].ToString()),3).ToString("N2")));
                    posY += 4;
                }
            }
        }

        private void gestionNFU()
        {
            #region
            //Dictionary<string, string> gestionNFU = new Dictionary<string, string>();

            //gestionNFU.Add("Texto", "S. I. Gestión de NFU (RD. 1619/2005) categoría CT");
            //gestionNFU.Add("Unidades", "-2");
            //gestionNFU.Add("Precio", "2.620");
            //gestionNFU.Add("IGIC", "2%");
            //gestionNFU.Add("Importe", "-5.24");
            #endregion

            propDatos.bBold = false;

            int posY = 225;

            foreach (DataRow fila in DatosFacturaPDF.Tables[2].Rows)
            {
                page_Cur.AddMM(9, posY, new RepString(propDatos, fila["VF_DESCARTICULO"].ToString()));
                page_Cur.AddRightMM(127, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["VF_UNIDADES"].ToString()), 3).ToString("N2")));
                page_Cur.AddRightMM(153, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["VF_PVENTA"].ToString()), 3).ToString("N2")));
                page_Cur.AddCB_MM(169, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["VF_PORC_IGIC"].ToString()),3).ToString("N2")));
                page_Cur.AddRightMM(200, posY, new RepString(propDatos, decimal.Round(decimal.Parse(fila["Importe"].ToString()),3).ToString("N2")));
                posY += 5;
            }
        }

        private double insertarLineas(Dictionary<string, string> listaConceptos)
        {
            double Y = posicionLinea; //posición vertical de la primera línea
            sc = new StaticContainerMM(67, 7);
            flmConcepto.SetContainer(sc);
            flmConcepto.Add(new RepString(propDatos, listaConceptos["Concepto"]));

            page_Cur.AddRightMM(41, Y, new RepString(propDatos, listaConceptos["Codigo"]));
            page_Cur.AddMM(45, Y, sc);
            page_Cur.AddRightMM(118, Y, new RepString(propDatos, listaConceptos["CAT"]));
            page_Cur.AddRightMM(127, Y, new RepString(propDatos, listaConceptos["Cantidad"]));
            page_Cur.AddRightMM(153, Y, new RepString(propDatos, listaConceptos["PrUnitario"]));
            page_Cur.AddCB_MM(160, Y, new RepString(propDatos, listaConceptos["Dto"]));
            page_Cur.AddCB_MM(169, Y, new RepString(propDatos, listaConceptos["IGIC"]));
            page_Cur.AddRightMM(200, Y, new RepString(propDatos, listaConceptos["Importe"]));

            Y = flmConcepto.rY_CurMM;

            //flmConcepto.NewLineMM(propDatos.rLineFeedMM + 0.9);
            flmConcepto.NewLineMM(propDatos.rLineFeedMM + 0.4);
            Y = flmConcepto.rY_CurMM;
            return Y;
        }

        #region plantilla

        private void imagenesCabecera()
        {
            //obtenemos la imagen
            string assemblyName = GetType().Assembly.GetName().Name;
            System.IO.Stream cabecera = GetType().Assembly.GetManifestResourceStream(string.Format("{0}.images.NA_cabecera.jpg", assemblyName));
            //colocamos la imagen          
            page_Cur.AddMM(5.55, 36.05, new RepImageMM(cabecera, 199.03, Double.NaN));
        }

        private void imagenPie()
        {
            //obtenemos la imagen
            string assemblyName = GetType().Assembly.GetName().Name;
            System.IO.Stream pie = GetType().Assembly.GetManifestResourceStream(string.Format("{0}.images.NA_pie.jpg", assemblyName));
            //colocamos la imagen          
            page_Cur.AddMM(5.20, 289.85, new RepImageMM(pie, 199.63, Double.NaN));
        }

        private void imagenMarcaAgua()
        {
            //obtenemos la imagen
            string assemblyName = GetType().Assembly.GetName().Name;
            System.IO.Stream marcaagua = GetType().Assembly.GetManifestResourceStream(string.Format("{0}.images.NA_marcaagua.jpg", assemblyName));
            //colocamos la imagen          
            page_Cur.AddMM(7.00, 218.40, new RepImageMM(marcaagua, 195.00, Double.NaN));
        }

        private void areaDatosFactura()
        {
            //número, vendedor, fecha, cliente
            RectanguloRedondeado(7, 53, 100, 13, 2, lineaGruesa); //75-22
            //RepRectMM Rec1 = new RepRectMM(lineaGruesa, 100, 13); 
            //page_Cur.AddMM(7, 75, Rec1);           

            page_Cur.AddMM(7, 45, new RepLineMM(lineaGruesa, 100, 0)); //líneas interiores 67-22
            page_Cur.AddMM(31, 53, new RepLineMM(lineaFina, 0, 13));//75-22
            page_Cur.AddMM(43, 53, new RepLineMM(lineaFina, 0, 13));
            page_Cur.AddMM(70, 53, new RepLineMM(lineaFina, 0, 13));

            page_Cur.AddLT_MM(9, 42, new RepString(propCabeceras, "Nº")); //texto cabeceras 64-22
            page_Cur.AddLT_MM(33, 42, new RepString(propCabeceras, "VEND."));
            page_Cur.AddCT_MM(57, 42, new RepString(propCabeceras, "FECHA"));
            page_Cur.AddLT_MM(72, 42, new RepString(propCabeceras, "CLIENTE"));

            //firma transportista, hora entrega, VºBº almacén
            RectanguloRedondeado(7, 73, 100, 13, 2, lineaGruesa); //95-22
            //RepRectMM Rec2 = new RepRectMM(lineaGruesa, 100, 13); //contornos
            //page_Cur.AddMM(7, 95, Rec2);

            page_Cur.AddMM(7, 65, new RepLineMM(lineaGruesa, 100, 0)); //líneas interiores 87-22
            page_Cur.AddMM(43, 73, new RepLineMM(lineaFina, 0, 13));//95-22
            page_Cur.AddMM(68, 73, new RepLineMM(lineaFina, 0, 13));

            page_Cur.AddMM(49, 71, new RepLineMM(lineaGruesa, 6, 0)); //hueco para escribir la hora 93-22
            page_Cur.AddMM(57, 71, new RepLineMM(lineaGruesa, 6, 0));
            page_Cur.AddMM(56, 68, new RepLineMM(lineaGruesa, 0, 0.5));//90-22
            page_Cur.AddMM(56, 70, new RepLineMM(lineaGruesa, 0, 0.5));//92-22

            page_Cur.AddLT_MM(9, 62, new RepString(propCabeceras, "FIRMA TRANSPORTISTA")); //texto cabeceras 84-22
            page_Cur.AddLT_MM(45, 62, new RepString(propCabeceras, "HORA ENTREGA"));
            page_Cur.AddCT_MM(86, 62, new RepString(propCabeceras, "Vº Bº ALMACÉN"));

            page_Cur.AddLT_MM(7, 55, new RepString(propPagina, "PÁGINA:")); //página y factura 77-22
            //page_Cur.AddLT_MM(46, 97, new RepString(propFactura, "FACTURA"));
            page_Cur.AddLT_MM(46, 75, new RepString(propFactura, string.Format("{0}          {1}", EsCopia ? "COPIA" : "", Tipo))); //97-22


            //rectángulos derecha
            RectanguloRedondeado(119, 50, 83, 10, 2, lineaGruesa); //72-22
            //RepRectMM Rec3 = new RepRectMM(lineaGruesa, 83, 10);
            //page_Cur.AddMM(119, 72, Rec3);

            RectanguloRedondeado(119, 73, 83, 21, 2, lineaGruesa); //95-22
            //RepRectMM Rec4 = new RepRectMM(lineaGruesa, 83, 21);
            //page_Cur.AddMM(119, 95, Rec4);
        }

        private void areaCuerpoFactura()
        {
            RectanguloRedondeado(7, 262, 195, 182, 2, lineaGruesa);//160+22

            page_Cur.AddMM(7, 85, new RepLineMM(lineaGruesa, 195, 0)); //líneas interiores horizontales 107-22
            page_Cur.AddMM(7, 243, new RepLineMM(lineaGruesa, 195, 0));
            page_Cur.AddMM(7, 248, new RepLineMM(lineaGruesa, 195, 0));
            page_Cur.AddMM(7, 220, new RepLineMM(lineaFina, 195, 0));

            page_Cur.AddMM(43, 220, new RepLineMM(lineaFina, 0, 140)); //líneas interiores verticales 118+22
            page_Cur.AddMM(111.5, 220, new RepLineMM(lineaFina, 0, 140));
            page_Cur.AddMM(120.5, 243, new RepLineMM(lineaFina, 0, 163)); //141+22
            page_Cur.AddMM(129, 243, new RepLineMM(lineaFina, 0, 163));
            page_Cur.AddMM(155, 262, new RepLineMM(lineaFina, 0, 182)); //160+22
            page_Cur.AddMM(164, 243, new RepLineMM(lineaFina, 0, 163)); //141+22
            page_Cur.AddMM(174, 243, new RepLineMM(lineaFina, 0, 163));

            page_Cur.AddMM(47, 262, new RepLineMM(lineaFina, 0, 19));
            page_Cur.AddMM(72, 262, new RepLineMM(lineaFina, 0, 19));
            page_Cur.AddMM(95, 262, new RepLineMM(lineaFina, 0, 19));

            page_Cur.AddLT_MM(20, 82, new RepString(propCabeceras, "CÓDIGO")); //texto cabeceras 104-22
            page_Cur.AddCT_MM(79, 82, new RepString(propCabeceras, "CONCEPTO"));
            page_Cur.AddCT_MM(116, 82, new RepString(propCabeceras, "CAT."));
            page_Cur.AddRT_MM(128, 82, new RepString(propCabeceras, "UDS."));
            page_Cur.AddCT_MM(142, 82, new RepString(propCabeceras, "PRECIO"));
            page_Cur.AddCT_MM(160, 82, new RepString(propCabeceras, "DTO."));
            page_Cur.AddCT_MM(169, 82, new RepString(propCabeceras, "IGIC"));
            page_Cur.AddCT_MM(188, 82, new RepString(propCabeceras, "IMPORTE"));

            page_Cur.AddCT_MM(28, 244.5, new RepString(propCabeceras, "BASE IMPONIBLE")); //texto cabeceras en la base
            page_Cur.AddCT_MM(59, 244.5, new RepString(propCabeceras, "% IGIC"));
            page_Cur.AddCT_MM(84, 244.5, new RepString(propCabeceras, "IGIC"));
            page_Cur.AddCT_MM(127, 244.5, new RepString(propCabeceras, "FORMA DE PAGO"));
            page_Cur.AddCT_MM(179, 244.5, new RepString(propCabeceras, "TOTAL"));
        }

        private void piedePagina()
        {
            //disclaimer
            FlowLayoutManager flmDisclaimer = new FlowLayoutManager(null);
            StaticContainer sc = new StaticContainerMM(195, 16);
            flmDisclaimer.SetContainer(sc);

            StringBuilder disclaimer = new StringBuilder();
            disclaimer.AppendLine("En cumplimiento de la normativa actual en materia de protección de datos de carácter personal. Ley Orgánica 15/1999 de diciembre, la empresa NEUMÁTICOS ATLÁNTICO S.L. pone en su conocimiento que los datos aportados por usted en la presente FACTURA serán incorporados a un fichero automatizado");
            flmDisclaimer.Add(new RepString(propDisclaimer, disclaimer.ToString()));
            flmDisclaimer.NewLine(0.2);
            disclaimer.Remove(0, disclaimer.Length - 1);
            disclaimer.AppendLine("propiedad de la empresa y debidamente inscrito en la Agencia Española de Protección de Datos, denominado GESTIÓN COMERCIAL. En cumplimiento del articulo 5 de la citada Ley le informamos que los datos aportados serán utilizados para la confección de la presente FACTURA. De la misma manera,");
            flmDisclaimer.Add(new RepString(propDisclaimer, disclaimer.ToString()));
            flmDisclaimer.NewLine(0.2);
            disclaimer.Remove(0, disclaimer.Length - 1);
            disclaimer.AppendLine("también podrán ser utilizados para comunicarles en el futuro oferta de productos o servicios que nuestra empresa preste o comercialice directa o indirectamente a través de otras empresas del grupo o de colaboradores. La recogida de datos, además se ha realizado con la finalidad de poder atender y procesar");
            flmDisclaimer.Add(new RepString(propDisclaimer, disclaimer.ToString()));
            flmDisclaimer.NewLine(0.2);
            disclaimer.Remove(0, disclaimer.Length - 1);
            disclaimer.AppendLine("adecuadamente las solicitudes de consulta remitidas para la ampliación y mejora de los servicios, el diseño de nuevos servicios relacionados con dichos servicios, el envío por medios tradicionales y electrónicos de información técnica, operativa y comercial de productos y servicios ofrecidos por NEUMÁTICOS");
            flmDisclaimer.Add(new RepString(propDisclaimer, disclaimer.ToString()));
            flmDisclaimer.NewLine(0.2);
            disclaimer.Remove(0, disclaimer.Length - 1);
            disclaimer.AppendLine("ATLÁNTICO S.L. actualmente y en el futuro para lo cual nos brinda al marcar la casilla abajo habilitada su consentimiento expreso. La finalidad de la recogida y tratamiento automatizado de los datos puede incluir igualmente el envío de formularios de encuestas, que Usted no queda obligado a contestar.");
            flmDisclaimer.Add(new RepString(propDisclaimer, disclaimer.ToString()));

            disclaimer.Remove(0, disclaimer.Length - 1);
            disclaimer.AppendLine(" ");
            disclaimer.AppendLine("Sí, deseo recibir información");
            flmDisclaimer.Add(new RepString(propDisclaimer, disclaimer.ToString()));

            disclaimer.Remove(0, disclaimer.Length - 1);
            disclaimer.AppendLine(" ");
            disclaimer.AppendLine("Usted como interesado directo tiene derecho de acceso, rectificación, cancelación y oposición. Para hacer uso del citado derecho deberá dirigirse al Responsable de Protección de Datos de NEUMÁTICOS ATLÁNTICO S.L. en la dirección C/ Gestur sin número. Poligono Industrial La Campana, código postal 38109 del municipio de El Rosario (Tenerife), aportando fotocopia de su DNI o documento identificativo.");
            flmDisclaimer.Add(new RepString(propDisclaimer, disclaimer.ToString()));

            page_Cur.AddMM(7, 265, sc);
            RepRectMM recibirInformacion = new RepRectMM(lineaFina, 1.6, 1.6);
            page_Cur.AddMM(26, 276.5, recibirInformacion);

            //texto lateral
            propCabeceras.rAngle = 270;
            propCabeceras.bBold = false;
            page_Cur.AddMM(5.80, 257.5, new RepString(propCabeceras, "Neumáticos Atlántico S.L. - C.I.F.:B-38342424 Inscrita en el Registro Mercantil de Santa Cruz de Tenerife, Tomo 1081, Folio 115, hoja TF-8636, inscripción 1a."));//225.30
            propCabeceras.bBold = true;
            propCabeceras.rAngle = 0;

        }

        private void RectanguloRedondeado(double posX, double posY, double dimX, double dimY, double r, PenProp grosor)
        {
            //posX, posY: posición del rectángulo, origen, esquina inferior izquierda.
            //dimX, dimY: dimensiones del rectángulo
            //r: radio de las esquinas

            RepLineMM LH1 = new RepLineMM(grosor, dimX - 2 * r, 0);
            RepLineMM LH2 = new RepLineMM(grosor, dimX - 2 * r, 0);
            RepLineMM LV1 = new RepLineMM(grosor, 0, dimY - 2 * r);
            RepLineMM LV2 = new RepLineMM(grosor, 0, dimY - 2 * r);

            page_Cur.AddMM(posX + r, posY - dimY, LH1);
            page_Cur.AddMM(posX + r, posY, LH2);
            page_Cur.AddMM(posX, posY - r, LV1);
            page_Cur.AddMM(posX + dimX, posY - r, LV2);

            //grosor.rWidth *= 1.1;

            RepArcMM Arc1 = new RepArcMM(grosor, r, 90, 90);
            RepArcMM Arc2 = new RepArcMM(grosor, r, 180, 90);
            RepArcMM Arc3 = new RepArcMM(grosor, r, 270, 90);
            RepArcMM Arc4 = new RepArcMM(grosor, r, 0, 90);

            page_Cur.AddMM(posX, posY, Arc1);
            page_Cur.AddMM(posX, posY - dimY + 2 * r, Arc2);
            page_Cur.AddMM(posX + dimX - 2 * r, posY - dimY + 2 * r, Arc3);
            page_Cur.AddMM(posX + dimX - 2 * r, posY, Arc4);
        }

        #endregion

    }
}

