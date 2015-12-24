using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

public class CProducto
{
    public CProducto() { }

    static public string m_Semantizar(string dato)
    {
        dato = dato.Replace(" ", "_");
        dato = dato.Replace("+", "-mas-");
        dato = dato.Replace("/", "-");
        return dato;
    }

    static public string m_DeSemantizar(string dato)
    {
        dato = dato.Replace("_", " ");
        dato = dato.Replace("-mas-", "+");
        dato = dato.Replace("=", "/");
        dato = dato.Replace("-", "/");
        return dato;
    }

    static public string getProducto(long idProducto, string descProducto, decimal precioUnidad, string rutaImagen, System.Web.UI.Page pagina, int columna, string presentacion, string codigo) 
    {
        try 
        {
            string texto = "";
            string nombreImagen = (idProducto % 2 == 0) ? "llanta155.jpg" : "neumatico155.jpg"; // TODO: cambiar por p.VP_IMAGEN;
            string descProductoDesemantizado = m_Semantizar(descProducto);
            string claseColumnas = "";
            string addToCart = "(function ($) { $.B2BProductos.m_addToCart(\"cesta\", \"codigo\", \"add\") })(jQuery);";

            if (presentacion == "grid")
            {
                claseColumnas = claseSegunColumna(columna);
                
                string descCorta = (descProducto.Length > 30) ? (descProducto.Substring(0, 27) + "...") : descProducto;                

                texto += "<li class='item product-items " + claseColumnas + "'>";
                texto += "    <div class='product-block'>";
                texto += "        <div class='product-block-inner'>";
                texto += "            <div class='new-label'>Nuevo</div>";
                texto += "            <a href='" + pagina.ResolveUrl("~/productos/" + descProductoDesemantizado + "/" + codigo) + "' title='" + descProducto + "' class='product-image'>";
                texto += "                <img src='" + pagina.ResolveUrl("~/" + rutaImagen + nombreImagen) + "' width='155' height='155' alt='" + descProducto + "' /></a>";
                texto += "            <h2 class='product-name'><a href='" + pagina.ResolveUrl("~/productos/" + descProductoDesemantizado + "/" + codigo) + "' title='" + descProducto + "'>" + descCorta + "</a></h2>";
                texto += "            <div class='price-box'>";
                texto += "                <span class='regular-price' id='product-price-" + idProducto + "'><span class='price'>" + precioUnidad.ToString("F2") + " €</span></span>";
                texto += "            </div>";
                texto += "            <div class='actions'>";
                texto += "                <button type='button' class='button btn-cart' onclick='" + addToCart.Replace("codigo", codigo) + "'>";
                texto += "                    <span><span>Añadir a la cesta</span></span>";
                texto += "                </button>";
                texto += "            </div>";
                texto += "        </div>";
                texto += "    </div>";
                texto += "</li>";
            }
            if (presentacion == "list")
            {
                texto += "<li class='item'>";
	            texto += "    <div class='list-left'>";
                texto += "        <div class='new-label'>Nuevo</div>";
                texto += "        <a href='" + pagina.ResolveUrl("~/productos/" + descProductoDesemantizado + "/" + codigo) + "' title='" + descProducto + "' class='product-image'>";
			    texto += "            <img src='" + pagina.ResolveUrl("~/" + rutaImagen + nombreImagen) + "' width='150' height='150' alt='" + descProducto + "' /></a>";		 
	            texto += "    </div>";		
	            texto += "    <div class='list-center'>";
		        texto += "        <div class='product-shop'>";
			    texto += "            <div class='f-fix'>";
                texto += "                <h2 class='product-name'><a href='" + pagina.ResolveUrl("~/productos/" + descProductoDesemantizado + "/" + codigo) + "' title='" + descProducto + "'>" + descProducto + "</a></h2>";
				texto += "                <div class='desc std'>" + descProducto + "</div>";                     
			    texto += "            </div>";
		        texto += "        </div>";
	            texto += "    </div>";			
	            texto += "    <div class='list-right'>";
		        texto += "        <div class='price-box'>";
			    texto += "            <span class='regular-price' id='product-price-" + idProducto + "'><span class='price'>" + precioUnidad.ToString("F2") + " €</span></span>";
		        texto += "        </div>";
		        texto += "        <p>";
                texto += "            <button type='button' class='button btn-cart' onclick='" + addToCart.Replace("codigo", codigo) + "'>";
				texto += "                <span><span>Añadir a la cesta</span></span>";
			    texto += "            </button>";
		        texto += "        </p>";
	            texto += "    </div>";
                texto += "</li>"; 
            }

            return texto; 
        }
        catch (Exception)
        {
            return "<li class='item product-items'></li>";            
        }   
    }

    static private string claseSegunColumna(int columna)
    {
        string claseColumnas;
        switch (columna)
        {
            case 1: claseColumnas = "first_item_tm"; break;
            case 4: claseColumnas = "last_item_tm"; break;
            default: claseColumnas = ""; break;
        }
        return claseColumnas;
    }

    static public string getListaInferiorProductos(string ruta, string imagen, string nombre, string precio, int columna)
    {
        try
        {
            string texto = "";
            string claseColumnas = claseSegunColumna(columna);

            texto += "<li class='item slider-item " + claseColumnas + "' style='width: 190px;'>";
            texto += "    <div class='product-block' style='height: 234px;'>";
            texto += "        <div class='product-block-inner'>";
            texto += "            <a href='" + ruta + "' title='" + nombre + "' class='product-image'>";
            texto += "                <img src='" + imagen + "' width='155' height='155' alt='" + nombre + "'></a>";
            texto += "            <h3 class='product-name'><a href='" + ruta + "' title='" + nombre + "'>" + nombre + "</a></h3>";
            texto += "            <div class='price-box'>";
            texto += "                <span class='regular-price' id='product-price-117-upsell'>";
            texto += "                    <span class='price'>" + precio + " €</span></span>";
            texto += "            </div>";
            texto += "        </div>";
            texto += "    </div>";
            texto += "</li>";            
            return texto;
        }
        catch (Exception)
        {
            return "";
        }
    }

    static public string getProductoCheckOut(int nFila, string rutaProducto, string rutaImagen, string nombre, string cantidad, string precio, string total, string codigo, bool filaFinal, bool confirmar)
    {
        string texto = "";
        string claseFila = "";
        try
        {
            if (nFila == 1) claseFila += "first ";
            if (filaFinal) claseFila += "last ";
            claseFila += ((nFila % 2 == 0) ? "even" : "odd");

            texto += "<tr class='" + claseFila + "' id='tr_" + nFila + "'>";
            texto += "    <td><a href='" + rutaProducto + "' title='" + nombre + "' class='product-image'>";
            texto += "        <img src='" + rutaImagen + "' width='75' height='75' alt='" + nombre + "'></a></td>";
            texto += "    <td>";
            texto += "        <h2 class='product-name'>";
            texto += "            <a href='" + rutaProducto + "'>" + nombre + "</a>";
            texto += "        </h2>";
            texto += "    </td>";
            texto += "    <td class='a-center'>";
            //texto += "        <a href='" + rutaProducto + "?edit=1' title='Editar artículo'>Editar</a>";
            texto += "    </td>";
            texto += "    <td class='a-right'>";
            texto += "        <span class='cart-price'>";
            texto += "            <span class='price'>" + precio + " €</span>";
            texto += "        </span>";
            texto += "    </td>";            
            texto += "    <td class='a-center'>";
            if (confirmar)
                texto += "        <span class='cart-price'><span class='price'>" + cantidad + "</span></span>";
            else
                texto += "        <input value='" + cantidad + "' size='4' title='Cantidad' id='prod_" + nFila + "' class='input-text qty' maxlength='12' onkeypress='return esNumero(event);' onblur='(function ($) { $.B2BProductos.m_actualizarResumenCesta(\"prod_" + nFila + "\"); })(jQuery);' />";
            texto += "    </td>";            
            texto += "    <td class='a-right'>";
            texto += "        <span class='cart-price'>";
            texto += "            <span class='price total'>" + total + " €</span>";
            texto += "        </span>";
            texto += "    </td>";
            texto += "    <td class='a-center last'>";
            if (!confirmar)
                texto += "<a href='#' onclick='' title='Eliminar elemento' class='btn-remove btn-remove2'>Eliminar elemento</a>"; //(function ($) { $.B2BProductos.m_actualizarResumenCesta(\"prod_" + nFila + "\",1) })(jQuery);
            texto += "    </td>";
            texto += "</tr>";

            return texto;
        }
        catch
        {
            return "";
        }
    }

}

