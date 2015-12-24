using System;
using System.Collections.Generic;
using System.Text;
using Root.Reports;
using B2B.Types;

namespace BAL.pdf
{

    public class RepFactura : Report
    {
        FontDef _definicionFuente;
        FontProp _propiedadFuente;
        private Factura datos;

        public Factura Datos
        {
            set
            {
                datos = value;
            }
        }

        public RepFactura() { }

        protected override void Create()
        {
            NuevaPagina();
            InicializarPropiedadesReporte();
            ImprimirHolaMundo();
        }

        private void InicializarPropiedadesReporte()
        {
            // Este estilo de fuente debe ser definido sólo una vez
            _definicionFuente = new FontDef(this, FontDef.StandardFont.TimesRoman);
            _propiedadFuente = new FontProp(_definicionFuente, 10, System.Drawing.Color.Navy);
        }

        private void ImprimirHolaMundo()
        {
            page_Cur.AddCT_MM(100, 50, new RepString(_propiedadFuente, "Hola mundo... ¿que esperabas?"));
        }
        internal void NuevaPagina()
        {
            new Root.Reports.Page(this);
        }
    }

}
