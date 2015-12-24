using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace B2Bv30.UserControls
{
    public partial class ucMyCart : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarCesta();
        }
        private void CargarCesta()
        {
            if (Session["CART"] != null && Session["sCART"] != null)
            {
                string[] cesta = (string[])Session["sCART"];
                if (cesta[0] != "") ltCesta.Text = cesta[0];
            }
        }
    }
}