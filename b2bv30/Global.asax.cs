using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace B2Bv30
{
    public class Global : System.Web.HttpApplication
    {
        private void PrepareRoutes()
        {
            RouteTable.Routes.MapPageRoute("", "", "~/index.aspx");
            RouteTable.Routes.MapPageRoute("", "inicio/", "~/index.aspx");
            RouteTable.Routes.MapPageRoute("", "login/", "~/login.aspx");
            RouteTable.Routes.MapPageRoute("", "funciones-comun/", "~/funciones/comun.aspx");
            RouteTable.Routes.MapPageRoute("", "productos/{sNombreProducto}/{sCodigo}", "~/detalleProducto.aspx");
            RouteTable.Routes.MapPageRoute("", "cesta/", "~/checkOut.aspx");
            RouteTable.Routes.MapPageRoute("", "cesta/confirmar", "~/checkOut.aspx");
            RouteTable.Routes.MapPageRoute("", "cesta/compra", "~/checkOut.aspx");
            RouteTable.Routes.MapPageRoute("", "informacion-administrativa/facturas", "~/facturas.aspx");
            RouteTable.Routes.MapPageRoute("", "informacion-administrativa/albaranes", "~/albaranes.aspx");
            RouteTable.Routes.MapPageRoute("", "informacion-administrativa/vencimientos", "~/vencimientos.aspx");
            RouteTable.Routes.MapPageRoute("", "informacion-administrativa/pedidos", "~/pedidos.aspx");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            PrepareRoutes();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}