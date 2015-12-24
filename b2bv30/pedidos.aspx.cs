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
    public partial class pedidos : System.Web.UI.Page
    {        
        private List<Producto> lsProductos;
        private Usuario usuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CURRENT_USER"] != null) usuario = Session["CURRENT_USER"] as Usuario;
                getDataPedidos();
            }
            catch
            {
                Response.Redirect(this.ResolveUrl("~/index.aspx"));            
            }
            
        }

        private void getDataPedidos()
        {
            try
            {
                ltNombreCategoria.Text = "Información administrativa / Pedidos";

       
                B2BWs.ServiceSoapClient cliente = new B2BWs.ServiceSoapClient();

                List<Pedido> lsPedidos = cliente.PedidosData(null, null, null, null, null,null);

                string texto = "";
                
                texto += "            <table id='shopping-cart-table' class='data-table cart-table'>";                
                texto += "                <thead>";
                texto += "                    <tr class='first last'>";
                texto += "                        <th>Ver</th>";
                texto += "                        <th>Referencia</th>";
                //texto += "                        <th>Albarán</th>";
                texto += "                        <th>F. Creación</th>";
                //texto += "                        <th>F. Envío</th>";
                //texto += "                        <th>F. Entrega</th>";
                texto += "                        <th>Base Imp.</th>";
                texto += "                        <th>% Dto.</th>";
                texto += "                        <th>Imp. Dto.</th>";
                texto += "                        <th>Base + Dto.</th>";
                texto += "                        <th>Ecotasa</th>";
                texto += "                        <th>IGIC</th>";
                texto += "                        <th>Total</th>";  
                texto += "                    </tr>";
                texto += "                </thead>";
                texto += "                <tbody>";

                //generamos las filas de producto    

                string claseFila = "";

                int i = 0;
                foreach (Pedido p in lsPedidos)
                {
                    if (i == 0) claseFila += "first ";
                    if (i == 10) claseFila += "last ";
                    claseFila += ((i % 2 == 0) ? "even" : "odd");

                    texto += "<tr class='" + claseFila + "'>";
                    texto += "    <td><a id='zoom-btn-" + p.idPedido + "' href='#' title='' onclick='(function ($) { $(\"#colorbox-" + p.idPedido + "\").show(\"slow\"); })(jQuery);'><img src='" + ResolveUrl("~/skin/frontend/default/MAG080146/images/zoom.png") + "' alt='' /></a></td>";
                    texto += "    <td>" + p.Referencia + "</td>";
                    //texto += "    <td>" + p.VF_AlBARAN + "</td>";
                    texto += "    <td>" + ((p.Fecha != null) ? p.Fecha.Value.ToShortDateString() : "") + "</td>";
                    //texto += "    <td>" + ((p.FechaEnvio != null) ? p.FechaEnvio.Value.ToShortDateString() : "") + "</td>";
                    //texto += "    <td>" + ((p.FechaEntrega != null) ? p.FechaEntrega.Value.ToShortDateString() : "") + "</td>";
                    texto += "    <td>" + p.BaseImponible.ToString("F2") + " €</td>";
                    texto += "    <td>" + p.Descuento.ToString("F2") + " €</td>";
                    texto += "    <td>" + p.ImporteDescuento.ToString("F2") + " €</td>";
                    texto += "    <td>" + (p.BaseImponible + p.Descuento).ToString("F2") + " €</td>";
                    texto += "    <td>0,00 €</td>";
                    texto += "    <td>" + p.IGIC.ToString("F2") + "</td>";
                    texto += "    <td>" + p.Total.ToString("F2") + " €</td>";
                    texto += "</tr>";

                    claseFila = "";
                    i++;
                    
                }
                texto += "                </tbody>";
                texto += "            </table>";
                foreach (Pedido p in lsPedidos) texto += generarDetalles(p);
                
                lblContenido.Text = texto;
                //Response.Write("&nbsp;");
            }
            catch
            {
                //Response.Write("");
            }


        }            
        private string generarDetalles(Pedido p)
        {
            try
            {
                B2BWs.ServiceSoapClient cliente = new B2BWs.ServiceSoapClient();

                string texto = "";
                texto += "<div id='colorbox-" + p.idPedido + "' class='detalle' style='padding-bottom: 20px; padding-right: 20px; top: 0px; left: 277px; position: absolute; width: 849px; height: 605px; opacity: 1; cursor: auto; display: none;'>";
                texto += "    <div id='cboxWrapper' style='height: 625px; width: 869px;'>";
                texto += "        <div>";
                texto += "            <div id='cboxTopLeft' style='float: left;'></div>";
                texto += "            <div id='cboxTopCenter' style='float: left; width: 849px;'></div>";
                texto += "            <div id='cboxTopRight' style='float: left;'></div>";
                texto += "        </div>";
                texto += "        <div style='clear: left;'>";
                texto += "            <div id='cboxMiddleLeft' style='float: left; height: 605px;'></div>";
                texto += "            <div id='cboxContent' style='float: left; width: 849px; height: 605px;'>";
                texto += "                <div id='cboxLoadedContent' style='overflow: auto; height: 565px;'>";
                texto += "                  <div style='float: left; width: 24%'><dl>";
                texto += "                      <dt>Referencia:</dt><dd>" + p.Referencia + "</dd>";
                texto += "                      <dt>Creación:</dt><dd>" + ((p.Fecha != null) ? p.Fecha.Value.ToShortDateString() : "")+ "</dd>";
                texto += "                      <dt>Enviado:</dt><dd>" + ((p.FechaEnvio != null) ? p.FechaEnvio.Value.ToShortDateString() : "") + "</dd>";               
                texto += "                  </dl></div>";
                texto += "                  <div style='float: left; width: 24%'><dl>";                
                texto += "                      <dt>Estado:</dt><dd>Pendiente</dd>";
                texto += "                      <dt>Nº de albarán: </dt><dd>" + p.VF_AlBARAN + "</dd>";
                texto += "                      <dt>Cliente:</dt><dd>" + p.Cliente.VC_NOMBRE + "</dd><dd>" + p.Cliente.VC_CIF + "</dd><dd>" + p.Cliente.VC_DENOMINACION + "</dd>";
                texto += "                  </dl></div>";
                texto += "                  <div style='float: left; width: 24%'><dl>";
                texto += "                      <dt>Base imponible:</dt><dd>" + p.BaseImponible + " €</dd>";
                texto += "                      <dt>Descuento:</dt><dd>" + p.Descuento + " €</dd>";
                texto += "                      <dt>Ecotasa:</dt><dd>0,00 €</dd>";
                texto += "                  </dl></div>";
                texto += "                  <div style='float: left; width: 24%'><dl>";
                texto += "                      <dt>IGIC:</dt><dd>" + p.IGIC + " €</dd>";
                texto += "                      <dt>Observaciones:</dt><dd>" + p.Observaciones + "</dd>";
                texto += "                      <dt>Dirección de envío</dt><dd>" + p.DirEnvio + "</dd>";
                texto += "                  </dl></div>";
                texto += "<div style='clear:both; margin-bottom: 20px'><div style='float:left'><a href='" + p.Fichero + "' target='_blank'>Descargar PDF</a></div><div style='float:right'><label class='total'>Total: " + p.Total.ToString("F2") + " €</label></div></div>";

                List<Producto> lsProds = new List<Producto>();
                lsProds = cliente.ProductosPedidoGetData(p.idPedido);
                if (lsProds != null)
                {
                    texto += "<table id='tbProductos'><tr><th>Unid.</th><th>Ref. Producto</th><th>Producto</th><th>Marca</th><th>Modelo</th><th>Neumático</th><th>Cat.</th><th>Eco.</th><th>PVP</th><th>% IGIC</th></tr>";

                    foreach (Producto prod in lsProds)
                    {
                        texto += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>% IGIC</td></tr>", prod.Cantidad, prod.VP_PRODUCTO, prod.VP_DESCRIPCION, prod.VP_DESCFAM, prod.VP_MODELO, prod.VP_DESC_TIPO, prod.VP_CATEGORIA, prod.Ecotasa, prod.VP_PVP1, prod.VP_PORC_IMP);
                    }
                    texto += "</table>";
                }
                //else { texto += "No hay productos"; }
                texto += "                </div>";
                texto += "                <div id='cboxTitle' style='float: left; display: block;'></div>";
                texto += "                <div id='cboxClose' style='float: left;' onclick='(function ($) { $(\"#colorbox-" + p.idPedido + "\").hide(\"fast\"); })(jQuery);'>close</div>";
                texto += "            </div>";            
                texto += "        </div>";                
                texto += "    </div>";
                texto += "    <div style='position: absolute; width: 9999px; visibility: hidden; display: none;'></div>";
                texto += "</div>";

                return texto;
            }
            catch
            {
                return "";
            }
        }
    }
}