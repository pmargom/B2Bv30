<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="detalleProducto.aspx.cs" Inherits="B2Bv30.detalleProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                var elemento = $("#upsell-carousel li");
                elemento.css("width", "190px");
                elemento.removeClass("first_item_tm");
                elemento.removeClass("last_item_tm");
            });
        })(jQuery);
    </script>
    <script type="text/javascript">
        function esNumero(e) {
            k = (document.all) ? e.keyCode : e.which;
            if (k == 8 || k == 0) return true;
            patron = /\d/;
            n = String.fromCharCode(k);
            return patron.test(n);
        }
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
                    <strong><asp:Literal ID="ltNombreProducto" runat="server" Visible="true" Text="" /></strong>
                </li>
            </ul>
        </div>
        <%--<script type="text/javascript">
            var optionsPrice = new Product.OptionsPrice([]);
        </script>--%>
        <div id="messages_product_view"></div>
        <div class="product-view">
            <div class="product-essential">
                <%--<form action="http://magento-demo.net/MAG191/MAG080146/index.php/checkout/cart/add/uenc/aHR0cDovL21hZ2VudG8tZGVtby5uZXQvTUFHMTkxL01BRzA4MDE0Ni9pbmRleC5waHAvYXZhbnRlLXhkLWVsYW50cmEtMTU1Lmh0bWw,/product/127/form_key/ov9jqKmNkSk8YWFV/" method="post" id="product_addtocart_form">
                    <input name="form_key" value="ov9jqKmNkSk8YWFV" type="hidden">
                    <div class="no-display">
                        <input name="product" value="127" type="hidden">
                        <input name="related_product" id="related-products-field" value="" type="hidden">
                    </div>--%>
                    <div class="product-img-box">
                        <div class="product-image">
                            <div id="wrap" style="top: 0px; z-index: 99; position: relative;">
                                <asp:Literal ID="ltImagen" runat="server" Visible="true" Text="" />
                                <%--<a style="position: relative; display: block;" href="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/600x600/9df78eab33525d08d6e5fb8d27136e95/2/3/23_4.jpg" rel="" title="Sell Tubeless" id="zoom1" class="cloud-zoom">
                                    <img style="display: block;" src="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/300x300/9df78eab33525d08d6e5fb8d27136e95/2/3/23_4.jpg" alt="Sell Tubeless" title="Sell Tubeless">
                                </a>--%>
                                <div class="mousetrap" style="background: none repeat scroll 0% 0% rgb(255, 255, 255); opacity: 0; z-index: 99; position: absolute; width: 300px; height: 300px; left: 0px; top: 0px; cursor: move;"></div>
                            </div>
                            <asp:Literal ID="ltBotonImagen" runat="server" Visible="true" Text="" />
                        </div>
                            <%--<a id="zoom-btn" class="lightbox-group zoom-btn-small cboxElement" href="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/600x600/9df78eab33525d08d6e5fb8d27136e95/2/3/23_4.jpg" title="">Zoom</a>--%>
                        
  <%--                       <div class="more-views additional-carousel">
                           <div class="customNavigation">
                                <a class="btn prev">&nbsp;</a>
                                <a class="btn next">&nbsp;</a>
                            </div>
                            <div style="opacity: 1; display: block;" id="additional-carousel" class="product-carousel">
                                <div class="slider-wrapper-outer">
                                    <div style="width: 910px; left: 0px; display: block; transition: all 1000ms ease 0s; transform: translate3d(0px, 0px, 0px);" class="slider-wrapper">
                                        <div style="width: 91px;" class="slider-item first_item_tm">
                                            <div style="height: 74px;" class="product-block">
                                                <asp:Literal ID="ltMiniatura" runat="server" Visible="true" Text="" />
                                                <%--<a href="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/600x600/9df78eab33525d08d6e5fb8d27136e95/2/3/23_4.jpg" class="cloud-zoom-gallery lightbox-group" title="" rel="useZoom: 'zoom1', smallImage: 'http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/300x300/9df78eab33525d08d6e5fb8d27136e95/2/3/23_4.jpg' ">
                                                    <img src="http://magento-demo.net/MAG191/media/catalog/product/cache/20/thumbnail/74x/9df78eab33525d08d6e5fb8d27136e95/2/3/23_4.jpg" alt="" height="74" width="74">
                                                </a>--%>
<%--                                            </div>
                                        </div>
                                        <div style="width: 91px;" class="slider-item">
                                            <div style="height: 74px;" class="product-block">
                                                <a href="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/600x600/9df78eab33525d08d6e5fb8d27136e95/2/0/20_5.jpg" class="cloud-zoom-gallery lightbox-group cboxElement" title="" rel="useZoom: 'zoom1', smallImage: 'http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/300x300/9df78eab33525d08d6e5fb8d27136e95/2/0/20_5.jpg' ">
                                                    <img src="http://magento-demo.net/MAG191/media/catalog/product/cache/20/thumbnail/74x/9df78eab33525d08d6e5fb8d27136e95/2/0/20_5.jpg" alt="" height="74" width="74">
                                                </a>
                                            </div>
                                        </div>
                                        <div style="width: 91px;" class="slider-item last_item_tm">
                                            <div style="height: 74px;" class="product-block">
                                                <a href="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/600x600/9df78eab33525d08d6e5fb8d27136e95/2/4/24_3.jpg" class="cloud-zoom-gallery lightbox-group cboxElement" title="" rel="useZoom: 'zoom1', smallImage: 'http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/300x300/9df78eab33525d08d6e5fb8d27136e95/2/4/24_3.jpg' ">
                                                    <img src="http://magento-demo.net/MAG191/media/catalog/product/cache/20/thumbnail/74x/9df78eab33525d08d6e5fb8d27136e95/2/4/24_3.jpg" alt="" height="74" width="74">
                                                </a>
                                            </div>
                                        </div>
                                        <div style="width: 91px;" class="slider-item first_item_tm">
                                            <div style="height: 74px;" class="product-block">
                                                <a href="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/600x600/9df78eab33525d08d6e5fb8d27136e95/2/2/22_4.jpg" class="cloud-zoom-gallery lightbox-group cboxElement" title="" rel="useZoom: 'zoom1', smallImage: 'http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/300x300/9df78eab33525d08d6e5fb8d27136e95/2/2/22_4.jpg' ">
                                                    <img src="http://magento-demo.net/MAG191/media/catalog/product/cache/20/thumbnail/74x/9df78eab33525d08d6e5fb8d27136e95/2/2/22_4.jpg" alt="" height="74" width="74">
                                                </a>
                                            </div>
                                        </div>
                                        <div style="width: 91px;" class="slider-item">
                                            <div style="height: 74px;" class="product-block">
                                                <a href="http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/600x600/9df78eab33525d08d6e5fb8d27136e95/2/1/21_2.jpg" class="cloud-zoom-gallery lightbox-group cboxElement" title="" rel="useZoom: 'zoom1', smallImage: 'http://magento-demo.net/MAG191/media/catalog/product/cache/20/image/300x300/9df78eab33525d08d6e5fb8d27136e95/2/1/21_2.jpg' ">
                                                    <img src="http://magento-demo.net/MAG191/media/catalog/product/cache/20/thumbnail/74x/9df78eab33525d08d6e5fb8d27136e95/2/1/21_2.jpg" alt="" height="74" width="74">
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                            <%--<span class="additional_default_width" style="display: none; visibility: hidden;"></span>--%>
                            <script type="text/javascript">
                                jQuery(function ($) {
                                    $(".lightbox-group").colorbox({
                                        rel: 'lightbox-group',
                                        opacity: 0.5,
                                        speed: 300
                                    });
                                    $(".cloud-zoom-gallery").first().removeClass("cboxElement");
                                    $(".cloud-zoom-gallery").click(function () {
                                        $("#zoom-btn").attr('href', $(this).attr('href'));
                                        $("#zoom-btn").attr('title', $(this).attr('title'));

                                        $(".cloud-zoom-gallery").each(function () {
                                            $(this).addClass("cboxElement");
                                        });
                                        $(this).removeClass("cboxElement");
                                    });
                                });
                                jQuery(function ($) {
                                    var t; $(window).resize(function () { clearTimeout(t); t = setTimeout(function () { $(".cloud-zoom-gallery").first().click(); }, 200); });
                                });
                            </script>
                        <%--</div>--%>
                    </div>
                    <div class="product-shop">
                        <div class="product-name">
                            <h1><asp:Literal ID="ltNombreProducto2" runat="server" Visible="true" Text="" /></h1>
                            <!--############################ PREV NEXT OUTPUT ##################################-->
                            <div id="prev-next-links">
                                <!--Previous Item Link-->
                                <asp:Literal ID="ltEnlacePrevio" runat="server" Visible="true" Text="" />
                                <%--<a id="link-previous-product" href="#">&nbsp;</a>--%>
                                <!--Next Item Link-->
                                <asp:Literal ID="ltEnlacePosterior" runat="server" Visible="true" Text="" />
                                <%--<a id="link-next-product" href="#">&nbsp;</a>--%>
                            </div>
                            <!--############################ END PREV NEXT OUTPUT ##################################-->
                        </div>
                        <%--<script type="text/javascript">
                            //<![CDATA[
                            var review_tab_show = function () {
                                var ul = $('product_tabs_review_tabbed').parentNode;
                                var li = $('product_tabs_review_tabbed');
                                ul.select('li', 'ol').each(function (el) {
                                    var contents = $(el.id + '_contents');
                                    if (el == li) {
                                        el.addClassName('active');
                                        contents.show();
                                    } else {
                                        el.removeClassName('active');
                                        contents.hide();
                                    }
                                });
                            };
                            //]]>
                        </script>
                        <p class="no-rating"><a rel="nofollow" onclick="review_tab_show()" href="#product_tabs_review_tabbed">Be the first to review this product</a></p>--%>

                        <div class="sku"><span>Referencia: </span><asp:Literal ID="ltCodigo" runat="server" Visible="true" Text="" /></div>

                        <div class="short-description">
                            <!--  <h2></h2>-->
                            <div class="std"><asp:Literal ID="ltNombreProducto3" runat="server" Visible="true" Text="" /></div>
                        </div>
                        <p class="availability in-stock">Disponibilidad: <span><asp:Literal ID="ltEnStock" runat="server" Visible="true" Text="" /></span></p>
                        <div class="price-box">
                            <%--<span class="price old-price" id="old-price-127">$500.00</span>--%>
                            <span class="price special-price" id="product-price-127"><asp:Literal ID="ltPrecio" runat="server" Visible="true" Text="" /></span>
                        </div>
                        <div class="clearer"></div>
                        <div class="add-to-box">
                            <div class="add-to-cart">
                                <label for="qty">Cantidad:</label>
                                <input name="qty" id="qty" maxlength="12" value="1" title="Cantidad" class="input-text qty" type="text" onkeypress="return esNumero(event);" />
                                <asp:Literal ID="ltAddtoCart" runat="server" Visible="true" Text="" />
                                <%--<button type="button" title="Añadir a la cesta" class="button btn-cart" onclick="productAddToCartForm.submit(this)"><span><span>Añadir a la cesta</span></span></button>--%>
                            </div>
                        </div>
                        <%--<div class="email-addto-box">
                            <p class="email-friend"><a href="http://magento-demo.net/MAG191/MAG080146/index.php/sendfriend/product/send/id/127/">Email to a Friend</a></p>
                            <ul class="add-to-links">
                                <li><a href="http://magento-demo.net/MAG191/MAG080146/index.php/wishlist/index/add/product/127/form_key/ov9jqKmNkSk8YWFV/" onclick="productAddToCartForm.submitLight(this, this.href); return false;" class="link-wishlist">Add to Wishlist</a></li>
                                <li><span class="separator">|</span> <a href="http://magento-demo.net/MAG191/MAG080146/index.php/catalog/product_compare/add/product/127/uenc/aHR0cDovL21hZ2VudG8tZGVtby5uZXQvTUFHMTkxL01BRzA4MDE0Ni9pbmRleC5waHAvYXZhbnRlLXhkLWVsYW50cmEtMTU1Lmh0bWw,/form_key/ov9jqKmNkSk8YWFV/" class="link-compare">Add to Compare</a></li>
                            </ul>
                        </div>--%>
                    </div>
                    <%--<div class="info3col-data">
                        <div class="custom_block">
                        </div>
                    </div>
                </form>
                <script type="text/javascript">
                    //<![CDATA[
                    var productAddToCartForm = new VarienForm('product_addtocart_form');
                    productAddToCartForm.submit = function (button, url) {
                        if (this.validator.validate()) {
                            var form = this.form;
                            var oldUrl = form.action;

                            if (url) {
                                form.action = url;
                            }
                            var e = null;
                            try {
                                this.form.submit();
                            } catch (e) {
                            }
                            this.form.action = oldUrl;
                            if (e) {
                                throw e;
                            }

                            if (button && button != 'undefined') {
                                button.disabled = true;
                            }
                        }
                    }.bind(productAddToCartForm);

                    productAddToCartForm.submitLight = function (button, url) {
                        if (this.validator) {
                            var nv = Validation.methods;
                            delete Validation.methods['required-entry'];
                            delete Validation.methods['validate-one-required'];
                            delete Validation.methods['validate-one-required-by-name'];
                            // Remove custom datetime validators
                            for (var methodName in Validation.methods) {
                                if (methodName.match(/^validate-datetime-.*/i)) {
                                    delete Validation.methods[methodName];
                                }
                            }

                            if (this.validator.validate()) {
                                if (url) {
                                    this.form.action = url;
                                }
                                this.form.submit();
                            }
                            Object.extend(Validation.methods, nv);
                        }
                    }.bind(productAddToCartForm);
                    //]]>
    </script>--%>
            </div>

            <div class="product-collateral">

                <ul class="tabs">
                    <li id="product_tabs_description_tabbed" class="active first"><a href="javascript:void(0)">Descripción</a></li>
                    <%--<li class="" id="product_tabs_review_tabbed"><a href="javascript:void(0)">Product's Review</a></li>
                    <li class="" id="product_tabs_tags_tabbed"><a href="javascript:void(0)">Product Tags</a></li>--%>
                </ul>
                <div class="padder">
                    <div id="product_tabs_description_tabbed_contents">
                        <h6>Descripción</h6>
                        <ol>
                            <h2>Detalles</h2>
                            <div class="std">
                                <asp:Literal ID="ltDetalles" runat="server" Visible="true" Text="" />   
                            </div>
                        </ol>
                    </div>
<%--                    <div style="display: none;" id="product_tabs_review_tabbed_contents">
                        <h6>Product's Review</h6>
                        <ol>

                            <br>

                            <div class="form-add">
                                <h2>Write Your Own Review</h2>
                                <form action="http://magento-demo.net/MAG191/MAG080146/index.php/review/product/post/id/127/" method="post" id="review-form">
                                    <input name="form_key" value="ov9jqKmNkSk8YWFV" type="hidden">
                                    <fieldset>
                                        <h3>You're reviewing: <span>Sell Tubeless</span></h3>
                                        <h4>How do you rate this product? <em class="required">*</em></h4>
                                        <span id="input-message-box"></span>
                                        <table class="data-table" id="product-review-table">
                                            <colgroup>
                                                <col>
                                                <col width="1">
                                                <col width="1">
                                                <col width="1">
                                                <col width="1">
                                                <col width="1">
                                            </colgroup>
                                            <thead>
                                                <tr class="first last">
                                                    <th>&nbsp;</th>
                                                    <th><span class="nobr">1 star</span></th>
                                                    <th><span class="nobr">2 stars</span></th>
                                                    <th><span class="nobr">3 stars</span></th>
                                                    <th><span class="nobr">4 stars</span></th>
                                                    <th><span class="nobr">5 stars</span></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr class="first last odd">
                                                    <th>Price</th>
                                                    <td class="value">
                                                        <div class="tm-radio">
                                                            <input name="ratings[3]" id="Price_1" value="11" class="radio tm-hide" type="radio"></div>
                                                    </td>
                                                    <td class="value">
                                                        <div class="tm-radio">
                                                            <input name="ratings[3]" id="Price_2" value="12" class="radio tm-hide" type="radio"></div>
                                                    </td>
                                                    <td class="value">
                                                        <div class="tm-radio">
                                                            <input name="ratings[3]" id="Price_3" value="13" class="radio tm-hide" type="radio"></div>
                                                    </td>
                                                    <td class="value">
                                                        <div class="tm-radio">
                                                            <input name="ratings[3]" id="Price_4" value="14" class="radio tm-hide" type="radio"></div>
                                                    </td>
                                                    <td class="value last">
                                                        <div class="tm-radio">
                                                            <input name="ratings[3]" id="Price_5" value="15" class="radio tm-hide" type="radio"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <input name="validate_rating" class="validate-rating" value="" type="hidden">
                                        <script type="text/javascript">decorateTable('product-review-table')</script>
                                        <ul class="form-list">
                                            <li>
                                                <label for="nickname_field" class="required"><em>*</em>Nickname</label>
                                                <div class="input-box">
                                                    <input name="nickname" id="nickname_field" class="input-text required-entry" value="" type="text">
                                                </div>
                                            </li>
                                            <li>
                                                <label for="summary_field" class="required"><em>*</em>Summary of Your Review</label>
                                                <div class="input-box">
                                                    <input name="title" id="summary_field" class="input-text required-entry" value="" type="text">
                                                </div>
                                            </li>
                                            <li>
                                                <label for="review_field" class="required"><em>*</em>Review</label>
                                                <div class="input-box">
                                                    <textarea name="detail" id="review_field" cols="5" rows="3" class="required-entry"></textarea>
                                                </div>
                                            </li>
                                        </ul>
                                    </fieldset>
                                    <div class="buttons-set">
                                        <button type="submit" title="Submit Review" class="button"><span><span>Submit Review</span></span></button>
                                    </div>
                                </form>
                                <script type="text/javascript">
                                    //<![CDATA[
                                    var dataForm = new VarienForm('review-form');
                                    Validation.addAllThese(
                                    [
                                           ['validate-rating', 'Please select one of each of the ratings above', function (v) {
                                               var trs = $('product-review-table').select('tr');
                                               var inputs;
                                               var error = 1;

                                               for (var j = 0; j < trs.length; j++) {
                                                   var tr = trs[j];
                                                   if (j > 0) {
                                                       inputs = tr.select('input');

                                                       for (i in inputs) {
                                                           if (inputs[i].checked == true) {
                                                               error = 0;
                                                           }
                                                       }

                                                       if (error == 1) {
                                                           return false;
                                                       } else {
                                                           error = 1;
                                                       }
                                                   }
                                               }
                                               return true;
                                           }]
                                    ]
                                    );
                                    //]]>
    </script>
                            </div>


                        </ol>
                    </div>

                    <div style="display: none;" id="product_tabs_tags_tabbed_contents">
                        <h6>Product Tags</h6>
                        <ol>
                            <div class="box-collateral box-tags">
                                <form id="addTagForm" action="http://magento-demo.net/MAG191/MAG080146/index.php/tag/index/save/product/127/uenc/aHR0cDovL21hZ2VudG8tZGVtby5uZXQvTUFHMTkxL01BRzA4MDE0Ni9pbmRleC5waHAvYXZhbnRlLXhkLWVsYW50cmEtMTU1Lmh0bWw,/" method="get">
                                    <div class="form-add">
                                        <label for="productTagName">Add Your Tags:</label>
                                        <div class="input-box">
                                            <input class="input-text required-entry" name="productTagName" id="productTagName" type="text">
                                        </div>
                                        <button type="button" title="Add Tags" class="button" onclick="submitTagForm()"><span><span>Add Tags</span></span></button>
                                    </div>
                                </form>
                                <p class="note">Use spaces to separate tags. Use single quotes (') for phrases.</p>
                                <script type="text/javascript">
                                    //<![CDATA[
                                    var addTagFormJs = new VarienForm('addTagForm');
                                    function submitTagForm() {
                                        if (addTagFormJs.validator.validate()) {
                                            addTagFormJs.form.submit();
                                        }
                                    }
                                    //]]>
    </script>
                            </div>
                        </ol>
                    </div>

                    <div id="product_tabs_cms_contents">
                        <h6>CMS tab</h6>
                        <ol>
                            <div class="product-specs"></div>
                        </ol>
                    </div>--%>
                </div>
                <%--<script type="text/javascript">
                    var MegnorTabs = Class.create();
                    MegnorTabs.prototype = {
                        initialize: function (selector) {
                            $$(selector).each(this.initTab.bind(this));
                        },

                        initTab: function (el) {
                            el.href = 'javascript:void(0)';
                            if ($(el).up('li').hasClassName('active')) {
                                this.showContent(el);
                            }
                            el.observe('click', this.showContent.bind(this, el));
                        },

                        showContent: function (a) {
                            var li = $(a).up('li'),
                                ul = $(li).up('ul');

                            ul.select('li'/*, 'ol'*/).each(function (el) {
                                var contents = $(el.id + '_contents');
                                if (el.id == li.id) {
                                    el.addClassName('active');
                                    contents.show();
                                } else {
                                    el.removeClassName('active');
                                    contents.hide();
                                }
                            });
                        }
                    }
                    new MegnorTabs('.tabs a');
                </script>--%>
            </div>
        </div>
        <div class="box-collateral box-up-sell">
            <div class="category-title">
                <h2>También le puede interesar...</h2>
            </div>
            <div class="products-grid" id="upsell-product-table">
                <%--<div class="customNavigation">
                    <a class="btn prev">&nbsp;</a>
                    <a class="btn next">&nbsp;</a>
                </div>--%>
                <ul class="product-carousel" id="upsell-carousel" style="opacity: 1; display: block;">
                    <div class="slider-wrapper-outer">
                        <div class="slider-wrapper" style="width: 4940px; left: 0px; display: block;">
                            <asp:Literal ID="ltProductosInf" runat="server" Visible="true" Text="" />
                            <%--<li class="item slider-item first_item_tm" style="width: 190px;">
                                <div class="product-block" style="height: 234px;">
                                    <div class="product-block-inner">
                                        <a href="http://magento-demo.net/MAG191/MAG080146/index.php/avante-xd-elantra-145.html" title="Steering Gear Box" class="product-image">
                                            <img src="http://magento-demo.net/MAG191/media/catalog/product/cache/20/small_image/155x155/9df78eab33525d08d6e5fb8d27136e95/2/_/2_1_14_3.jpg" width="155" height="155" alt="Steering Gear Box"></a>
                                        <h3 class="product-name"><a href="http://magento-demo.net/MAG191/MAG080146/index.php/avante-xd-elantra-145.html" title="Steering Gear Box">Steering Gear Box</a></h3>
                                        <div class="price-box">
                                            <span class="regular-price" id="product-price-117-upsell">
                                                <span class="price">$500.00</span></span>
                                        </div>
                                    </div>
                                </div>
                            </li> --%>                           
                        </div>
                    </div>
                </ul>
                <span style="display: none; visibility: hidden;" class="upsell_default_width"></span>
            </div>
        </div>
        <%--<script type="text/javascript">
            var lifetime = 3600;
            var expireAt = Mage.Cookies.expires;
            if (lifetime > 0) {
                expireAt = new Date();
                expireAt.setTime(expireAt.getTime() + lifetime * 1000);
            }
            Mage.Cookies.set('external_no_cache', 1, expireAt);
        </script>--%>
    </article>
</asp:Content>