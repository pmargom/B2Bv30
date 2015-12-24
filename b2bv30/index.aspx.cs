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
    public partial class index : System.Web.UI.Page
    {
        private List<Producto> lsProductos;
        private Usuario user;
  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENT_USER"] != null) user = Session["CURRENT_USER"] as Usuario;
            //primera carga de la página
            if (Request.Form["Tarea"] == null) 
            {
                cargarProductosDestacados();
            }            
        }

        private void cargarProductosDestacados()
        {
            ServiceSoapClient servicio = new ServiceSoapClient();
            
            int id = (user != null) ? user.idUsuario.Value : 113;

            int res = 0;

            lsProductos = servicio.ProductosBuscadorV3(id, null, null, null, null, null, null, null, 1, 8, ref res, "nombre", null);

            cargarFiltros(res);

            string texto = "";

            if (lsProductos.Count == 0)
                texto += "No se obtuvo ningún registro";
            else
            {
                string rutaImagen = ConfigurationManager.AppSettings["DirImagenes"] + "/";

                foreach (Producto p in lsProductos)
                    texto += CProducto.getProducto(p.ID, p.VP_DESCRIPCION, p.PrecioUnidad, rutaImagen, this.Page, 0, "grid", p.VP_PRODUCTO);
            }
            ltProductos.Text = texto; 
        }

        private void cargarFiltros(int resultados)
        {
            string texto = "";
            ServiceSoapClient servicio = new ServiceSoapClient();
            List<Combo> lsFamilias = servicio.FamiliasCombo();
            List<Combo> lsTipoNeumatico = servicio.TipoNeumaticosCombo();

            string capturaFiltros = "\"(function ($) { $.B2BProductos.m_capturaFiltros('destacados', $('#ordenarPor').val(), 0, $('#nFilas').val(),$('#paginas li.current').text(), $('.view-mode strong').attr('class'), " + resultados + ",$('#medida').val(), $('#marca').val(), $('#modelo').val(), $('#neumatico').val(), $('#IC').val(), $('#IV').val()) })(jQuery);\"";

            texto += "<div class='pager'>";
            texto += "    <div style='clear:both'>";
            texto += "        <div class='pager_right'>";
            //paginado
            texto += "            <div class='pages' id='paginas'>";
            texto += "                <strong>Página:</strong>";
            texto += "                <ol>";
            texto += "                        <li class='current digit'><a href='#' onclick=" + capturaFiltros.Replace("$('#paginas li.current').text()", "1") + ">1</a></li>";

            //vamos a mostrar hasta tres botones del paginado
            if (resultados > 8)
                texto += "                <li class='digit'><a href='#nodolista' onclick=" + capturaFiltros.Replace("$('#paginas li.current').text()", "2") + ">2</a></li>";
            if (resultados > 16)
                texto += "                <li class='digit'><a href='#nodolista' onclick=" + capturaFiltros.Replace("$('#paginas li.current').text()", "3") + ">3</a></li>";
            if (resultados > 24)
                texto += "                <li><a href='#nodolista' onclick=" + capturaFiltros.Replace("$('#paginas li.current').text()", "'mas'") + ">&gt;</a></li>";
            texto += "                </ol>";
            texto += "            </div>";
            //cantidad de resultados
            texto += "            <div class='limiter'>";
            texto += "                <label>Mostrar</label>";
            texto += "                <select id='nFilas' onchange=" + capturaFiltros + ">";
            texto += "                    <option value='8' selected='selected'>8</option>";
            texto += "                    <option value='12'>12</option>";
            texto += "                    <option value='32'>32</option>";
            texto += "                </select>";
            texto += "                <span>registros</span>";
            texto += "            </div>";
            //ordenar por nombre o precio
            texto += "            <div class='sort-by'>";
            texto += "                <div class='desc-asc'>";
            texto += "                    <a href='#ordenAscDesc' id='ordenAscDesc' title='Orden ascendente o descendente' class='uparrow' onclick=" + capturaFiltros.Replace("0", "1") + "></a>";
            texto += "                </div>";
            texto += "                <label>Ordenar por</label>";
            texto += "                <select id='ordenarPor' onchange=" + capturaFiltros + ">";
            texto += "                    <option value='nombre' selected='selected'>Nombre</option>";
            texto += "                    <option value='precio'>Precio</option>";
            texto += "                </select>";
            texto += "            </div>";
            //presentación grid o lista
            texto += "            <div class='pager_left'>";
            texto += "                <p class='view-mode' style='margin-right:5px'>";
            texto += "                    <label>Ver como:</label>";
            texto += "                    <strong title='Grid' class='grid'>Grid</strong>&nbsp;";
            texto += "                    <a href='#nodolista' id='nodolista' title='Lista' class='list' onclick=" + capturaFiltros.Replace("$('.view-mode strong').attr('class')", "'list'") + ">Lista</a>&nbsp;";
            texto += "                </p>";
            texto += "                <p class='amount' id='nResultados'>1 a 8 de " + resultados + "</p>";
            texto += "            </div>";
            texto += "        </div>";
            texto += "    </div>";       
            //fila inferior
            texto += "    <div style='clear:both;padding-top:7px' id='divMarcas'>";
            texto += "        <table><tr><td><label title='Ejemplo de medida: 2055515' style='vertical-align:baseline'>Medida </label></td><td><label>Marca </label></td><td><label>Modelo </label></td><td><label>Tipo de Neumático </label></td><td><label style='vertical-align:baseline'>IC </label></td><td><label style='vertical-align:baseline'>IV </label></td><td></td></tr>";
            texto += "            <tr>";
            //medidas
            texto += "                <td><input type='text' title='Ejemplo de medida: 2055515' id='medida' class='input-text' style='width:110px' /></td>";
            //marcas
            texto += "                <td style='width: 150px'><div class='input-box limiter' id='divMarcas'><select id='marca' onchange='(function ($) { $.B2BProductos.m_refrescarModelos(\"marca\", $(\"#marca\").val()) })(jQuery);'>";
            foreach (Combo option in lsFamilias)
            {
                texto += string.Format("<option value='{0}'>{1}</option>", option.ValueMember, option.DisplayMember);
            }
            texto += "                </select></div></td>";
            //modelo
            texto += "                <td style='width: 150px'><select id='modelo' style='width: 150px'>";
            texto += "                     <option value=''>Todos los modelos</option>";
            texto += "                </select></td>";
            //tipo de neumático
            texto += "                <td  style='width: 150px'><div class='input-box limiter' id='divNeumaticos'><select id='neumatico'>";
            foreach (Combo option in lsTipoNeumatico)
            {
                texto += string.Format("<option value='{0}'>{1}</option>", option.ValueMember, option.DisplayMember);
            }
            texto += "                </select></div></td>";
            //IC
            texto += "                <td><input type='text' id='IC' class='input-text' style='width:30px;margin-right:10px' /></td>";
            //IV
            texto += "                <td><input type='text' id='IV' class='input-text' style='width:30px' /></td>";
            //botón buscar y limpiar
            texto += "                <td><button type='button' class='button' title='Buscar' id='btnBuscar' onclick=" + capturaFiltros + " style='float:right;margin-bottom:3px'><span><span>Buscar</span></span></button><br />";
            texto += "                    <button type='button' class='button' title='Buscar' id='btnLimpiar' onclick=" + capturaFiltros.Replace("$.B2BProductos", "$('#medida').val(''); $('#marca').val(''); $('#divMarcas a.sbSelector').text('Todas las marcas'); $('#modelo').val('');$('#divNeumaticos a.sbSelector').text('Todos los neumáticos'); $('#IC').val(''); $('#IV').val(''); $.B2BProductos") + " style='float:right;margin-left:10px'><span><span>Limpiar</span></span></button></td>";
            texto += "                </tr></table>";

            ltFiltros.Text = texto;
        }
    }
}