using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B2Bv30.B2BWs;

namespace B2Bv30
{
    public partial class vencimientos : System.Web.UI.Page
    {        
        private List<Producto> lsProductos;
        private Usuario user;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CURRENT_USER"] != null) user = Session["CURRENT_USER"] as Usuario;
                getDataVencimientos();
            }
            catch
            {
                Response.Redirect(this.ResolveUrl("~/index.aspx"));            
            }
            
        }

        private void getDataVencimientos()
        {
            try
            {
                ltNombreCategoria.Text = "Información administrativa / Vencimientos";

                string texto = "";
                texto += "            <table id='shopping-cart-table' class='data-table cart-table'>";                
                texto += "                <thead>";
                texto += "                    <tr class='first last'>";
                texto += "                        <th rowspan='1'>Acciones</th>";
                texto += "                        <th class='a-center' colspan='1'>Factura</th>";
                texto += "                        <th>Documento</th>";                
                texto += "                        <th class='a-center' colspan='1'>Forma de pago</th>";
                texto += "                        <th rowspan='1' class='a-center'>Vencimiento</th>";                        
                texto += "                    </tr>";
                texto += "                </thead>";
                texto += "                <tbody>";

                //generamos las filas de producto    

                string claseFila = "";

                for (int i = 0; i <= 10; i++)
                {
                    if (i == 0) claseFila += "first ";
                    if (i == 10) claseFila += "last ";
                    claseFila += ((i % 2 == 0) ? "even" : "odd");

                    texto += "<tr class='" + claseFila + "'>";
                    texto += "    <td><a id='zoom-btn' href='#' title='' onclick=''><img src='" + ResolveUrl("~/skin/frontend/default/MAG080146/images/zoom.png") + "' alt='' /></a></td>";     
                    texto += "    <td><h2 class='product-name'><a href='#'>AT1305434</a></h2></td>";
                    texto += "    <td><a href='#' title='' class='product-name'>AT1305434.pdf</a></td>";
                    texto += "    <td>-</td>";                    
                    texto += "    <td class='a-center'>27/06/2014</td>";
                    texto += "</tr>";
                    claseFila = "";
                }
                texto += "                </tbody>";
                texto += "            </table>";                
                lblContenido.Text = texto;
                //Response.Write("&nbsp;");
            }
            catch
            {
                //Response.Write("");
            }
        }
    }
}