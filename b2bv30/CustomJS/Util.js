jQuery.extend({
    Util: {
        m_isEmail: function (cadena) {
            if (cadena.lastIndexOf('.') == -1) {
                return false; //no tiene puntos
            }
            var ArrDom = new Array("es", "com", "net", "edu", "info", "cat", "tv", "uk", "fr", "org", "cat");
            var SufixDom = cadena.substring(cadena.lastIndexOf('.') + 1, cadena.length);
            var DomOk = true;
            for (i = 0; i < ArrDom.length; i = i + 1) {
                if (SufixDom == ArrDom[i]) { DomOk = true; }
            }
            if (!DomOk) return false; //El dominio no esta en la lista
            if (cadena.search('@') == -1) {
                return false; //no hay ningun @
            }
            if (cadena.indexOf('@') != cadena.lastIndexOf('@')) {
                return false; //hay mas de una @
            }
            if (cadena.indexOf('@') == 0) {
                return false; // @ en primer lugar
            }
            if (cadena[cadena.indexOf('@') - 1] == '.' || cadena[cadena.indexOf('@') + 1] == '.') {
                return false; // puntos pegados en el @
            }
            for (i = 0; i < cadena.length; i++) {
                var caracter = cadena.charAt(i);
                if (!((caracter >= 'a' && caracter <= 'z') ||
    			(caracter >= 'A' && caracter <= 'Z') ||
    			(caracter >= '0' && caracter <= '9') ||
    			caracter == '.' || caracter == '-' ||
    			caracter == '@' || caracter == '_')) {
                    return false;
                }
            }
            return true;
        },
        m_paginar: function (totalRegistros, regPagina, pagina, paginaActual) {
            //modificamos los botones del paginado si es necesario. Serán de la forma [<] [x] [x+1] [x+2] [>] 
            var texto = "";
            //debugger;
            var botonesNecesarios = Math.ceil(totalRegistros / regPagina);
            var nPagina = parseInt(pagina);

            //vemos si es necesario representar el botón [<] (si pretendemos ir a una página mayor a dos)
            if ((!isNaN(nPagina) && nPagina > 2) || (pagina == "mas" && paginaActual > 1) || (pagina == "menos" && paginaActual > 3))
                texto += "<li><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), \"menos\", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\", $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>&lt;</a></li>";
            
            //BOTÓN [x]            
            //si es posible, representamos la página actual en el botón [x+1]
            if (!isNaN(nPagina)) { //se ha presionado un número                
                texto += "<li class='digit" + ((botonesNecesarios == 1 || pagina == 1) ? " current" : "") + "'><a href='#' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + ((nPagina > 1) ? (nPagina - 1) : "1") + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + ((nPagina > 1) ? (nPagina - 1) : "1") + "</a></li>";
            }
            else { //se ha presionado el botón [<] o el [>]
                if (pagina == "mas")
                    texto += "<li class='digit'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + paginaActual + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + paginaActual + "</a></li>";
                if (pagina == "menos")
                    texto += "<li class='digit'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + ((paginaActual > 2) ? (paginaActual - 2) : "1") + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + ((paginaActual > 2) ? (paginaActual - 2) : "1") + "</a></li>";
            }
            //BOTÓN [x+1]
            //vemos si es necesario representarlo
            if (botonesNecesarios > 1) {
                if (!isNaN(nPagina)) { //se ha presionado un número                
                    texto += "<li class='digit" + ((pagina > 1) ? " current" : "") + "'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + ((nPagina > 1) ? nPagina : "2") + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + ((nPagina > 1) ? nPagina : "2") + "</a></li>";
                }
                else { //se ha presionado el botón [<] o el [>]
                if (pagina == "mas")
                    texto += "<li class='digit current'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + (paginaActual + 1) + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + (paginaActual + 1) + "</a></li>";
                if (pagina == "menos")
                    texto += "<li class='digit" + ((paginaActual > 2) ? " current" : "") + "'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + ((paginaActual > 2) ? (paginaActual - 1) : "2") + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + ((paginaActual > 2) ? (paginaActual - 1) : "2") + "</a></li>";

                }
            }
            //BOTÓN [x+2]
            //vemos si es necesario representarlo
            if (botonesNecesarios > 2) {
                if (!isNaN(nPagina)) { //se ha presionado un número                
                    texto += "<li class='digit'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + ((nPagina > 1) ? (nPagina + 1) : "3") + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + ((nPagina > 1) ? (nPagina + 1) : "3") + "</a></li>";
                }
                else { //se ha presionado el botón [<] o el [>]
                    if (pagina == "mas")
                        texto += "<li class='digit'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + (paginaActual + 2) + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + (paginaActual + 2) + "</a></li>";
                    if (pagina == "menos")
                        texto += "<li class='digit'><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), " + ((paginaActual > 2) ? paginaActual : "3") + ", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>" + ((paginaActual > 2) ? paginaActual : "1") + "</a></li>";
                }
            }
            //vemos si es necesario representar el botón [>] (si pretendemos ir a una página mayor a tres)
            if (botonesNecesarios > 3)
                texto += "<li><a href='#nodolista' onclick='(function ($) { $.B2BProductos.m_capturaFiltros(\"destacados\", $(\"#ordenarPor\").val(), 0, $(\"#nFilas\").val(), \"mas\", $(\".view-mode strong\").attr(\"class\"), " + totalRegistros + ",$(\"#medida\").val(), $(\"#marca\").val(), $(\"#modelo\").val(), $(\"#neumatico\").val(), $(\"#IC\").val(), $(\"#IV\").val()) })(jQuery);'>&gt;</a></li>";

            return texto;
        }
    }

});