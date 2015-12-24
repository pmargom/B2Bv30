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
    public partial class facturas : System.Web.UI.Page
    {        
        private List<Producto> lsProductos;
        private Usuario user;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CURRENT_USER"] != null) user = Session["CURRENT_USER"] as Usuario;
                getdata();
            }
            catch
            {
                Response.Redirect(this.ResolveUrl("~/index.aspx"));            
            }
            
        }

        private void getdata()
        {
            string tipo = Request.Form["tipo"];
            switch (tipo)
            {
                case "facturas":
                    getDataFacturas();
                    break;
                case "albaranes":
                    getDataAlbaranes();
                    break;
                case "vencimientos":
                    getDataVencimientos();
                    break;
                case "pedidos":
                    getDataPedidos();
                    break;
                default:
                    getDataFacturas();
                    break;
            }
        }

        private void getDataFacturas()
        {
            try
            {
                ltNombreCategoria.Text = "Información administrativa / Facturas";

                string texto = "";
                texto += "            <table id='shopping-cart-table' class='data-table cart-table'>";
                //texto += "                <colgroup><col width='1'><col width='1'><col width='1'><col width='1'><col width='1'></colgroup>";
                texto += "                <thead>";
                texto += "                    <tr class='first last'>";
                texto += "                        <th rowspan='1'>Acciones</th>";
                texto += "                        <th>Cliente</th>";
                texto += "                        <th class='a-center' colspan='1'>Factura</th>";
                texto += "                        <th rowspan='1' class='a-center'>Fecha</th>";
                texto += "                        <th class='a-center' colspan='1'>Importe Neto</th>";                
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
                    texto += "    <td><a href='#' title='' class='product-name'>NEUMÁTICOS ATLÁNTICO, S. L.</a></td>";                    
                    texto += "    <td><h2 class='product-name'><a href='#'>FV1300544</a></h2></td>";
                    texto += "    <td class='a-center'>27/06/2014</td>";   
                    texto += "    <td class='a-right last'>";
                    texto += "        <span class='cart-price'>";
                    texto += "            <span class='price'>0,00 €</span>";
                    texto += "        </span>";
                    texto += "    </td>";                                        
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
                Response.Write("");
            }
        }

        private void getDataAlbaranes()
        {
            try
            {
                ltNombreCategoria.Text = "Información administrativa / Albaranes";

                string texto = "";
                texto += "            <table id='shopping-cart-table' class='data-table cart-table'>";                
                texto += "                <thead>";
                texto += "                    <tr class='first last'>";
                texto += "                        <th rowspan='1'>Acciones</th>";
                texto += "                        <th>Cliente</th>";
                texto += "                        <th class='a-center' colspan='1'>Factura</th>";
                texto += "                        <th class='a-center' colspan='1'>Albarán</th>";
                texto += "                        <th rowspan='1' class='a-center'>Fecha</th>";
                texto += "                        <th class='a-center' colspan='1'>Importe Neto</th>";
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
                    texto += "    <td></td>";
                    texto += "    <td><a href='#' title='' class='product-name'>NEUMÁTICOS ATLÁNTICO, S. L.</a></td>";
                    texto += "    <td></td>";
                    texto += "    <td><h2 class='product-name'><a href='#'>AT1305434</a></h2></td>";
                    texto += "    <td class='a-center'>27/06/2014</td>";
                    texto += "    <td class='a-right last'>";
                    texto += "        <span class='cart-price'>";
                    texto += "            <span class='price'>0,00 €</span>";
                    texto += "        </span>";
                    texto += "    </td>";
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

        private void getDataVencimientos()
        {
            try
            {
                ServiceSoapClient servicio = new ServiceSoapClient();


            }
            catch
            {

            }
        }

        private void getDataPedidos()
        {
            try
            {
                ServiceSoapClient servicio = new ServiceSoapClient();


            }
            catch
            {

            }
        }
    }
}