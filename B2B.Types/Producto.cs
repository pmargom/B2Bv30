using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace B2B.Types
{
    public class Producto
    {
        public long ID { get; set; }
        public string VP_PRODUCTO { get; set; }
        public string VP_DESCRIPCION { get; set; }
        public string VP_MODELO { get; set; }
        public string VP_FAMILIA { get; set; }
        public string VP_DESCFAM { get; set; }
        public string VP_IC { get; set; }
        public string VP_IV { get; set; }
        public int Stock { get; set; }
        public int Stock24 { get; set; }
        public string PteLlegada { get; set; }
        public string Contenedor { get; set; }
        public int? VL_UNIDADES { get; set; }
        public decimal PrecioUnidad { get; set; }

        public decimal PVP { get; set; }

        public string VP_PRODUCTO1 { get; set; }
        public decimal? VP_TIPO_NEUMA { get; set; }
        public int VP_TIPO_OFER { get; set; }
        public string VP_COLOR_OFER { get; set; }
        public string VP_CATEGORIA { get; set; }
        public decimal Ecotasa { get; set; }
        public string ECOTASA_DETALLES { get; set; }

        public decimal? VP_SERIE { get; set; }
        public decimal? VP_LLANTA { get; set; }
        public string VP_MEDIDA { get; set; }
        public string VP_DESC_TIPO { get; set; }
        public decimal VP_PVP1 { get; set; }
        public decimal VP_PVP2 { get; set; }
        public decimal VP_PVP3 { get; set; }
        public DateTime MODIFICADO { get; set; }

        public decimal VP_PORC_IMP { get; set; }
        public decimal VT_PORC_IMP { get; set; }

        public string VP_IMAGEN { get; set; }
        public int Cantidad { get; set; }
        public string Importe { get; set; }
        public int? VP_IMPORTADO { get; set; }

        public string VF_DESCFAM { get; set; }
        public string VF_LOGO { get; set; }

        public decimal VP_NIVELRUIDO { get; set; }
        public string VP_EFICOMBUSTIBLE { get; set; }
        public string VP_ADHERENCIA { get; set; }
        public decimal VP_VALORRUIDO { get; set; }
        public bool EsReserva { get; set; }

        public Stock StockProducto { get; set; }

        public static IComparer ProductoSortByIGIC()
        {
            return (IComparer)new ProductoSortHelper();
        }
    }

    public class ProductoSortHelper : IComparer
    {
        // método para poder ordenar por el %IGIC
        int IComparer.Compare(object a, object b)
        {
            Producto pa = (Producto)a;
            Producto pb = (Producto)b;

            if (pa.VP_PORC_IMP > pb.VP_PORC_IMP) return 1;
            if (pa.VP_PORC_IMP < pb.VP_PORC_IMP) return -1;
            else return 0;
        }


    }

}
