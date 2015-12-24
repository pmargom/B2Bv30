<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="checkOut.aspx.cs" Inherits="B2Bv30.checkOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                $("aside").hide();
                $(".col-main").css("width", "100%");
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
    <asp:Literal ID="ltContenido" runat="server" Text="" Visible="true" />
</asp:Content>
