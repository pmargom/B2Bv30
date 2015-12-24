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
    public partial class checkOut : System.Web.UI.Page
    {        
        private List<Producto> lsCesta;
        private Usuario usuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            tareas();            
        }

        private void tareas()
        {
            string tipo = Request.Form["tipo"];
            if (string.IsNullOrEmpty(tipo))
            {
                tipo = Request.QueryString["tipo"];
            }
            switch (tipo)
            {
                case "del":
                    vaciarCesta();
                    break;
                case "update":
                    actualizarCestaSuperior();
                    break;
                case "checkout":
                    ConfirmarPedido();
                    break;
                default:
                    getResumenCesta();
                    break;
            }
        }

        private void getResumenCesta()
        {
            string texto = "";
            try
            {
                List<Producto> lsCesta = new List<Producto>();
                bool usuarioLogueado = false;
                if (Session["CURRENT_USER"] != null)
                {
                    usuario = Session["CURRENT_USER"] as Usuario;
                    usuarioLogueado = true;
                }
                string rutaProducto, nombreImagen, rutaImagen;

                if (Session["CART"] != null)
                {
                    lsCesta = (List<Producto>)Session["CART"];
                    
                    bool confirmar = Request.Url.ToString().Contains("confirmar");

                    texto += "<article class='col-main'>";
                    texto += "    <div class='breadcrumbs'>";
                    texto += "        <ul>";
                    texto += "            <li class='home'>";
                    texto += "                <a href='" + ResolveUrl("~/") + "' title='Ir a página de inicio'>Inicio</a>";
                    texto += "                <span>/ </span>";
                    texto += "            </li>";
                    texto += "            <li class='product'><strong>" + ((confirmar) ? "Confirmar pedido" : "Resumen de la cesta de la compra") + "</strong></li>";
                    texto += "        </ul>";
                    texto += "    </div>";
                    texto += "    <div class='cart'>";
                    texto += "        <div class='page-title title-buttons'>";
                    texto += "            <h1>" + ((confirmar) ? "Confirmar pedido" : "Mi cesta") + "</h1>";
                    texto += "            <ul class='checkout-types'>";
                    texto += "                <li>";
                    if (confirmar)
                    {
                        if (usuarioLogueado)
                            texto += "                    <button type='button' title='Realizar pedido' class='button btn-proceed-checkout btn-checkout' onclick='(function ($) { $.B2BProductos.m_checkOut(\"checkout\") })(jQuery);'><span><span>Realizar compra</span></span></button>";
                        else
                            texto += "                    <button type='button' title='Inicie sesión para realizar el pedido' class='button btn-proceed-checkout btn-checkout' onclick='window.location = baseUrl + \"login\";'><span><span>Inicie sesión</span></span></button>";
                    }
                    else
                        texto += "                    <button type='button' title='Confirmar pedido' class='button btn-proceed-checkout btn-checkout' onclick='window.location = baseUrl + \"cesta/confirmar\";'><span><span>Confirmar compra</span></span></button>";                    
                    texto += "                </li>";
                    texto += "            </ul>";
                    texto += "        </div>";
                    texto += "        <ul class='messages' style='display:none'><li class='success-msg'><ul><li><span></span></li></ul></li></ul>";
                    texto += "        <fieldset>";
                    texto += "            <table id='shopping-cart-table' class='data-table cart-table'>";
                    texto += "                <colgroup><col width='1'><col><col width='1'><col width='1'><col width='1'><col width='1'><col width='1'></colgroup>";
                    texto += "                <thead>";
                    texto += "                    <tr class='first last'>";
                    texto += "                        <th rowspan='1'>&nbsp;</th>";
                    texto += "                        <th rowspan='1'>Producto</th>";
                    texto += "                        <th rowspan='1'></th>";
                    texto += "                        <th class='a-center' colspan='1'>Precio unitario</th>";
                    texto += "                        <th rowspan='1' class='a-center'>Cantidad</th>";
                    texto += "                        <th class='a-center' colspan='1'>Subtotal</th>";
                    texto += "                        <th rowspan='1' class='a-center'>&nbsp;</th>";
                    texto += "                    </tr>";
                    texto += "                </thead>";
                    texto += "                <tfoot>";
                    texto += "                    <tr class='first last'>";
                    texto += "                        <td colspan='50' class='a-right last'>";
                    texto += "                            <button type='button' title='Seguir comprando' class='button btn-continue' onclick='window.location =\"" + ResolveUrl("~/inicio") + "\"'><span><span>Seguir comprando</span></span></button>";
                    //texto += "                            <button type='submit' name='update_cart_action' value='update_qty' title='Update Shopping Cart' class='button btn-update'><span><span>Update Shopping Cart</span></span></button>";
                    if (!confirmar) 
                        texto += "                            <button type='button' name='update_cart_action' onclick='(function ($) { $.B2BProductos.m_vaciarCesta() })(jQuery);' value='empty_cart' title='Vaciar cesta' class='button btn-empty' id='empty_cart_button'><span><span>Vaciar cesta</span></span></button>";
                    texto += "                        </td>";
                    texto += "                    </tr>";
                    texto += "                </tfoot>";
                    texto += "                <tbody>";
                    //generamos las filas de producto    
                    int orden = 1;
                    decimal importe = 0;
                    decimal precio, importeUnitario;
                    int ultimo = lsCesta.Count;
                    bool esUltimo = false;
                    foreach (Producto p in lsCesta)
                    {
                        rutaProducto = ResolveUrl("~/productos/" + CProducto.m_Semantizar(p.VP_DESCRIPCION) + "/" + p.VP_PRODUCTO);
                        nombreImagen = (p.ID % 2 == 0) ? "llanta155.jpg" : "neumatico155.jpg"; // TODO: cambiar por p.VP_IMAGEN;
                        rutaImagen = ResolveUrl("~/" + ConfigurationManager.AppSettings["DirImagenes"] + "/" + nombreImagen);

                        precio = p.VP_PVP1;
                        importeUnitario = precio * p.Cantidad;
                        esUltimo = (orden == ultimo);
                        texto += CProducto.getProductoCheckOut(orden, rutaProducto, rutaImagen, p.VP_DESCRIPCION, p.Cantidad.ToString(), (precio.ToString("F2")), importeUnitario.ToString("F2"), p.VP_PRODUCTO, esUltimo, confirmar);
                        importe += importeUnitario;
                        orden++;
                    }
                    texto += "                </tbody>";
                    texto += "            </table>";
                    texto += "            <script type='text/javascript'>decorateTable('shopping-cart-table')</script>";
                    texto += "        </fieldset>";
                    texto += "        <div class='cart-collaterals'>";
                    texto += "            <div class='col1-set'>";
                    texto += "                <div class='col-2'>";
                    texto += "                    <div class='totals'>";
                    texto += "                        <table id='shopping-cart-totals-table'>";
                    texto += "                            <colgroup><col><col width='1'></colgroup>";
                    texto += "                            <tfoot>";
                    texto += "                                <tr>";
                    texto += "                                    <td style='' class='a-right' colspan='1'><strong>Importe</strong></td>";
                    texto += "                                    <td style='' class='a-right'>";
                    texto += "                                        <strong><span class='price importe'>" + importe.ToString("F2") + " €</span></strong>";
                    texto += "                                    </td>";
                    texto += "                                </tr>";
                    texto += "                            </tfoot>";
                    texto += "                            <tbody>";
                    texto += "                                <tr>";
                    texto += "                                    <td style='' class='a-right' colspan='1'>Subtotal</td>";
                    texto += "                                    <td style='' class='a-right'><span class='price importe'>" + importe.ToString("F2") + " €</span></td>";
                    texto += "                                </tr>";
                    texto += "                            </tbody>";
                    texto += "                        </table>";
                    texto += "                        <ul class='checkout-types'>";
                    texto += "                            <li>";
                    if (confirmar)
                    {
                        if (usuarioLogueado)
                            texto += "                    <button type='button' title='Realizar pedido' class='button btn-proceed-checkout btn-checkout' onclick='(function ($) { $.B2BProductos.m_checkOut(\"checkout\") })(jQuery);'><span><span>Realizar compra</span></span></button>";
                        else
                            texto += "                    <button type='button' title='Inicie sesión para realizar el pedido' class='button btn-proceed-checkout btn-checkout' onclick='window.location = baseUrl + \"login\";'><span><span>Inicie sesión</span></span></button>";
                    }
                    else
                        texto += "                    <button type='button' title='Confirmar pedido' class='button btn-proceed-checkout btn-checkout' onclick='window.location = baseUrl + \"cesta/confirmar\";'><span><span>Confirmar compra</span></span></button>";
                    texto += "                            </li>";
                    texto += "                        </ul>";
                    texto += "                    </div>";
                    texto += "                </div>";
                    texto += "            </div>";
                    texto += "        </div>";
                    texto += "    </div>";
                    texto += "</article>";

                   ltContenido.Text = texto;
                }
                else ltContenido.Text = "<div class='page-title'><h1>Cesta vacía</h1><p>No hay artículos en la cesta</p><button type='button' title='Seguir comprando' class='button btn-continue' onclick='window.location =\"" + ResolveUrl("~/inicio") + "\"'><span><span>Seguir comprando</span></span></button></div>";
            }
            catch
            {
                ltContenido.Text = "<div class='page-title'><h1>Cesta vacía</h1><p>No hay artículos en la cesta</p><button type='button' title='Seguir comprando' class='button btn-continue' onclick='window.location =\"" + ResolveUrl("~/inicio") + "\"'><span><span>Seguir comprando</span></span></button></div>";
            }
        }

        private void vaciarCesta()
        {
            Session.Remove("CART");
            Session.Remove("sCART");
            Response.Write("(0 artículos) - <span class='price'>0,00 €</span>");
        }

        private void actualizarCestaSuperior()
        {
            string sCantidades = Request.Form["sCantidades"];
            //recorremos el carrito adjudicanto la(s) nueva(s) cantidad(es)
            List<Producto> lsCesta = new List<Producto>();
                
            if (Session["CART"] != null)
            {
                lsCesta = (List<Producto>)Session["CART"];
                string[] sCantidad = sCantidades.Split(',');
                int cantidad = 1;
                for (int i = 0; i < lsCesta.Count; i++)
                {
                    int.TryParse(sCantidad[i], out cantidad);
                    lsCesta[i].Cantidad = (cantidad > 0) ? cantidad : 1;
                }
                Session.Add("CART", lsCesta);

                string sMiniCart = "";

                sMiniCart += "    <ol id='cart-sidebar' class='mini-products-list'>";
                string rutaProducto, nombreImagen, rutaImagen;
                int nArticulos = 0;
                decimal subTotal = 0;
                foreach (Producto p in lsCesta)
                {
                    rutaProducto = ResolveUrl("~/productos/" + CProducto.m_Semantizar(p.VP_DESCRIPCION) + "/" + p.VP_PRODUCTO);
                    nombreImagen = (p.ID % 2 == 0) ? "llanta155.jpg" : "neumatico155.jpg"; // TODO: cambiar por p.VP_IMAGEN;
                    rutaImagen = ResolveUrl("~/" + ConfigurationManager.AppSettings["DirImagenes"] + "/" + nombreImagen);

                    sMiniCart += "        <li class='item'>";
                    sMiniCart += "            <a href='" + rutaProducto + "' title='" + p.VP_PRODUCTO + "' class='product-image'>";
                    sMiniCart += "                <img src='" + rutaImagen + "' width='50' height='50' alt='" + p.VP_PRODUCTO + "'></a>";
                    sMiniCart += "            <div class='product-details'>";
                    sMiniCart += "                <a href='#' title='Eliminar " + p.VP_PRODUCTO + "' onclick='(function ($) { $.B2BProductos.m_addToCart(\"cesta\", \"" + p.VP_PRODUCTO + "\", \"del\") })(jQuery);' class='btn-remove'>Eliminar este artículo</a>";
                    sMiniCart += "                <p class='product-name'><a href='" + rutaProducto + "'>" + p.VP_PRODUCTO + "</a></p>";
                    sMiniCart += "                <strong>" + p.Cantidad + "</strong> x";
                    sMiniCart += "                <span class='price'>" + p.VP_PVP1.ToString("F2") + " €</span>";
                    sMiniCart += "            </div>";
                    sMiniCart += "        </li>";

                    subTotal += (p.VP_PVP1 * p.Cantidad);
                    nArticulos += p.Cantidad;
                }                
                sMiniCart += "    </ol>";

                string[] Carro = new string[3];

                //enviamos información en un array con el carrito de la columna izquierda y de la cabecera
                Carro[0] = "";
                Carro[1] = string.Format("({0} artículo{1}) - <span class='price'>{2} €</span>", nArticulos, ((nArticulos > 1) ? "s" : ""), subTotal.ToString("F2"));
                Carro[2] = sMiniCart;

                Session.Add("sCART", Carro);

                string respuesta = string.Format("{0}----{1}", Carro[1], Carro[2]);
                Response.Write(respuesta);
            }
            else
                ltContenido.Text = "<div class='page-title'><h1>Cesta vacía</h1><p>No hay artículos en la cesta</p><button type='button' title='Seguir comprando' class='button btn-continue' onclick='window.location =\"" + ResolveUrl("~/inicio") + "\"'><span><span>Seguir comprando</span></span></button></div>";
        }

        #region GUARDAR PEDIDO

        private void ConfirmarPedido()
        {
            string mensaje = "";
            mensaje += "<article class='col-main'>";
            mensaje += "    <div class='breadcrumbs'>";
            mensaje += "        <ul>";
            mensaje += "            <li class='home'>";
            mensaje += "                <a href='" + ResolveUrl("~/") + "' title='Ir a página de inicio'>Inicio</a>";
            mensaje += "                <span>/ </span>";
            mensaje += "            </li>";
            mensaje += "            <li class='product'><strong>Realizar pedido</strong></li>";
            mensaje += "        </ul>";
            mensaje += "    </div>";            
            mensaje += "    <div class='cart'>";
            mensaje += "        <div class='page-title title-buttons'>";
            mensaje += "            <h1>Realizar pedido</h1>";
            mensaje += "        </div>";
            mensaje += "        <ul class='messages'><li class='success-msg'><ul><li><span>mensaje</span></li></ul></li></ul>";
            mensaje += "    </div>";
            mensaje += "</article>";

            try
            {
                B2BWs.ServiceSoapClient cliente = new B2BWs.ServiceSoapClient();

                decimal temp = 0;
                usuario = (Usuario)Session["CURRENT_USER"];
                int idUsuario = usuario.idUsuario.Value;
                DateTime? fechaEnvio = DateTime.Now;
                DateTime? fechaEntrega = null;
                decimal? baseImp = null;
                decimal? descuento = usuario.DtoB2B;     
                decimal? nfu = 0;           
                decimal? igic = 0;          
                bool? porAgencia = null;

                //if (decimal.TryParse(txtImpBruto.Text.Trim(), out temp)) 
                    baseImp = temp;
                //if (decimal.TryParse(txtNFU.Text.Trim(), out temp)) 
                    nfu = temp;
                //if (decimal.TryParse(txtIGIC.Text.Trim(), out temp)) 
                    igic = temp;
                //porAgencia = chPorAgencia.IsChecked;

                List<B2BWs.Producto> lsProductos = (List<Producto>)Session["CART"]; 
                if (lsProductos == null || lsProductos.Count == 0)
                {
                    mensaje = mensaje.Replace("mensaje", "El pedido no tiene productos asociados.").Replace("success-msg", "error-msg");
                    ltContenido.Text = mensaje;
                    return;
                }                

                List<B2BWs.Producto> productosConStock = new List<B2BWs.Producto>();
                List<B2BWs.Producto> productosReservados = new List<B2BWs.Producto>();
                lsProductos.ForEach(p =>
                    {
                        if (p.EsReserva) 
                            productosReservados.Add(p);
                        else 
                            productosConStock.Add(p);
                    });
                string resultadoGuardarPedido = "";
                if (productosConStock.Count() > 0)
                {
                    resultadoGuardarPedido = cliente.PedidosGuardar(idUsuario, fechaEnvio, fechaEntrega, porAgencia,
                            string.Format("{0} {1}", usuario.cliente.VC_DIRECCION, usuario.cliente.VC_DIRECCIONB),
                            baseImp.Value, descuento.Value, nfu.Value, igic.Value, "", productosConStock);
                    //cliente.PedidosGuardarAsync(idUsuario, fechaEnvio, fechaEntrega, porAgencia,
                    //                            string.Format("{0} {1}", usuario.cliente.VC_DIRECCION, usuario.cliente.VC_DIRECCIONB),
                    //                            baseImp.Value, descuento.Value, nfu.Value, igic.Value, "", productosConStock);

                    //cliente.PedidosGuardarCompleted += new EventHandler<B2BWs.PedidosGuardarCompletedEventArgs>(cliente_PedidosGuardarCompleted);
                }

                //if (productosReservados.Count() > 0)
                //{
                //    cliente.ReservasGuardarAsync(idUsuario, fechaEnvio, fechaEntrega, porAgencia,
                //                string.Format("{0} {1}", usuario.cliente.VC_DIRECCION, usuario.cliente.VC_DIRECCIONB),
                //                0, 0, 0, 0, "", productosReservados);

                //    //cliente.ReservasGuardarCompleted += new EventHandler<B2BWs.ReservasGuardarCompletedEventArgs>(cliente_ReservasGuardarCompleted);
                //}
                if (string.IsNullOrEmpty(resultadoGuardarPedido))
                {
                    string s = "Gracias. Su pedido se ha realizado correctamente.";
                    s += string.Format("<br/><br/>Puede consultar su pedido <a href='{0}'>aqu&iacute;</a>.", this.ResolveUrl("~/" + "informacion-administrativa/pedidos"));
                    mensaje = mensaje.Replace("mensaje", s);
                }
                else
                {
                    mensaje = mensaje.Replace("mensaje", resultadoGuardarPedido + string.Format("<br/><br/>Puede consultar su pedido <a href='{0}'>aqu&iacute;</a>.", this.ResolveUrl("~/" + "informacion-administrativa/pedidos"))).Replace("success-msg", "error-msg");
                }
                ltContenido.Text = mensaje;               

                //Response.Write("ok");
            }
            catch (Exception ex)
            {                
                //Response.Write("");
                mensaje = mensaje.Replace("mensaje", "Se ha producido un error al generar el pedido.").Replace("success-msg", "error-msg");
                ltContenido.Text = mensaje;
            }
        }

        //void cliente_PedidosGuardarCompleted(object sender, B2BWs.PedidosGuardarCompletedEventArgs e)
        //{
        //    try
        //    {
        //        frPro.Close();

        //        string error = string.Empty;

        //        if (e.Error != null)
        //        {
        //            string mensaje = string.Format("Se ha producido un error en la creación del pedido. Consulte su email y en la sección de Pedidos.");
        //            MostrarError(TipoError.Warning, mensaje);
        //            return;
        //        }
        //        error = e.Result;
        //        if (!string.IsNullOrEmpty(error))
        //        {
        //            MostrarError(TipoError.Warning, string.Format("Error al guardar el pedido. {0}", error));
        //            return;
        //        }

        //        MostrarError(TipoError.Information, "Pedido confirmado con éxito");

        //        ResetearControles();
        //    }
        //    catch
        //    {
        //        if (frPro != null) frPro.Close();
        //    }
        //}

        //void cliente_ReservasGuardarCompleted(object sender, B2BWs.ReservasGuardarCompletedEventArgs e)
        //{
        //    try
        //    {
        //        frPro.Close();

        //        string error = string.Empty;

        //        if (e.Error != null)
        //        {
        //            string mensaje = string.Format("Se ha producido un error en la creación de la reserva. Consulte su email y en la sección de Reservas.");
        //            MostrarError(TipoError.Warning, mensaje);
        //            return;
        //        }
        //        error = e.Result;
        //        if (!string.IsNullOrEmpty(error))
        //        {
        //            MostrarError(TipoError.Warning, string.Format("Error al guardar la reserva. {0}", error));
        //            return;
        //        }

        //        MostrarError(TipoError.Information, "Reserva realizada con éxito");

        //        ResetearControles();
        //    }
        //    catch
        //    {
        //        if (frPro != null) frPro.Close();
        //    }
        //}

        #endregion

    }
}