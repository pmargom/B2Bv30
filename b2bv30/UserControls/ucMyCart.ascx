<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMyCart.ascx.cs" Inherits="B2Bv30.UserControls.ucMyCart" %>
<div class="block block-cart">
    <div class="block-title">
        <strong><span>Mi cesta</span></strong>
    </div>
    <div class="block-content" id="cesta">
        <asp:Literal ID="ltCesta" runat="server" Visible="true" Text="<p>No tiene elementos en su cesta</p>" />
    </div>
</div>

