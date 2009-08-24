<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.ShippingMethod>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<table width="95%" cellpadding="2" cellspacing="0" align="center" summary="User Grid" border="1" id="shipping-table">
<thead>
<tr style="background-color:#A8C3CB;">
	<th>Shipping Option</th>
	<th>Price</th>
	<th style="width:100px">&nbsp;</th>
</tr>
</thead>
<tbody>
<% foreach(ShippingMethod shippingMethod in ViewData.Model) { %>
<tr id="shipping-method-<%= shippingMethod.ShippingMethodID %>">
<td align="center"><%= Html.Encode(shippingMethod.Title) %></td>
<td align="center"><%= shippingMethod.Price.ToString("C") %></td>
<td align="center"><a href="#" class="delete-shipping-method-button" meta:id="<%= shippingMethod.ShippingMethodID %>"><img border="0" alt="Delete Role" src="/content/images/DeleteSymbol.png" title="Delete Role" align="middle"/></a></td>
</tr>
<% } %>
</tbody>
<tfoot>
<tr style="background-color:#A8C3CB;">
	<td align="center"><%= Html.TextBox("title")%></td>
	<td align="center"><%= Html.TextBox("price")%></td>
	<td align="center"><button id="add-shipping-method-button" type="submit">Add</button></td>
</tr>
</tfoot>
</table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/commerece.js"></script>
</asp:Content>
