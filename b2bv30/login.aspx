<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="B2Bv30.login" %>
<%@ Register Src="~/UserControls/ucLogin.ascx" TagName="login" TagPrefix="ucLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/js/jquery-1.7.2.js")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("aside").hide();
            $(".col-main").css("width", "100%");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="parteCentral" runat="server">
    <ucLogin:login ID="idLogin" runat="server" />
</asp:Content>
