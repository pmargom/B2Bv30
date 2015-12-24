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
    public partial class detalleProducto : System.Web.UI.Page
    {
        string sCodigo = "";
        private List<Producto> lsProductos;
        private Usuario user;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CURRENT_USER"] != null) user = Session["CURRENT_USER"] as Usuario;
                if (RouteData.Values["sCodigo"] != null) 
                    sCodigo = RouteData.Values["sCodigo"].ToString();
                cargarProducto(sCodigo);
            }
            catch
            {
                Response.Redirect(this.ResolveUrl("~/index.aspx"));            
            }
            
        }

        private void cargarProducto(string sCodigo)
        {
            try
            {
                ServiceSoapClient servicio = new ServiceSoapClient();

                //producto del que se quiere ver el detalle
                Producto p = servicio.ProductosDataV3(sCodigo)[0];

                //lista de productos para el navegador de productos [<][>] y para las miniaturas de la parte inferior
                int id = (user != null) ? user.idUsuario.Value : 113;
                int res = 0;
                lsProductos = servicio.ProductosBuscadorV3(id, null, null, null, null, null, null, null, 1, 8, ref res, "nombre", null);

                //imágenes superiores
                string nombreProducto = p.VP_DESCRIPCION;
                string nombreImagen = (p.ID % 2 == 0) ? "llanta.jpg" : "neumatico.jpg"; // TODO: cambiar por p.VP_IMAGEN;
                string rutaImagen = ResolveUrl(string.Format("{0}/{1}", ConfigurationManager.AppSettings["DirImagenes"], nombreImagen));
                string imagen = string.Format("<img style='display: block;' src='{0}' alt='{1}' title='{1}' width='300' />", rutaImagen, nombreProducto);
                string rutaCompleta = string.Format("<a style='position: relative; display: block;' href='{0}' rel='' title='{1}' id='zoom1' class='cloud-zoom'>{2}</a>", rutaImagen, nombreProducto, imagen);
                                      
                ltNombreProducto.Text = ltNombreProducto2.Text = ltNombreProducto3.Text = nombreProducto;

                ltImagen.Text = rutaCompleta;
                
                ltBotonImagen.Text = string.Format("<a id='zoom-btn' class='lightbox-group zoom-btn-small cboxElement' href='{0}' title=''>Zoom</a>", rutaImagen);

                //ltMiniatura.Text = string.Format("<a href='{0}' class='cloud-zoom-gallery lightbox-group' title='' rel='useZoom: \"zoom1\", smallImage: \"{0}\"'><img src='{0}' alt='' height='74' width='74'></a>", rutaImagen);

                //botones [<] y [>]
                int indice = lsProductos.FindIndex(x => (x.ID == p.ID));
                int indiceAnt = (indice == 0) ? -1 : (indice - 1);
                int indiceSig = (indice + 1 == lsProductos.Count) ? -1 : (indice + 1);

                Producto Ant, Sig;
                Ant = Sig = null;
                string desemantizAnt, desemantizSig, codAnt, codSig;
                desemantizAnt = desemantizSig = codAnt = codSig = null;

                if (indiceAnt >= 0)
                {
                    Ant = lsProductos.ElementAt(indiceAnt);
                    desemantizAnt = CProducto.m_DeSemantizar(Ant.VP_DESCRIPCION);
                    codAnt = Ant.VP_PRODUCTO;
                    ltEnlacePrevio.Text = string.Format("<a id='link-previous-product' href='{0}'>&nbsp;</a>", ResolveUrl("~/productos/" + desemantizAnt + "/" + codAnt));
                }

                if (indiceSig >= 0)
                {
                    Sig = lsProductos.ElementAt(indiceSig);
                    desemantizSig = CProducto.m_DeSemantizar(Sig.VP_DESCRIPCION);
                    codSig = Sig.VP_PRODUCTO;
                    ltEnlacePosterior.Text = string.Format("<a id='link-next-product' href='{0}'>&nbsp;</a>", ResolveUrl("~/productos/" + desemantizSig + "/" + codSig));
                }                

                //datos de producto
                ltCodigo.Text = p.VP_PRODUCTO;

                string clickBoton = "";
                if (p.Stock > 0)
                {
                    ltEnStock.Text = "En stock";
                    clickBoton = "(function ($) { $.B2BProductos.m_addToCart(\"cesta\", \"" + p.VP_PRODUCTO + "\", \"add\") })(jQuery);";
                    ltAddtoCart.Text = "<button type='button' title='Añadir a la cesta' class='button btn-cart' onclick='" + clickBoton + "'><span><span>Añadir a la cesta</span></span></button>"; //estaba obteniendo un string.Format exception
                }
                else
                {
                    ltEnStock.Text = "No disponible";
                    clickBoton = "alert(\"El producto está agotado\");";
                    //no mostramos el botón de comprar si no hay stock
                    ltAddtoCart.Text = "";
                }
                

                ltPrecio.Text = p.VP_PVP1.ToString("F2") + " €";

                ltDetalles.Text = string.Format("<dl><dt>Familia:</dt><dd>{0}</dd><dt>Ecotasa:</dt><dd>{1}</dd><dd>{2}</dd></dl>", p.VP_DESCFAM, p.Ecotasa, p.ECOTASA_DETALLES);

                //productos "También le puede interesar..."
                string productosInferior = "";
                string rutaProducto;
                int orden = 1;
                foreach(Producto prod in lsProductos) 
                {
                    rutaProducto = ResolveUrl("~/productos/" + CProducto.m_DeSemantizar(prod.VP_DESCRIPCION) + "/" + prod.VP_PRODUCTO);
                    nombreImagen = (prod.ID % 2 == 0) ? "llanta155.jpg" : "neumatico155.jpg"; // TODO: cambiar por p.VP_IMAGEN;
                    rutaImagen = ResolveUrl(string.Format("{0}/{1}", ConfigurationManager.AppSettings["DirImagenes"], nombreImagen));

                    productosInferior += CProducto.getListaInferiorProductos(rutaProducto, rutaImagen, prod.VP_DESCRIPCION, prod.PrecioUnidad.ToString("F2"), orden);
                    orden++;
                    if (orden == 5) orden = 1;
                }
                ltProductosInf.Text = productosInferior;
            }
            catch 
            {                
                
            }
        }
    }
}