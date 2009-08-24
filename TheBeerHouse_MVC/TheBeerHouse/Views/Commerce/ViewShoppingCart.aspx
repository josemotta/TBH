<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.ShoppingCart>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<table width="100%" cellpadding="2" cellspacing="0" align="left" summary="User Grid" border="1">
<tr style="font-weight:bold; background-color:#A8C3CB; ">
<td align="center">Item</td>
<td style="width:100px" align="center">Quantity</td>
<td style="width:100px" align="center">Price</td>
<td style="width:20px">&nbsp;</td>
</tr>
<% foreach(ShoppingCartItem shoppingCartItem in ViewData.Model) { %>
<tr id="item-<%= shoppingCartItem.Product.ProductID %>">
<td align="center"><%= Html.Encode(shoppingCartItem.Title) %></td>
<td align="center"><%= shoppingCartItem.Quantity %></td>
<td align="center"><%= shoppingCartItem.Price.ToString("c") %></td>
<td align="center"><a class="delete-item-button" meta:id="<%= shoppingCartItem.Product.ProductID %>" href="#"><img border="0" alt="Delete Role"  src="/content/images/DeleteSymbol.png" align="middle"/</a></td>
</tr>
<% } %>
<tr style="font-weight:bold; background-color:#A8C3CB;">
<td>&nbsp;</td>
<td align="right">Subtotal:</td>
<td align="center"><%= ViewData.Model.SubTotal.ToString("c")%></td>
<td>&nbsp;</td>
</tr>

<tr style="font-weight:bold; background-color:#A8C3CB;">
<td align="right"><form method="post" action="<%= Url.Action("ViewShoppingCart","Commerce") %>" class="add-cart"><%= Html.DropDownList("shippingMethod")%></form></td>
<td align="right">Shipping:</td>
<td align="center"><%= ViewData.Model.ShippingPrice.ToString("c") %></td>
<td>&nbsp;</td>
</tr>

<tr style="font-weight:bold; background-color:#A8C3CB;">
<td>&nbsp;</td>
<td align="right">Total:</td>
<td align="center"><%= ViewData.Model.Total.ToString("c") %></td>
<td>&nbsp;</td>
</tr>
<tr>
<td>&nbsp;</td>
<td>&nbsp;</td>
<td align="center">
<form id="Paypal" name="Paypal" action="<%= TheBeerHouseSection.Current.Commerce.PayPalServer %>" method="post">
<input type="hidden" name="cmd" value="_cart" />
<input type="hidden" name="upload" value="1" />
<input type="hidden" name="business" value="<%= TheBeerHouseSection.Current.Commerce.PayPalAccount %>" />

<input type="hidden" name="shipping" value="<%= ViewData.Model.ShippingPrice.ToString("N2") %>" />
<input type="hidden" name="amount" value="<%= ViewData.Model.Total.ToString("N2") %>" />

<% int count = 1; %>
<% foreach(ShoppingCartItem shoppingCartItem in ViewData.Model) { %>
    <%=Html.Hidden("amount_" + count, shoppingCartItem.Price.ToString("N2"))%>
    <%=Html.Hidden("item_name_" + count, shoppingCartItem.Title) %>
	<%=Html.Hidden("item_number_" + count, shoppingCartItem.Product.ProductID) %>
	<%=Html.Hidden("quantity_" + count, shoppingCartItem.Quantity) %>
    <%count++;%>
<% } %>

<button type="submit" id="paypal-checkout-button" value="PayPal">Checkout</button>
</form>
</td>
<td>&nbsp;</td>
</tr>
</table>

</asp:Content>


<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/commerece.js"></script>
</asp:Content>



