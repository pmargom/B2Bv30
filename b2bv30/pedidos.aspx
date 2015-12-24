<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="pedidos.aspx.cs" Inherits="B2Bv30.pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                $(".block-cart").hide();
                var categorias = $("#category-treeview");
                categorias.empty();
                //inserto las nuevas categorías
                var facturas, albaranes, vencimientos, pedidos;
                facturas = "<li class='level0 nav-1'><a href='#' onclick='window.location = baseUrl + \"informacion-administrativa/facturas/\"'><span>Facturas</span></a></li>";
                albaranes = "<li class='level0 nav-2'><a href='#' onclick='window.location = baseUrl + \"informacion-administrativa/albaranes/\"'><span>Albaranes</span></a></li>";
                vencimientos = "<li class='level0 nav-3'><a href='#' onclick='window.location = baseUrl + \"informacion-administrativa/vencimientos/\"'><span>Vencimientos</span></a></li>";
                pedidos = "<li class='level0 nav-4 last'><a href='#' onclick='window.location = baseUrl + \"informacion-administrativa/pedidos/\"'><span>Pedidos</span></a></li>";
                categorias.append(facturas).append(albaranes).append(vencimientos).append(pedidos);
                $('label, select').css('vertical-align', 'middle');
            });
        })(jQuery);
    </script>
    <style type="text/css">
        .detalle { padding: 5px; }
        #cboxWrapper{ border-radius: 5px; }
        #cboxLoadedContent { background-color: #ddd; padding: 15px;}
        dt { font-weight:bolder;  }
        dd { margin: 5px 15px; }
        #tbProductos { margin-top: 30px; width: 101%; }
        #tbProductos th { font-weight:bolder; border: 1px solid #808080; text-align: left; background-color: #bcbcbc; }
        #tbProductos td { border: 1px solid #bababa; text-align: left; }
        .total { font-size: 18px; font-weight: bold; }
    </style> 
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="parteCentral" runat="server">
    <article class="col-main">
        <div class="breadcrumbs">
            <ul>
                <li class="home">
                    <a href='<%=ResolveUrl("~/")%>' title="Ir a página de inicio">Inicio</a>
                    <span>/ </span>
                </li>
                <li class="product">
                    <strong><asp:Literal ID="ltNombreCategoria" runat="server" Visible="true" Text="" /></strong>
                </li>
            </ul>
        </div>
        <div class="sub_banner2">
            <div id="filtros">
                <div class="pager">
                    <div class="pager_left">
                    </div>
                    <div class="pager_right">
                        <div class="limiter">
                            <label style="margin-right:70px;">Rango de fechas </label>
                            <label>Desde: </label>
                            <input type="text" placeholder="<dd/mm/yyyy>" class="input-text" style="width: 90px;" />
                            <label>Hasta: </label>
                            <input type="text" placeholder="<dd/mm/yyyy>" class="input-text" style="width: 90px;" />&nbsp;
                            <label>Estado: </label>
                            <select>
                                <option value="1" selected="selected">Seleccione un valor</option>
                                <option value="2">Aceptado</option>
                                <option value="3">Anulado</option>
                                <option value="4">Entregado</option>
                                <option value="5">Pendiente</option>                                
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Label ID="lblContenido" runat="server" Visible="true" Text=""></asp:Label>
    </article>
</asp:Content>

