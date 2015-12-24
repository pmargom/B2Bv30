<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="B2Bv30.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .pager { font-size: 90%; }
        #divMarcas div.sbHolder { width: 100px; z-index: 10; margin-left:-45px;}
        #nFilas  { z-index: 9999;}
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="parteCentral" runat="server">

    <article class="col-main">
        <div class="std2">
            <div class="flexslider">
                <ul class="slides">
                    <li><a href="#">
                        <img src='<%=ResolveUrl("~/skin/frontend/default/MAG080146/images/banners/main-banner1.png")%>' alt="" /></a></li>
                    <li><a href="#">
                        <img src='<%=ResolveUrl("~/skin/frontend/default/MAG080146/images/banners/main-banner2.png")%>' alt="" /></a></li>
                    <li><a href="#">
                        <img src='<%=ResolveUrl("~/skin/frontend/default/MAG080146/images/banners/main-banner3.png")%>' alt="" /></a></li>
                </ul>
            </div>
            <div class="sub_banner2">
                <div id="filtros">
                    <asp:Literal ID="ltFiltros" runat="server" Text="" Visible="true" />
                </div>
            </div>
            <div class="featured-products">
                <div class="category-title">
                    <h2><a href="featured-products.html">Productos destacados</a></h2>
                </div>
                <div id="contenido">
                    <ul class="products-grid" id="featured-grid">
                        <asp:Literal ID="ltProductos" runat="server" Text="" Visible="true" />
                    </ul>
                </div>
            </div>
            <span class="featured_default_width" style="display: none; visibility: hidden"></span>

            <%--    <div class="block_sub_banner">
        <div class="Subbanner1_container">
        <a href="#">
        <div class="Subbanner1">
        <h1>Envío gratuíto</h1>
        On Order Over <strong>$30</strong> Offer today</div>
        </a>
        </div>
        <div class="Subbanner2_container">
        <a href="#">
        <div class="Subbanner2">
        <h1>Give Us Call</h1>
        1 - 800 - Diapers (1-800-342-7377)</div>
        </a>
        </div>
    </div>--%>
        </div>
    </article>
</asp:Content>