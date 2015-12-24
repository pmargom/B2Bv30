<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNavigation.ascx.cs" Inherits="B2Bv30.UserControls.ucNavigation" %>
<nav class="nav-container">
	<div class="nav-inner">
        <div id="advancedmenu">
            <div class="menu">
                <div class="parentMenu menu0 home_link">
                    <a href="<%=ResolveUrl("~/inicio")%>">
                        <span>Inicio</span>
                    </a>
                </div>
            </div>
            <div id="menu199" class="menu" onmouseover="megnorShowMenuPopup(this, 'popup199');" onmouseout="megnorHideMenuPopup(this, event, 'popup199', 'menu199')">
                <div class="parentMenu"><a href="accessories.html"><span>Llantas</span></a></div>
            </div>
            <div id="popup199" class="megnor-advanced-menu-popup" onmouseout="megnorHideMenuPopup(this, event, 'popup199', 'menu199')" onmouseover="megnorPopupOver(this, event, 'popup199', 'menu199')">
                <div class="megnor-advanced-menu-popup_inner">
                    <div class="block1">
                        <div class="column first odd">
                            <div class="itemMenu level1"><a class="itemMenuName level1" href="accessories/exterior.html"><span>Exterior Parts</span></a>
                                <div class="itemSubMenu level1">
                                    <div class="itemMenu level2">
                                        <a class="itemMenuName level2" href="accessories/exterior/hood-products.html">
                                            <span>Hood Products</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/exterior/wheel-accessories.html">
                                            <span>Wheel Accessories</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/exterior/trim-dress-up.html">
                                            <span>Trim Dress up</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/exterior/deer-alert.html">
                                            <span>Deer Alert</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="column even">
                            <div class="itemMenu level1">
                                <a class="itemMenuName level1" href="accessories/interior-parts.html">
                                    <span>Interior Parts</span>
                                </a>
                                <div class="itemSubMenu level1">
                                    <div class="itemMenu level2">
                                        <a class="itemMenuName level2" href="accessories/interior-parts/door-lock.html">
                                            <span>Door Lock</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/interior-parts/steering-wheel.html">
                                            <span>Steering Wheel</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/interior-parts/sun-heat-protection.html">
                                            <span>Sun/Heat Protection</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/interior-parts/mats-protection.html">
                                            <span>Mats Protection</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="column last odd">
                            <div class="itemMenu level1">
                                <a class="itemMenuName level1" href="accessories/lightings.html">
                                    <span>Lightings</span>
                                </a>
                                <div class="itemSubMenu level1">
                                    <div class="itemMenu level2">
                                        <a class="itemMenuName level2" href="accessories/lightings/accent-lighting.html">
                                            <span>Accent Lighting</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/lightings/led-lighting.html">
                                            <span>LED Lighting</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/lightings/fog-lighting.html">
                                            <span>Fog Lighting</span>
                                        </a>
                                        <a class="itemMenuName level2" href="accessories/lightings/trailer-lighting.html">
                                            <span>Trailer Lighting</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clearBoth"></div>
                    </div>
                    <div class="clearBoth"></div>
                    <div id="tm_advanced_menu_199" class="block2">
                        <a href="#"><img src='<%=ResolveUrl("~/skin/frontend/default/MAG080146/images/banners/advancemenu-banner.jpg")%>' alt="" /></a>
                    </div>
                </div>
            </div>                    
            <div id="menu200" class="menu" onmouseover="megnorShowMenuPopup(this, 'popup200');" onmouseout="megnorHideMenuPopup(this, event, 'popup200', 'menu200')">
                <div class="parentMenu">
                    <a href="tools-equipment.html">
                        <span>Neumáticos</span>
                    </a>
                </div>
            </div>
            <div id="popup200" class="megnor-advanced-menu-popup" onmouseout="megnorHideMenuPopup(this, event, 'popup200', 'menu200')" onmouseover="megnorPopupOver(this, event, 'popup200', 'menu200')">
                <div class="megnor-advanced-menu-popup_inner">
                    <div class="block1">
                        <div class="column first odd">
                            <div class="itemMenu level1">
                                <a class="itemMenuName level1" href="tools-equipment/tools.html">
                                    <span>Tools</span>
                                </a>
                                <div class="itemSubMenu level1">
                                    <div class="itemMenu level2">
                                        <a class="itemMenuName level2" href="tools-equipment/tools/hand-tools.html">
                                            <span>Hand Tools</span>
                                        </a>
                                        <a class="itemMenuName level2" href="tools-equipment/tools/air-tools.html">
                                            <span>Air Tools</span>
                                        </a>
                                        <a class="itemMenuName level2" href="tools-equipment/tools/power-tools.html">
                                            <span>Power Tools</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="column last even">
                            <div class="itemMenu level1">
                                <a class="itemMenuName level1" href="tools-equipment/system.html">
                                    <span>System</span>
                                </a>
                                <div class="itemSubMenu level1">
                                    <div class="itemMenu level2">
                                        <a class="itemMenuName level2" href="tools-equipment/system/break-system.html">
                                            <span>Break System</span>
                                        </a>
                                        <a class="itemMenuName level2" href="tools-equipment/system/fuel-system.html">
                                            <span>Fuel System</span>
                                        </a>
                                        <a class="itemMenuName level2" href="tools-equipment/system/intake-system.html">
                                            <span>Intake System</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearBoth"></div>
                </div>
            </div>
            <div id="menu201" class="menu">
                <div class="parentMenu">
                    <a href="performance.html">
                        <span>Moto</span>
                    </a>
                </div>
            </div>                    
            <div id="menu202" class="menu">
                <div class="parentMenu">
                    <a href="fluids-and-chemicals.html">
                        <span>Lubricantes</span>
                    </a>
                </div>
            </div>                    
            <div id="menu203" class="menu">
                <div class="parentMenu">
                    <a href="batteries.html">
                        <span>Otros</span>
                    </a>
                </div>
            </div>                
            <div class="clearBoth"></div>
        </div>
	    <!--- Code for responsive menu start --->
		<div class="nav-responsive" style="display:none;"><span>Menu</span><div class="expandable"></div></div>
		<div class="responsive_menu">
			<ul id="nav" class="advanced_nav">
				<li class="level0 nav-1 level-top first parent">
                    <a href="accessories.html" class="level-top">
                        <span>Accessories</span>
                    </a>
                    <ul class="level0">
                    <li class="level1 nav-1-1 first parent">
                        <a href="accessories/exterior.html">
                            <span>Exterior Parts</span>
                        </a>
                        <ul class="level1">
                            <li class="level2 nav-1-1-1 first">
                                <a href="accessories/exterior/hood-products.html">
                                    <span>Hood Products</span>
                                </a>
                            </li>
                            <li class="level2 nav-1-1-2">
                                <a href="accessories/exterior/wheel-accessories.html">
                                    <span>Wheel Accessories</span>
                                </a>
                            </li>
                            <li class="level2 nav-1-1-3">
                                <a href="accessories/exterior/trim-dress-up.html">
                                    <span>Trim Dress up</span>
                                </a>
                            </li>
                            <li class="level2 nav-1-1-4 last">
                                <a href="accessories/exterior/deer-alert.html">
                                    <span>Deer Alert</span>
                                </a>
                            </li>
                        </ul>
                    </li>
                    <li class="level1 nav-1-2 parent">
                        <a href="accessories/interior-parts.html">
                            <span>Interior Parts</span>
                        </a>
                        <ul class="level1">
                            <li class="level2 nav-1-2-5 first">
                                <a href="accessories/interior-parts/door-lock.html">
                                    <span>Door Lock</span>
                                </a>
                            </li>
                            <li class="level2 nav-1-2-6">
                                <a href="accessories/interior-parts/steering-wheel.html">
                                    <span>Steering Wheel</span>
                                </a>
                            </li>
                            <li class="level2 nav-1-2-7">
                                <a href="accessories/interior-parts/sun-heat-protection.html">
                                    <span>Sun/Heat Protection</span>
                                </a>
                            </li>
                            <li class="level2 nav-1-2-8 last">
                                <a href="accessories/interior-parts/mats-protection.html">
                                    <span>Mats Protection</span>
                                </a>
                            </li>
                        </ul>
                        </li>
                        <li class="level1 nav-1-3 last parent">
                            <a href="accessories/lightings.html">
                                <span>Lightings</span>
                            </a>
                            <ul class="level1">
                                <li class="level2 nav-1-3-9 first">
                                    <a href="accessories/lightings/accent-lighting.html">
                                        <span>Accent Lighting</span>
                                    </a>
                                </li>
                                <li class="level2 nav-1-3-10">
                                    <a href="accessories/lightings/led-lighting.html">
                                        <span>LED Lighting</span>
                                    </a>
                                </li>
                                <li class="level2 nav-1-3-11">
                                    <a href="accessories/lightings/fog-lighting.html">
                                        <span>Fog Lighting</span>
                                    </a>
                                </li>
                                <li class="level2 nav-1-3-12 last">
                                    <a href="accessories/lightings/trailer-lighting.html">
                                        <span>Trailer Lighting</span>
                                    </a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li class="level0 nav-2 level-top parent">
                    <a href="tools-equipment.html" class="level-top">
                        <span>Tools &amp; Equipment</span>
                    </a>
                    <ul class="level0">
                        <li class="level1 nav-2-1 first parent">
                            <a href="tools-equipment/tools.html">
                                <span>Tools</span>
                            </a>
                            <ul class="level1">
                                <li class="level2 nav-2-1-1 first">
                                    <a href="tools-equipment/tools/hand-tools.html">
                                        <span>Hand Tools</span>
                                    </a>
                                </li>
                                <li class="level2 nav-2-1-2">
                                    <a href="tools-equipment/tools/air-tools.html">
                                        <span>Air Tools</span>
                                    </a>
                                </li>
                                <li class="level2 nav-2-1-3 last">
                                    <a href="tools-equipment/tools/power-tools.html">
                                     <span>Power Tools</span>
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <li class="level1 nav-2-2 last parent">
                            <a href="tools-equipment/system.html">
                                <span>System</span>
                            </a>
                            <ul class="level1">
                                <li class="level2 nav-2-2-4 first">
                                    <a href="tools-equipment/system/break-system.html">
                                        <span>Break System</span>
                                    </a>
                                </li>
                                <li class="level2 nav-2-2-5">
                                    <a href="tools-equipment/system/fuel-system.html">
                                        <span>Fuel System</span>
                                    </a>
                                </li>
                                <li class="level2 nav-2-2-6 last">
                                    <a href="tools-equipment/system/intake-system.html">
                                        <span>Intake System</span>
                                    </a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li class="level0 nav-3 level-top">
                    <a href="performance.html" class="level-top">
                        <span>Performance</span>
                    </a>
                </li>
                <li class="level0 nav-4 level-top">
                    <a href="fluids-and-chemicals.html" class="level-top">
                        <span>Fluids and Chemicals</span>
                    </a>
                </li>
                <li class="level0 nav-5 level-top last">
                    <a href="batteries.html" class="level-top">
                        <span>Batteries</span>
                    </a>
                </li>
			</ul>
		</div>
	</div>	
	<!--- Code for responsive menu end --->
</nav>
<script type="text/javascript">
    //<![CDATA[
    var CUSTOMMENU_POPUP_WIDTH = 0;
    var CUSTOMMENU_POPUP_TOP_OFFSET = 0;
    var CUSTOMMENU_POPUP_RIGHT_OFFSET_MIN = 0;
    var CUSTOMMENU_POPUP_DELAY_BEFORE_DISPLAYING = 0;
    var CUSTOMMENU_POPUP_DELAY_BEFORE_HIDING = 0;
    var megnorCustommenuTimerShow = {};
    var megnorCustommenuTimerHide = {};
    //]]>
</script>
