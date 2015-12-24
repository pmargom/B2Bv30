(function ($) {
    jQuery.extend({
        Comun: {
            //m_showModal: function () {

            //    var winH = $(window).height();
            //    var winW = $(window).width();

            //    $('#progreso.modal').css('margin-left', '0%');
            //    $('#progreso.modal').css('height', (winH + 'px'));
            //    $('#progreso.modal').css('width', (winW + 'px'));
            //    $('#progreso.modal img').css('margin-top', (parseInt(winH / 2) + 'px'));

            //    $('#progreso.modal').css('display', 'block');
            //    //$('#progreso').modal('show');
            //    //$('#progreso').fadeIn(500);
            //},
            //m_closeModal: function () {
            //    //$('#progreso').modal('hide');
            //    $('#progreso.modal').css('display', 'none');
            //},
            m_login: function () {
                //alert($.trim($('#txtEmail').val()));
                //$('#error').hide();
                //$('#imloader').show();
                $('#txtEmail').prop("disabled", true);
                $('#txtPass').prop("disabled", true);
                $('#btnLogin').prop("disabled", true);
                //$('#mensajeerror').html("");
                var user = $('#txtEmail').val();
                var pass = $('#txtPass').val();

                if ($.trim(user) == '') {
                    mostrarError('Debe introducir un E-mail');
                    $('#txtEmail').focus();
                    return;
                }
                //if (!$.Util.m_isEmail(user)) {
                //    mostrarError('Debe introducir un E-mail válido');
                //    $('#txtEmail').focus();
                //    return;
                //}
                if ($.trim(pass) == '') {
                    mostrarError('Debe introducir una contraseña');
                    $('#txtPass').focus();
                    return;
                }
                //muestra el error dentro de la ventana de login
                function mostrarError(error) {
                    $('#error').show();
                    $('#mensajeerror').html(error);
                    $('#mensajeerror').css('color', 'red');
                    //$('#mensajeerror').css('font-weight', 'bold');
                    $('#imloader').hide();
                    $('#txtEmail').prop("disabled", false);
                    $('#txtPass').prop("disabled", false);
                    $('#btnLogin').prop("disabled", false);
                }

                //var urlPost = $.General.m_urlBase + 'funciones-comun/';
                var urlPost = baseUrl + 'funciones-comun/';
                //alert(urlPost);
                $.post(urlPost,
                {
                    Tarea: 'login',
                    user: user,
                    pass: pass
                },
                function (data) {
                    //$('#imloader').show();

                    var datos = "";
                    var indice = data.indexOf("<!DOCTYPE");
                    if (indice >= 0) datos = data.substr(0, indice - 4);
                    else datos = data;

                    if (datos == "ok") {                        
                        //window.location = $.General.m_urlBase + 'inicio/';
                        window.location = baseUrl + 'inicio/';
                    }
                    else {
                        mostrarError('Usuario inexistente o no encontrado');
                    }
                });
            },
            m_logout: function () {                
                //var urlPost = $.General.m_urlBase + 'funciones-comun/';               
                var urlPost = baseUrl + 'funciones-comun/';
                $.post(urlPost,
                {
                    Tarea: 'logout'
                },
                function (data) {
                    var datos = "";
                    var indice = data.indexOf("<!DOCTYPE");
                    if (indice >= 0) datos = data.substr(0, indice - 4);
                    else data = datos;

                    if (datos != "ok") {
                        alert("Se ha producido un error al salir de la sesión");
                    }
                });                
                //location = $.General.m_urlBase + 'login/';
                location = baseUrl + 'login/';
            }
        }
    });
})(jQuery);