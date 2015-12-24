using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B2Bv30.B2BWs;

public partial class funciones_comun : System.Web.UI.Page
{
    private B2Bv30.B2BWs.Usuario user = null;    
    private string version = ConfigurationManager.AppSettings["version"];

    protected void Page_Load(object sender, EventArgs e)
    {
        string tarea = Request.Form["Tarea"];

        #region LOGIN

        if (tarea == "logout") logout();
        if (tarea == "login") login();

        #endregion

        if (tarea != "logout")
        {
            //user = Session["user"] as B2Bv30.B2BWs.Usuario;
            //if (user == null) Response.Redirect("~/login");
            //else TipoUsuario = user.tipo;

            if (tarea == "getdata") getdata();
            //if (tarea == "addregistro") addregistro();
            //if (tarea == "eliminar") eliminar();            
        }
    }

    private string cargarScriptsLibreria()
    {
        string texto = "";

        string version = ConfigurationManager.AppSettings["version"];

        texto += "<script src='" + ResolveUrl("~/customJS/Comun.js?version=" + version) + "' type='text/javascript'></script>";
        texto += "<script src='" + ResolveUrl("~/customJS/Util.js?version=" + version) + "' type='text/javascript'></script>";
        texto += "<script src='" + ResolveUrl("~/customJS/General.js?version=" + version) + "' type='text/javascript'></script>";
        texto += "<script src='" + ResolveUrl("~/customJS/B2BProductos.js?version=" + version) + "' type='text/javascript'></script>";
        texto += "<script src='" + ResolveUrl("~/customJS/B2BInfo.js?version=" + version) + "' type='text/javascript'></script>";

        return texto;
    }

    private void getdata()
    {
        string tipo = Request.Form["tipo"];
        switch (tipo)
        {
            case "destacados":
                getDataDestacados();
                break;
            case "cesta":
                getDataCesta();
                break;
            case "marca":
                getDataModelos();
                break;
            default:
                break;
        }
    }

    #region PRODUCTOS

    private void getDataDestacados()
    {
        try
        {
            ServiceSoapClient servicio = new ServiceSoapClient();

            int id = (user != null) ? user.idUsuario.Value : 113;
            int res = 0;

            //obtenemos los parámetros de la consulta
            string ordenarPor = Request.Form["ordenarPor"];
            string ordenAscDesc = Request.Form["ordenAscDesc"];
            string regPagina = Request.Form["regPagina"];
            string pagina = Request.Form["pagina"];
            string presentacion = Request.Form["presentacion"];
            string medida = Request.Form["medida"];
            string marca = Request.Form["marca"];
            string modelo = Request.Form["modelo"];
            string tNeumatico = Request.Form["neumatico"];
            string IC = Request.Form["IC"];
            string IV = Request.Form["IV"];

            if (string.IsNullOrEmpty(medida)) medida = null;
            else medida = medida.Trim();

            if (string.IsNullOrEmpty(marca)) marca = null;
            else marca = marca.Trim();
            
            if (string.IsNullOrEmpty(modelo)) modelo = null;
            else modelo = modelo.Trim();

            int? tipoNeumatico = null;
            int tNeuma = -1;
            if (string.IsNullOrEmpty(tNeumatico)) tNeumatico = null;
            else
            { 
                tNeumatico = tNeumatico.Trim();
                int.TryParse(tNeumatico, out tNeuma);
                if (tNeuma > 0) tipoNeumatico = tNeuma;
            }
            if (string.IsNullOrEmpty(IC)) IC = null;
            else IC = IC.Trim();
            if (string.IsNullOrEmpty(IV)) IV = null;
            else IV = IV.Trim();  

            int regP, pag;            
            int.TryParse(regPagina, out regP);
            int.TryParse(pagina, out pag);

            if (regP == 0 || pag == 0) { regP = 8; pag = 1; }

            List<Producto> lsProductos = servicio.ProductosBuscadorV3(id, medida, marca, modelo, tipoNeumatico, IC, IV, null, pag, regP, ref res, ordenarPor, ordenAscDesc);

            string texto = "";

            if (lsProductos.Count == 0)
            {
                texto = "No se obtuvo ningún registro";
            }
            else
            {
                string rutaImagen = ConfigurationManager.AppSettings["DirImagenes"] + "/";
                if (presentacion == "grid") texto += "<ul style='width: 760px;' class='products-grid' id='featured-grid'>";
                if (presentacion == "list") texto += "<ul class='products-list' id='products-list'>";

                int orden = 1;
                foreach (Producto p in lsProductos)
                {
                    texto += CProducto.getProducto(p.ID, p.VP_DESCRIPCION, p.PrecioUnidad, rutaImagen, this.Page, orden, presentacion, p.VP_PRODUCTO);
                    orden++;
                    if (orden == 5) orden = 1;
                }
                texto += "</ul>";
            }      
            //enviamos en un array de tres elementos el html a representar y la cantidad de resultados
            string[] resultado = new string[2];
            resultado[0] = texto;
            resultado[1] = res.ToString();           
            string jsonString = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(resultado);
            Response.Write(jsonString);
        }
        catch (Exception)
        {
            string[] resultado = new string[2];
            resultado[0] = "No se obtuvo ningún resultado";
            resultado[1] = "0";            
            string jsonString = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(resultado);
            Response.Write(jsonString);          
        }
    }

    private void getDataModelos()
    {
        try
        {
            string marca = Request.Form["marca"];
            
            string textoModelos = "";
            textoModelos += "<option value=''>Todos los modelos</option>";
            if (!string.IsNullOrEmpty(marca)) 
            {
                List<Combo> lsModelos = (new ServiceSoapClient()).ModelosCombo(marca);

                foreach (Combo option in lsModelos)
                {
                    if (!string.IsNullOrEmpty(option.DisplayMember))
                        textoModelos += string.Format("<option value='{0}'>{1}</option>", option.DisplayMember.Trim(), option.DisplayMember);
                }
            }


            Response.Write(textoModelos);
        }
        catch (Exception)
        {
            Response.Write("");
        }

    }

    private void getDataCesta()
    {
        string texto = "";
        try
        {
            string sCodigo = Request.Form["sCodigo"];
            string sCantidad = Request["sCantidad"];
            int cantidad = -1;
            string accion = Request.Form["accion"];
            
            //obtenemos el producto que se está eliminando o añadiendo
            ServiceSoapClient servicio = new ServiceSoapClient();
            Producto prod = servicio.ProductosDataV3(sCodigo)[0];
            if (!string.IsNullOrEmpty(sCantidad))
            {
                int.TryParse(sCantidad, out cantidad);
                prod.Cantidad = cantidad;
            }

            List<Producto> lsCesta;

            if (Session["CART"] != null) lsCesta = (List<Producto>)Session["CART"];
            else lsCesta = new List<Producto>();

            int indice = lsCesta.FindIndex(x => x.ID == prod.ID);
            if (accion == "add")
            {
                //comprobamos si ya existe, para aumentar la cantidad
                if (lsCesta.Exists(x => x.ID == prod.ID))                                    
                    lsCesta[indice].Cantidad += cantidad;                
                else 
                    lsCesta.Add(prod);
            }
            if (accion == "del") lsCesta.RemoveAt(indice);

            Session.Add("CART", lsCesta);
            string sMiniCart = ""; //guardamos el cuntenido de la lista del carrito para el resumen del carrito en la cabecera
            int nArticulos = 0;
            decimal subTotal = 0;

            if (lsCesta.Count == 0)
            {
                texto += "<div class='block-content'><p class='empty'>No tiene elementos en su cesta</p></div>";
                sMiniCart += "<p>No tiene artículos en su cesta</p>";
            }
            else
            {                
                foreach (Producto p in lsCesta) 
                { 
                    subTotal += (p.VP_PVP1 * p.Cantidad);
                    nArticulos += p.Cantidad;
                }
                string rutaProducto, nombreImagen, rutaImagen;
                //no vamos a mostrar la columna izquierda
                //texto += "<script type='text/javascript'>$(document).ready(function () { $('aside').hide(); $('.col-main').css('width', '100%'); }); </script>";

                texto += "<div class='block-content'>";
                texto += "    <div class='summary'>";
                texto += "        <p class='amount'>Hay <a href='#' onclick='window.location = baseUrl + \"cesta/\";'>" + ((nArticulos == 1) ? "un artículo" : (nArticulos + " artículos")) + "</a> en la cesta.</p>";
                texto += "        <p class='subtotal'>";
                texto += "            <span class='label'>Subtotal:</span> <span class='price'>" + subTotal.ToString("F2") + " €</span>";
                texto += "        </p>";
                texto += "    </div>";
                texto += "    <div class='actions'>";
                texto += "        <button type='button' title='Checkout' class='button' onclick='window.location = baseUrl + \"cesta/confirmar\";'><span><span>Checkout</span></span></button>&nbsp;";
                texto += "        <button type='button' title='Ver cesta' class='button' onclick='window.location = baseUrl + \"cesta/\";' style='margin-right: 3px;'><span><span>Ver cesta</span></span></button>";
                texto += "    </div>";
                sMiniCart += "    <ol id='cart-sidebar' class='mini-products-list'>";
                texto += sMiniCart;
                foreach (Producto p in lsCesta)
                {
                    rutaProducto = ResolveUrl("~/productos/" + CProducto.m_Semantizar(p.VP_DESCRIPCION) + "/" + p.VP_PRODUCTO);
                    nombreImagen = (p.ID % 2 == 0) ? "llanta155.jpg" : "neumatico155.jpg"; // TODO: cambiar por p.VP_IMAGEN;
                    rutaImagen = ResolveUrl("~/" +  ConfigurationManager.AppSettings["DirImagenes"] + "/" + nombreImagen);

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
                }
                texto += sMiniCart;
                texto += "    </ol>";
                sMiniCart += "    </ol>";
                texto += "    <script type='text/javascript'>decorateList('cart-sidebar', 'none-recursive')</script>";
                texto += "</div>";

                //pie del  carrito superior
                sMiniCart += "    <p class='subtotal'>";
                sMiniCart += "        <span class='label'>Subtotal:</span> <span class='price'>" + subTotal.ToString("F2") + " €</span>";
                sMiniCart += "    </p>";
                sMiniCart += "    <div class='actions'>";
                sMiniCart += "        <button type='button' title='Ver cesta' class='button' onclick='window.location = baseUrl + \"cesta/\";'><span><span>Ver cesta</span></span></button>";
                sMiniCart += "        <button type='button' title='Checkout' class='button' onclick='window.location = baseUrl + \"cesta/confirmar\";'><span><span>Checkout</span></span></button>";
                sMiniCart += "    </div>";
            }
            
            string[] Carro = new string[3];

            //enviamos información en un array con el carrito de la columna izquierda y de la cabecera
            Carro[0] = texto;
            Carro[1] = string.Format("({0} artículo{1}) - <span class='price'>{2} €</span>", nArticulos, ((nArticulos > 1) ? "s" : ""), subTotal.ToString("F2"));
            Carro[2] = sMiniCart;

            Session.Add("sCART", Carro);
            string jsonString = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(Carro);
            Response.Write(jsonString);
        }
        catch 
        {
            Response.Write("");
        }
    }

    #endregion

    #region LOGIN

    private void login()
    {
        string texto = "";
        try
        {
            string Login = Request.Form["user"];
            string Password = Request.Form["pass"];
            //Password = MD5.MD5Encrypt(Password);
            //DataAccess.DataAccess da = new DataAccess.DataAccess();

            B2Bv30.B2BWs.Usuario res = new B2Bv30.B2BWs.ServiceSoapClient().Login(Login, Password);

            if (res != null)
            {
                texto += "ok";
                Session.Add("CURRENT_USER", res);
            }
        }
        catch { }
        Response.Write(texto);
    }

    private void logout()
    {
        try
        {
            Session.Clear();
            if (Session["CURRENT_USER"] == null) Response.Write("ok");
            else Response.Write("");
        }
        catch
        {
            Response.Write("");
        }
    }

    #endregion

}