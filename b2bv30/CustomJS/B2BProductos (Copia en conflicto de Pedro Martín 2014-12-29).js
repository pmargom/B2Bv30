(function ($) {
    jQuery.extend({
        B2BProductos: {
            m_capturaFiltros: function (tipo, ordenarPor, ordenAscDesc, regPagina, pagina, presentacion, totalRegistros, medida, marca, modelo, neumatico, IC, IV) {
                $('#ordenarPor').val(ordenarPor);
                //debugger;
                if (ordenAscDesc == 1) {
                    //cambiamos la clase del elemento
                    if ($('#ordenAscDesc').attr('class') == 'downarrow') {
                        $('#ordenAscDesc').removeClass('downarrow').addClass('uparrow');
                        ordenAscDesc = "asc";
                    }
                    else {
                        $('#ordenAscDesc').removeClass('uparrow').addClass('downarrow');
                        ordenAscDesc = "desc";
                    }
                }
                $('#nFilas').val(regPagina);

                var cadenaFiltros = "(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(),$(\"#paginas li.current\").text(), \"list\", " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);";

                //cambiamos los botones de presentación grid/lista si es necesario
                var presentacionActual = $('.view-mode strong').attr('class');
                if (presentacion != presentacionActual) {
                    var modoGrid = "<strong title='Grid' class='grid'>Grid</strong>&nbsp;<a href='#nodolista' id='nodolista' title='Lista' class='list' onclick='" + cadenaFiltros + "'>Lista</a>&nbsp;";
                    var modoLista = "<a href='#nodolista' title='Grid' class='grid' onclick='" + cadenaFiltros.replace("list", "grid") + "'>Grid</a>&nbsp;<strong title='Lista' class='list'>Lista</strong>&nbsp;";

                    $('.view-mode').empty();
                    $('.view-mode').html((presentacion == 'grid') ? modoGrid : modoLista);                    
                }
                var paginaActual = parseInt($('#paginas li.current').text());
                //si se ha pulsado [<] o [>], actualizamos el número de página
                if (pagina == "mas") pagina = paginaActual++;
                if (pagina == "menos") pagina = paginaActual--;

                //var urlPost = $.General.m_urlBase + 'funciones-comun/';               
                var urlPost = baseUrl + 'funciones-comun/';
                $.post(urlPost,
                {
                    Tarea: 'getdata',
                    tipo: tipo,
                    ordenarPor: ordenarPor,
                    ordenAscDesc: ordenAscDesc,
                    regPagina: regPagina,
                    pagina: pagina,
                    presentacion: presentacion,
                    medida: medida,
                    marca: marca,
                    modelo: modelo,
                    neumatico: neumatico,
                    IC: IC,
                    IV: IV
                },
                function (data) {                    
                    //var datos = "";
                    //var indice = data.indexOf("<!DOCTYPE");
                    //if (indice >= 0) datos = data.substr(0, indice - 4);
                    //else datos = data;
                    //debugger;
                    var resultado = $.parseJSON(data);
                    if (resultado == null || resultado == "" || resultado.length < 2) {
                        alert("Se ha producido un error al obtener los datos");
                    }
                    else {
                        //resultados
                        $('#contenido').html(resultado[0]);
                        //paginado
                        registroActual = (pagina - 1) * regPagina;
                        registroActual = (parseInt(registroActual) < 0) ? 0 : registroActual;                        
                        registroSiguiente = (registroActual + parseInt(regPagina));
                        registroSiguiente = (registroSiguiente > parseInt(resultado[1])) ? (resultado[1] - 1) : registroSiguiente;
                        registroSiguiente = (registroSiguiente < 0) ? 0 : registroSiguiente;
                        regTotales = (resultado[1] - 1);
                        regTotales = (regTotales < 0) ? 0 : regTotales;
                        $('#nResultados').html((((regTotales == 0) ? 0 : (registroActual + 1)) + " a " + registroSiguiente + " de ") + regTotales);
                       
                        var texto = $.Util.m_paginar(resultado[1], regPagina, pagina, paginaActual);
                        $('#paginas ol').empty();
                        $('#paginas ol').html(texto);
                    }
                });
            },
            m_refrescarModelos: function (tipo, marca){
                //var urlPost = $.General.m_urlBase + 'funciones-comun/';               
                var urlPost = baseUrl + 'funciones-comun/';
                $.post(urlPost,
                {
                    Tarea: 'getdata',
                    tipo: tipo,
                    marca: marca
                },
                function (data) {                    
                    var datos = "";
                    var indice = data.indexOf("<!DOCTYPE");
                    if (indice >= 0) datos = data.substr(0, indice - 4);
                    else datos = data;

                    if (datos == "") {
                        alert("Se ha producido un error al cargar los modelos");
                    }
                    else {
                        $('#modelo').html(datos);
                    }
                });
            },
            m_addToCart: function (tipo, sCodigo, accion) {                
                //si estamos en la página de detalle, podemos escoger cantidad de elementos. En caso contrario, añadimos solo un elemento
                var sCantidad = $('#qty').val(); 
                if (isNaN(sCantidad)) sCantidad = 1;

                if (accion == "del") confirm("Se eliminará " + sCodigo + " de la cesta. ¿Está seguro?");

                //var urlPost = $.General.m_urlBase + 'funciones-comun/';               
                var urlPost = baseUrl + 'funciones-comun/';
                $.post(urlPost,
                {
                    Tarea: 'getdata',
                    tipo: tipo,
                    sCodigo: sCodigo,
                    sCantidad: sCantidad,
                    accion: accion
                },
                function (data) {                    
                    var minicart = $.parseJSON(data);
                    //var datos = "";                    
                    //var indice = data.indexOf("<!DOCTYPE");
                    //if (indice >= 0) datos = data.substr(0, indice - 4);
                    //else datos = data;
                    
                    if (minicart == null || minicart == "" || minicart.length < 3) {
                        alert("Se ha producido un error al obtener los datos");
                    }
                    else {                        
                        $('#cesta').html(minicart[0]);
                        $('#idHeader_lblResumen').html(minicart[1]);
                        $('#miniCartContainer').html(minicart[2]);
                    }
                });
            },
            m_actualizarResumenCesta: function (nId, eliminar) {
                var cantidadNueva = parseInt($('#' + nId).val());
                var precioUnitario = parseFloat($('#' + nId).parent().prev().find('span.price').text().replace(',', '.'));                
                //debugger;
                
                //si el nuevo importe es cero, eliminamos la fila
                if (eliminar == 1) {
                    var idFila = "tr_" + nId.replace("prod_", "");
                    $('#' + idFila).remove();
                    cantidadNueva = 0;
                }
               
                var nuevoImporte = (cantidadNueva * precioUnitario).toFixed(2);
                //escribo el nuevo importe de la fila
                $('#' + nId).parent().next().find('span.price').text(nuevoImporte.replace('.', ',') + " €");
                //actualizo el importe total
                var importeTotal = 0;
                $('span.total').each(function () {
                    importeTotal += parseFloat($(this).text().replace(',', '.'));
                });

                $('.importe').text(importeTotal.toFixed(2).replace('.', ',') + ' €');
               

                //obtengo las nuevas cantidades para actualizar en servidor
                var nuevasCantidades = "";
                $('input.qty').each(function () {
                    nuevasCantidades += ($(this).val() + ",");
                });
                

                //var urlPost = $.General.m_urlBase + 'cesta/';               
                var urlPost = baseUrl + 'cesta/';
                $.post(urlPost,
                {                    
                    tipo: 'update',
                    sCantidades: nuevasCantidades
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
                        var minicart = datos.split("----");
                        $('#idHeader_lblResumen').html(minicart[0]);
                        $('#miniCartContainer').html(minicart[1]);
                    }
                });
            },
            m_vaciarCesta: function () {
                if (confirm('¿Desea vaciar la cesta de la compra?')) {
                    //var urlPost = $.General.m_urlBase + 'funciones-comun/';               
                    var urlPost = baseUrl + 'cesta/';
                    $.post(urlPost,
                    {
                        tipo: 'del'
                    },
                    function (data) {
                        var datos = "";
                        var indice = data.indexOf("<!DOCTYPE");
                        if (indice >= 0) datos = data.substr(0, indice - 4);
                        else datos = data;
                        
                        if (datos == "") {
                            alert("Se ha producido un error al vaciar la cesta");
                        }
                        else {
                            $('#idHeader_lblResumen').html(datos);
                            $('#miniCartContainer').html("<p>No tiene artículos en su cesta</p>");
                            $('.cart').empty();
                            $('.cart').html("<button type='button' title='Seguir comprando' class='button btn-continue' onclick='window.location =baseUrl + \"inicio\"'><span><span>Seguir comprando</span></span></button>");
                        }
                    });
                }
            },
            m_checkOut: function (tipo) {
                //var urlPost = $.General.m_urlBase + 'funciones-comun/';               
                var urlPost = baseUrl + 'cesta/compra';
                $.post(urlPost,
                {
                    tipo: tipo
                },
                function (data) {
                    //debugger;
                    var datos = "";
                    var indice = data.indexOf("<!DOCTYPE");
                    if (indice >= 0) datos = data.substr(0, indice - 4);                    
                    else datos = data;

                    if (datos == "") {
                        alert("Se ha producido un error al enviar los datos");
                    }                    
                });
            },
            m_eliminarProdCheck: function (tipo) {

            }
        }
    });
})(jQuery);