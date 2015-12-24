<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLogin.ascx.cs" Inherits="B2Bv30.UserControls.ucLogin" %>
<article class="col-main"> 
    <script type="text/javascript">
        function entrar(e) {
            tecla = (document.all) ? e.keyCode : e.which;
            if (tecla == 13)
                $.Comun.m_login();
        }
    </script>
    <style type="text/css">
        .registered-users, .col-2 { margin: 0 auto !important; float: none !important; }
    </style>
    <div class="account-login">
        <div class="page-title">
            <h1>Acceda a su cuenta</h1>
        </div>
        <%--<form action="http://magento-demo.net/MAG191/MAG080146/index.php/customer/account/loginPost/" method="post" id="login-form">
        <input name="form_key" type="hidden" value="mBFoo5x2W2EWsFcn" />--%>
        <div class="col2-set">
<%--            <div class="col-1 new-users">
                <div class="content">
                    <h2>Nuevos clientes</h2>
                    <p>Al crear una cuenta en nuestra tienda, usted podrá realizar el proceso de compra más rápidamente, 
                        ver y hacer un seguimiento de sus pedidos en su cuenta y más.</p>
                </div>
            </div>--%>
            <div class="col-2 registered-users">
                <div class="content">
                    <h2>Clientes registrados</h2>
                    <p>Si tiene cuenta con nosotros, por favor acceda.</p>
                    <ul class="form-list">
                        <li>
                            <label for="txtEmail" class="required"><em>*</em>Nombre de usuario</label>
                            <div class="input-box">
                                <input type="text" name="txtEmail" value="" id="txtEmail" class="input-text required-entry validate-email" title="Email" />
                            </div>
                        </li>
                        <li>
                            <label for="txtPass" class="required"><em>*</em>Contraseña</label>
                            <div class="input-box">
                                <input type="password" 
                                       name="txtPass" 
                                       class="input-text required-entry validate-password" 
                                       id="txtPass" 
                                       title="Contraseña" 
                                       onkeypress="entrar(event);"/>
                            </div>
                        </li>
                    </ul>
                    <div style="clear:both;margin-top:10px;"><label style="margin-left: 0%" id="mensajeerror"></label></div>
                    <%--<div id="window-overlay" class="window-overlay" style="display:none;"></div>--%>
               <%--     <div id="remember-me-popup" class="remember-me-popup" style="display:none;">
                        <div class="remember-me-popup-head">
                            <h3>What's this?</h3>
                            <a href="#" class="remember-me-popup-close" title="Close">Close</a>
                        </div>
                        <div class="remember-me-popup-body">
                            <p>Checking &quot;Remember Me&quot; will let you access your shopping cart on this computer when you are logged out</p>
                            <div class="remember-me-popup-close-button a-right">
                                <a href="#" class="remember-me-popup-close button" title="Close"><span>Close</span></a>
                            </div>
                        </div>
                    </div>
                    <script type="text/javascript">
                        //<![CDATA[
                        function toggleRememberMepopup(event) {
                            if ($('remember-me-popup')) {
                                var viewportHeight = document.viewport.getHeight(),
                                    docHeight = $$('body')[0].getHeight(),
                                    height = docHeight > viewportHeight ? docHeight : viewportHeight;
                                $('remember-me-popup').toggle();
                                $('window-overlay').setStyle({ height: height + 'px' }).toggle();
                            }
                            Event.stop(event);
                        }

                        document.observe("dom:loaded", function () {
                            new Insertion.Bottom($$('body')[0], $('window-overlay'));
                            new Insertion.Bottom($$('body')[0], $('remember-me-popup'));

                            $$('.remember-me-popup-close').each(function (element) {
                                Event.observe(element, 'click', toggleRememberMepopup);
                            })
                            $$('#remember-me-box a').each(function (element) {
                                Event.observe(element, 'click', toggleRememberMepopup);
                            });
                        });
                        //]]>
                    </script>--%>
                    <p class="required">* Campos obligatorios</p>
                </div>
            </div>
        </div>
        <div class="col2-set">
<%--            <div class="col-1 new-users">
                <div class="buttons-set">
                    <button type="button" title="Crear una cuenta" class="button"><span><span>Crear una cuenta</span></span></button>
                </div>
            </div>--%>
            <div class="col-2 registered-users">
                <div class="buttons-set">
                    <a href="#" class="f-left">Recordar contraseña</a>
                    <button type="button" class="button" title="Acceder" name="btnLogin" id="btnLogin" onclick="$.Comun.m_login();"><span><span>Acceder</span></span></button>
                </div>
            </div>
        </div>
        <%--</form>--%>
        <script type="text/javascript">
            //<![CDATA[
            //var dataForm = new VarienForm('login-form', true);
            //]]>
        </script>
    </div>
</article>
