<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="vencimientos.aspx.cs" Inherits="B2Bv30.vencimientos" %>

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
            });
        })(jQuery);
    </script>
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
        <asp:Label ID="lblContenido" runat="server" Visible="true" Text=""></asp:Label>
    </article>
</asp:Content>