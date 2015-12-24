<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCategorias.ascx.cs" Inherits="B2Bv30.UserControls.ucCategorias" %>
<script type="text/javascript">
    //<![CDATA[
    function toggleMenu(el, over) {
        if (over) {
            Element.addClassName(el, 'over');
        }
        else {
            Element.removeClassName(el, 'over');
        }
    }
    //]]>
</script>

<!-- ================== Tree View ================ -->
<div class="block block-side-nav-container">    
    <div class="block-title">
        <strong><span>Categorías</span></strong>
    </div>
	<div class="block-content">
		<div class="side-nav">
            <ul id="category-treeview" class="treeview-side treeview">
                <li class="level0 nav-1 parent" onmouseover="toggleMenu(this,1)" onmouseout="toggleMenu(this,0)">
                    <a href="accessories.html"><span>Llantas</span></a>
                    <ul class="level0">
                        <li class="level1 nav-1-1 first parent" onmouseover="toggleMenu(this,1)" onmouseout="toggleMenu(this,0)">
                            <a href="accessories/exterior.html"><span>Exterior Parts</span></a>
                            <ul class="level1">
                                <li class="level2 nav-1-1-1 first">
                                    <a href="accessories/exterior/hood-products.html"><span>Hood Products</span></a>
                                </li>
                                <li class="level2 nav-1-1-2">
                                    <a href="accessories/exterior/wheel-accessories.html"><span>Wheel Accessories</span></a>
                                </li>
                                <li class="level2 nav-1-1-3">
                                    <a href="accessories/exterior/trim-dress-up.html"><span>Trim Dress up</span></a>
                                </li>
                                <li class="level2 nav-1-1-4 last">
                                    <a href="accessories/exterior/deer-alert.html"><span>Deer Alert</span></a>
                                </li>
                            </ul>
                        </li>
                        <li class="level1 nav-1-2 parent" onmouseover="toggleMenu(this,1)" onmouseout="toggleMenu(this,0)">
                            <a href="accessories/interior-parts.html"><span>Interior Parts</span></a>
                            <ul class="level1">
                                <li class="level2 nav-1-2-5 first">
                                    <a href="accessories/interior-parts/door-lock.html"><span>Door Lock</span></a>
                                </li>
                                <li class="level2 nav-1-2-6">
                                    <a href="accessories/interior-parts/steering-wheel.html"><span>Steering Wheel</span></a>
                                </li>
                                <li class="level2 nav-1-2-7">
                                    <a href="accessories/interior-parts/sun-heat-protection.html"><span>Sun/Heat Protection</span></a>
                                </li>
                                <li class="level2 nav-1-2-8 last">
                                    <a href="accessories/interior-parts/mats-protection.html"><span>Mats Protection</span></a>
                                </li>
                            </ul>
                        </li>
                        <li class="level1 nav-1-3 last parent" onmouseover="toggleMenu(this,1)" onmouseout="toggleMenu(this,0)">
                            <a href="accessories/lightings.html"><span>Lightings</span></a>
                            <ul class="level1">
                                <li class="level2 nav-1-3-9 first">
                                    <a href="accessories/lightings/accent-lighting.html"><span>Accent Lighting</span></a>
                                </li>
                                <li class="level2 nav-1-3-10">
                                    <a href="accessories/lightings/led-lighting.html"><span>LED Lighting</span></a>
                                </li>
                                <li class="level2 nav-1-3-11">
                                    <a href="accessories/lightings/fog-lighting.html"><span>Fog Lighting</span></a>
                                </li>
                                <li class="level2 nav-1-3-12 last">
                                    <a href="accessories/lightings/trailer-lighting.html"><span>Trailer Lighting</span></a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>                    
                <li class="level0 nav-2 parent" onmouseover="toggleMenu(this,1)" onmouseout="toggleMenu(this,0)">
                    <a href="tools-equipment.html"><span>Neumáticos</span></a>
                    <ul class="level0">
                        <li class="level1 nav-2-1 first parent" onmouseover="toggleMenu(this,1)" onmouseout="toggleMenu(this,0)">
                            <a href="tools-equipment/tools.html"><span>Turismo</span></a>
                            <ul class="level1">
                                <li class="level2 nav-2-1-1 first">
                                    <a href="tools-equipment/tools/hand-tools.html"><span>Hand Tools</span></a>
                                </li>
                                <li class="level2 nav-2-1-2">
                                    <a href="tools-equipment/tools/air-tools.html"><span>Air Tools</span></a>
                                </li>
                                <li class="level2 nav-2-1-3 last">
                                    <a href="tools-equipment/tools/power-tools.html"><span>Power Tools</span></a>
                                </li>
                            </ul>
                        </li>
                        <li class="level1 nav-2-2 last parent" onmouseover="toggleMenu(this,1)" onmouseout="toggleMenu(this,0)">
                            <a href="tools-equipment/system.html"><span>4x4</span></a>
                            <ul class="level1">
                                <li class="level2 nav-2-2-4 first">
                                    <a href="tools-equipment/system/break-system.html"><span>Break System</span></a>
                                </li>
                                <li class="level2 nav-2-2-5">
                                    <a href="tools-equipment/system/fuel-system.html"><span>Fuel System</span></a>
                                </li>
                                <li class="level2 nav-2-2-6 last">
                                    <a href="tools-equipment/system/intake-system.html"><span>Intake System</span></a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li class="level0 nav-3">
                    <a href="performance.html"><span>Moto</span></a>
                </li>
                <li class="level0 nav-4">
                    <a href="fluids-and-chemicals.html"><span>Lubricantes</span></a>
                </li>
                <li class="level0 nav-5">
                    <a href="batteries.html"><span>Otros</span></a>
                </li>
            </ul>
	    </div>
	</div>
</div> 