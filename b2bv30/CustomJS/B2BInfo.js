(function ($) {
    jQuery.extend({
        B2BInfo: {
            m_facturas: function (tipo) {
                //var urlPost = $.General.m_urlBase + 'cesta/';               
                var urlPost = baseUrl + 'informacion-administrativa/';
                alert(urlPost);
                $.post(urlPost,
                {
                    tipo: 'factura'
                },
                function (data) {
                    var datos = "";
                    var indice = data.indexOf("<!DOCTYPE");
                    if (indice >= 0) datos = data.substr(0, indice - 4);
                    else datos = data;

                    if (datos == "") {
                        alert("Se ha producido un error al obtener los datos");
                    }
                    else {
                        $('#parteCentral_lblContenido').html(datos);
                    }
                });
            },
            m_albaranes: function (tipo) {

            },
            m_vencimientos: function (tipo) {

            },
            m_pedidos: function (tipo) {

            }
        }

    });
});