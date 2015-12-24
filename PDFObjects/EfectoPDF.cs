using System;
using System.Collections.Generic;
using System.Text;
using Root.Reports;
using System.Drawing;
using System.Reflection;
using System.Data;
using B2B.Generic;

namespace PDFObjects
{
    public class EfectoPDF: Report
    {
        public DataSet DatosEfectoPDF { private get; set; }
        public bool EsCopia = false;
        public string Tipo = "EFECTO";

        FontDef defGeneral;
        FontProp propCabeceras;
        FontProp propContacto;
        FontProp propDatos;

        PenProp lineaFina;
        PenProp lineaGruesa;
        BrushProp pincelFino;    

        public EfectoPDF() 
        {
            //NuevaPagina();
            //InicializarPropiedadesReporte();
            //logotipo();
            //rectangulos();
            //textosFijos();
            //datosRecibo();
            //cantidadYConcepto();
            //datosEntidad();
            //datosCCC();
            //datosLibrado();
        }

        public void Generar()
        {
            NuevaPagina();
            InicializarPropiedadesReporte();
            logotipo();
            rectangulos();
            textosFijos();
            datosRecibo();
            cantidadYConcepto();
            datosEntidad();
            datosCCC();
            datosLibrado();
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
            propDatos = new FontProp(defGeneral, 6.5, Color.Black);
                                   
            //Estilos de línea
            lineaFina = new PenProp(this, 0.5, Color.Black);
            lineaGruesa = new PenProp(this, 1, Color.Black);
            pincelFino = new BrushProp(this, Color.Black);                    
        }

        private void datosRecibo()
        {
            Dictionary<string, string> datosRecibo = new Dictionary<string, string>();

            #region DEMO
            //datosRecibo.Add("NRecibo", "582");
            //datosRecibo.Add("LugarLib", "EL CHORRILLO");
            //datosRecibo.Add("Importe", "10,50");
            //datosRecibo.Add("FechaLib", "06/06/2011");
            //datosRecibo.Add("Vencimiento", "21/05/2011");
            #endregion

            DataRow fila = DatosEfectoPDF.Tables[0].Rows[0];

            datosRecibo.Add("NRecibo", fila["VE_DOCUMENTO"].ToString());
            datosRecibo.Add("LugarLib", "EL CHORRILLO");
            datosRecibo.Add("Importe", decimal.Round(decimal.Parse(fila["VE_IMPORTE"].ToString()), 3).ToString("N2"));
            datosRecibo.Add("FechaLib", DateTime.Parse(fila["VE_EMISION"].ToString()).ToShortDateString());
            datosRecibo.Add("Vencimiento", DateTime.Parse(fila["VE_VENCIMIENTO"].ToString()).ToShortDateString());

            page_Cur.AddMM(24, 9.5, new RepString(propDatos, datosRecibo["NRecibo"]));
            page_Cur.AddMM(87, 9.5, new RepString(propDatos, datosRecibo["LugarLib"]));
            propDatos.bBold = true;
            page_Cur.AddRightMM(202, 9.5, new RepString(propDatos, string.Format("{0} €", datosRecibo["Importe"])));
            propDatos.bBold = false;
            page_Cur.AddMM(42, 18, new RepString(propDatos, datosRecibo["FechaLib"]));
            page_Cur.AddMM(125, 18, new RepString(propDatos, datosRecibo["Vencimiento"]));            
        }

        private void cantidadYConcepto()
        {
            Dictionary<string, string> cantidadYConcepto = new Dictionary<string, string>();

            #region DEMO
            //cantidadYConcepto.Add("Cantidad", "diez euros con cincuenta céntimos");
            //cantidadYConcepto.Add("Concepto", "Recibo (582) pago factura FV00000170 de fecha 25/02/2011");
            #endregion

            DataRow fila = DatosEfectoPDF.Tables[0].Rows[0];
            string concepto = string.Format("Recibo {0} pago factura {1} de fecha {2}",
                                            fila["VE_DOCUMENTO"].ToString(),
                                            fila["VE_FACTURA"].ToString(),
                                            DateTime.Parse( fila["VE_FECHA_FACT"].ToString()).ToShortDateString());

            cantidadYConcepto.Add("Cantidad", decimal.Round(decimal.Parse(fila["VE_IMPORTE"].ToString()), 3).ToString("N2"));
            cantidadYConcepto.Add("Concepto", concepto);

            propDatos.bItalic = true;
            //page_Cur.AddMM(31, 31, new RepString(propDatos, cantidadYConcepto["Cantidad"]));
            Numalet let = new Numalet();
            //un porcentaje como he visto en algunos documentos de caracter legal:
            //let.MascaraSalidaDecimal = "por ciento";
            let.SeparadorDecimalSalida = "con";
            let.ConvertirDecimales = true;
            page_Cur.AddMM(31, 31, new RepString(propDatos, let.ToCustomCardinal(cantidadYConcepto["Cantidad"].Replace(".",""))));
            propDatos.bItalic = false;
            propDatos.bBold = true;
            page_Cur.AddMM(29, 44, new RepString(propDatos, cantidadYConcepto["Concepto"]));
            propDatos.bBold = false;
        }

        private void datosEntidad()
        {
            Dictionary<string, string> datosEntidad = new Dictionary<string, string>();

            #region DEMO
            //datosEntidad.Add("Persona", "Ejemplo persona o entidad");
            //datosEntidad.Add("Direccion", "C/ De ejemplo nº 17");
            //datosEntidad.Add("CodPostal", "38201");
            //datosEntidad.Add("Municipio", "San Cristóbal de La Laguna");
            //datosEntidad.Add("Provincia", "Santa Cruz de Tenerife");
            #endregion

            DataRow fila = DatosEfectoPDF.Tables[0].Rows[0];

            datosEntidad.Add("Persona", fila["VC_BANCO"].ToString());
            datosEntidad.Add("Direccion", fila["VC_BANCO_DIRECCION"].ToString());
            datosEntidad.Add("CodPostal", "");
            datosEntidad.Add("Municipio", fila["VC_BANCO_POBLACION"].ToString());
            datosEntidad.Add("Provincia", "");


            page_Cur.AddMM(12, 57.5, new RepString(propDatos, datosEntidad["Persona"]));
            page_Cur.AddMM(12, 66, new RepString(propDatos, string.Format("{0} - {1}", datosEntidad["Direccion"], datosEntidad["Municipio"])));
            //page_Cur.AddMM(12, 70, new RepString(propDatos, string.Format("{0} - {1}", datosEntidad["CodPostal"], datosEntidad["Provincia"])));
        }

        private void datosCCC()
        {
            Dictionary<string, string> datosCCC = new Dictionary<string, string>();

            #region DEMO
            //datosCCC.Add("Entidad", "2065");
            //datosCCC.Add("Oficina", "0704");
            //datosCCC.Add("DC", "46");
            //datosCCC.Add("CCC", "3000007242");
            #endregion

            DataRow fila = DatosEfectoPDF.Tables[0].Rows[0];

            string ccc = fila["VC_BANCO_CUENTA"].ToString().Replace(" ", "");

            string entidad = "", sucursal = "", dc = "", cuenta = "";

            if (!string.IsNullOrEmpty(ccc))
            {
                entidad = ccc.Substring(0, 4);
                sucursal = ccc.Substring(4, 4);
                dc = ccc.Substring(8, 2);
                cuenta = ccc.Substring(10, 10);
            }

            datosCCC.Add("Entidad", entidad);
            datosCCC.Add("Oficina", sucursal);
            datosCCC.Add("DC", dc);
            datosCCC.Add("CCC", cuenta);

            page_Cur.AddMM(159, 57.5, new RepString(propDatos, datosCCC["Entidad"]));
            page_Cur.AddMM(159, 62.5, new RepString(propDatos, datosCCC["Oficina"]));
            page_Cur.AddCT_MM(199, 60, new RepString(propDatos, datosCCC["DC"]));
            page_Cur.AddMM(159, 70, new RepString(propDatos, datosCCC["CCC"]));
        }

        private void datosLibrado()
        {
            Dictionary<string, string> datosLibrado = new Dictionary<string, string>();

            #region DEMO
            //datosLibrado.Add("Nombre", "FINANZAUTO, S. L.");
            //datosLibrado.Add("Direccion", "Autopista Santa Cruz - La Laguna");
            //datosLibrado.Add("Municipio", "Santa Cruz de Tenerife");
            //datosLibrado.Add("CP", "38201");
            //datosLibrado.Add("Provincia", "Santa Cruz de Tenerife");
            #endregion

            DataRow fila = DatosEfectoPDF.Tables[0].Rows[0];

            //datosLibrado.Add("Nombre", fila["VC_DENOMINACION"].ToString());
            datosLibrado.Add("Nombre", fila["VC_NOMBRE"].ToString());
            datosLibrado.Add("Direccion", fila["VC_DIRECCION"].ToString());
            datosLibrado.Add("DireccionB", fila["VC_DIRECCIONB"].ToString());
            datosLibrado.Add("CP", fila["VC_CODPOSTAL"].ToString());
            datosLibrado.Add("Municipio", fila["VC_POBLACION"].ToString());
            datosLibrado.Add("Provincia", "");

            propDatos.bBold = true;
            page_Cur.AddMM(10, 85, new RepString(propDatos, datosLibrado["Nombre"]));
            propDatos.bBold = false;
            page_Cur.AddMM(10, 90, new RepString(propDatos, datosLibrado["Direccion"]));
            page_Cur.AddMM(10, 95, new RepString(propDatos, datosLibrado["DireccionB"]));
            page_Cur.AddMM(10, 100, new RepString(propDatos, string.Format("{0} - {1}", datosLibrado["CP"], datosLibrado["Municipio"])));
        }

        #region plantilla

        private void logotipo()
        {
            //obtenemos la imagen
            string assemblyName = GetType().Assembly.GetName().Name;
            System.IO.Stream logo = GetType().Assembly.GetManifestResourceStream(string.Format("{0}.images.NeumaticosAtlantico.jpg", assemblyName));            

            //colocamos la imagen
            page_Cur.AddMM(140, 104, new RepImageMM(logo, 60, Double.NaN));
        }

        private void rectangulos()
        {
            //datos recibo
            //RectanguloRedondeado(7, 22, 198, 17, 2, lineaGruesa);
            RepRectMM recibo = new RepRectMM(lineaGruesa, 198, 17);
            page_Cur.AddMM(7, 22, recibo);
            page_Cur.AddMM(7, 14, new RepLineMM(lineaFina, 198, 0));
            page_Cur.AddMM(52, 14, new RepLineMM(lineaFina, 0, 9));
            page_Cur.AddMM(152, 14, new RepLineMM(lineaFina, 0, 9));
            page_Cur.AddMM(103, 22, new RepLineMM(lineaFina, 0, 8));

            //datos entidad
            //RectanguloRedondeado(7, 74, 198, 24, 2, lineaGruesa);
            RepRectMM entidad = new RepRectMM(lineaGruesa, 198, 24);
            page_Cur.AddMM(7, 74, entidad);

            //datos CCC
            //RectanguloRedondeado(156, 74, 49, 34, 2, lineaGruesa);
            //RectanguloRedondeado(192, 64, 13, 14, 2, lineaGruesa);
            RepRectMM CCC = new RepRectMM(lineaGruesa, 49, 34);
            page_Cur.AddMM(156, 74, CCC);
            RepRectMM DC = new RepRectMM(lineaGruesa, 13, 14);
            page_Cur.AddMM(192, 64, DC);

            //datos librado
            //RectanguloRedondeado(7, 104, 128, 28, 2, lineaGruesa);
            RepRectMM librado = new RepRectMM(lineaGruesa, 128, 28);
            page_Cur.AddMM(7, 104, librado);
            page_Cur.AddMM(152, 14, new RepLineMM(lineaFina, 0, 9));
            page_Cur.AddMM(78, 104, new RepLineMM(lineaFina, 0, 28));
        }

        private void textosFijos()
        {
            //datos recibo
            page_Cur.AddMM(10, 9, new RepString(propCabeceras, "RECIBO Nº"));
            page_Cur.AddMM(55, 9, new RepString(propCabeceras, "LUGAR DE LIBRAMIENTO"));
            page_Cur.AddMM(154, 9, new RepString(propCabeceras, "IMPORTE"));
            page_Cur.AddMM(10, 17.5, new RepString(propCabeceras, "FECHA DE LIBRAMIENTO"));
            page_Cur.AddMM(106, 17.5, new RepString(propCabeceras, "VENCIMIENTO"));

            //cantidad
            page_Cur.AddMM(10, 26, new RepString(propDatos, "Por esta LETRA DE CAMBIO pagará usted al vencimiento expresado"));
            page_Cur.AddMM(10, 31, new RepString(propDatos, "la cantidad de"));

            //concepto:
            page_Cur.AddMM(10, 44, new RepString(propDatos, "CONCEPTO"));

            //datos entidad
            page_Cur.AddMM(10, 54, new RepString(propCabeceras, "PERSONA O ENTIDAD"));
            page_Cur.AddMM(10, 62, new RepString(propCabeceras, "DIRECCIÓN"));
            page_Cur.AddRightMM(153, 70, new RepString(propCabeceras, "NÚM. DE CTA."));

            //datos CCC
            propDatos.bBold = true;
            page_Cur.AddCT_MM(180, 42, new RepString(propDatos, "CCC"));
            page_Cur.AddCT_MM(199, 52, new RepString(propDatos, "DC"));
            propDatos.bBold = false;

            //datos librado
            page_Cur.AddMM(10, 80, new RepString(propContacto, "Nombre y domicilio del librado"));
            page_Cur.AddMM(81, 80, new RepString(propContacto, "Firma, nombre y domicilio del librador"));

            //dirección
            FlowLayoutManager flmDireccion = new FlowLayoutManager(null);
            StaticContainer sc = new StaticContainerMM(64, 28);
            flmDireccion.SetContainer(sc);

            StringBuilder direccion = new StringBuilder();
            direccion.AppendLine("Neumáticos Atlántico, S.L.");
            flmDireccion.Add(new RepString(propDatos, direccion.ToString()));
            flmDireccion.NewLine(0.8);
            direccion.Remove(0, direccion.Length - 1);
            direccion.AppendLine("C/ Amapola Nº 5. El Chorrillo.");
            flmDireccion.Add(new RepString(propDatos, direccion.ToString()));
            flmDireccion.NewLine(0.8);
            direccion.Remove(0, direccion.Length - 1);
            direccion.AppendLine("CP: 38108 - Santa Cruz de Tenerife");
            flmDireccion.Add(new RepString(propDatos, direccion.ToString()));
            flmDireccion.NewLine(0.8);
            direccion.Remove(0, direccion.Length - 1);
            direccion.AppendLine("Tlf: 922 626282 / 922 615061");
            flmDireccion.Add(new RepString(propDatos, direccion.ToString()));

            page_Cur.AddMM(81, 85, sc);

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

