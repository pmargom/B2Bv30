<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHeaderTop.ascx.cs" Inherits="B2Bv30.UserControls.ucHeaderTop" %>

<header class="header-container">  
    <script type="text/javascript">
        (function ($) { 
            $(document).ready(function ()
            {            
                $("#idLogout").click(function () {                    
                    $.Comun.m_logout();
                });
            });
        })(jQuery);
    </script>
	<div class="header_top">
		<div class="tm_top_currency">

        <%--<select name="currency" title="Currency" onchange="setLocation(this.value)">
            <option value="http://magento-demo.net/MAG191/MAG080146/directory/currency/switch/currency/THB/uenc/aHR0cDovL21hZ2VudG8tZGVtby5uZXQvTUFHMTkxL01BRzA4MDE0Ni8,/">
        THB            </option>
            <option value="http://magento-demo.net/MAG191/MAG080146/directory/currency/switch/currency/TRY/uenc/aHR0cDovL21hZ2VudG8tZGVtby5uZXQvTUFHMTkxL01BRzA4MDE0Ni8,/">
        TRY            </option>
            <option value="http://magento-demo.net/MAG191/MAG080146/directory/currency/switch/currency/USD/uenc/aHR0cDovL21hZ2VudG8tZGVtby5uZXQvTUFHMTkxL01BRzA4MDE0Ni8,/" selected="selected">
        USD            </option>
        </select>
		    <label class="btn">Currency:</label>--%>
    </div>


 				<div class="form-language btn-group">
  <%--  
    <select id="select-language" title="Your Language" onchange="window.location.href=this.value">
                    <option value="http://magento-demo.net/MAG191/MAG080146/?___store=english_mag080146&amp;___from_store=english_mag080146" selected="selected">English</option>
                    <option value="index102b.html?___store=french_mag080146&amp;___from_store=english_mag080146">French</option>
                    <option value="index8afa.html?___store=german_mag080146&amp;___from_store=english_mag080146">German</option>
        </select>
	<label for="select-language" class="btn">Language:</label>--%>
</div>
				<div class="welcome-msg">
                    <asp:Literal ID="liWelcomelogin" runat="server" Text="Bienvenido usuario anónimo" />
                    <a href='#' id='idLogout'>salir</a>
				</div>			
		   </div> 
    <!-- End of Header_top -->
	<div class="header">
		<div class="header-bottom"> 
			<h1 class="logo"><strong>B2B</strong><a href="<%=ResolveUrl("~/")%>" title="Magento Commerce" class="logo">
                <img src='<%=ResolveUrl("~/skin/frontend/default/MAG080146/images/logo_NA.png")%>' alt="" /></a></h1>
			<div class="quick-access">
			    <%--<form id="search_mini_form" action="http://magento-demo.net/MAG191/MAG080146/catalogsearch/result/" method="get">--%>
<%--                <div class="form-search">
                    <label for="search">Search:</label>
                    <input id="search" type="text" name="q" value="" class="input-text" maxlength="128" />
                    <button type="submit" title="Search" class="button"><span><span>Search</span></span></button>
                    <div id="search_autocomplete" class="search-autocomplete"></div>
                    <script type="text/javascript">
                        //<![CDATA[
                        var searchForm = new Varien.searchForm('search_mini_form', 'search', 'Search entire store here...');
                        searchForm.initAutocomplete('catalogsearch/ajax/suggest/index.html', 'search_autocomplete');
                        //]]>
                    </script>
                </div>--%>
                <%--</form>--%>
			    <div class="header-cart">
                    <div class="block-cart btn-slide">
	                    <div class="cart-label">
	                        <div class="cart_mini_right">                                
                                <asp:Label ID="lblResumen" runat="server" Visible="true" Text="(0 artículos) - <span class='price'>0.00 €</span>"></asp:Label>                                
                                <div class="right_arrow"></div>
	                        </div>	
	                    </div>
	                    <div class="block-content" id="panel">
	                        <div class="top_arrow"></div>
		                    <div class="cart_topbg">
		                        <div class="main-cart" id="miniCartContainer">
						            <asp:Literal ID="ltMiniCart" runat="server" Visible="true" Text="<p>No tiene artículos en su cesta</p>" />
					            </div>
	                        </div>	
                        </div>
                    </div>
                </div>
			    <div class="tm_headerlinkmenu">
				    <div class="tm_headerlinks_inner">
                        <div class="headertoggle_img">&nbsp;</div>
				    </div>
				    <ul class="links">
                        <li class="first" ><a href="#" title="Perfil de usuario">Perfil de usuario</a></li>
                        <li ><a href="<%=ResolveUrl("~/informacion-administrativa/facturas")%>" title="Información administrativa">Información administrativa</a></li>
                        <li class="last"><a href="<%=ResolveUrl("~/login")%>" title="Acceder">Acceder</a></li>
                    </ul>
			    </div>
			</div>
		</div>
	</div>
</header>
