using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace B2Bv30.UserControls
{
    public partial class ucHeaderTop : System.Web.UI.UserControl
    {
        private B2BWs.Usuario CurrentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = GetUserFromSession();
            if (CurrentUser != null)
            {
                liWelcomelogin.Text = string.Format("Bienvenido {0}", CurrentUser.login);
            }
            else liWelcomelogin.Text = string.Format("Bienvenido {0}", "anónimo");
            RepresentarCarrito();
        }

        private B2BWs.Usuario GetUserFromSession()
        {
            B2BWs.Usuario user = Session["CURRENT_USER"] as B2BWs.Usuario;
            return user;
        }

        private void RepresentarCarrito()
        {
            //en Session["sMINICART"] se almacena un array de dos elementos, con el resumen y la lista de items
            if (Session["CART"] != null && Session["sCART"] != null) 
            {
                string[] minicart = (string[])Session["sCART"];
                lblResumen.Text = minicart[1];
                ltMiniCart.Text = minicart[2];
            }
        }
    }
}