<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Order>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<p><%= Html.ActionLink("Return to Order Management", "ManageOrders") %></p>

<div id="OrderInformation" class="form">
<h2 class="title">Order Information</h2> 
<div class="content">
	<strong>Order ID: </strong><%= ViewData.Model.OrderID %><br />
	<strong>Paypal Transaction ID: </strong> <%= ViewData.Model.TransactionID %><br />
	<strong>Order Placed By: </strong> <%= ViewData.Model.AddedBy %><br />
	<strong>Order Date Placed: </strong> <%= ViewData.Model.AddedDate %><br />
	<strong>Order Status: </strong> <%= ViewData.Model.Status %>
</div>
</div>

<div id="ShippingInformation" class="form">
<h2 class="title">Shipping Information</h2> 
<div class="content">
    <address><%= Html.Encode(Model.ShippingFirstName) %> <%= Html.Encode(Model.ShippingLastName) %><br />
    <%= Html.Encode(Model.ShippingStreet) %><br />
   <%= Html.Encode(Model.ShippingCity) %>, <%= Html.Encode(Model.ShippingState) %> <%= Html.Encode(Model.ShippingPostalCode) %></address><br />

	<strong>Email:</strong> <a href="mailto:<%= Html.Encode(Model.CustomerEmail) %>"><%= Html.Encode(Model.CustomerEmail) %></a><br /><br />
	<strong>Shipping Details: </strong><%= Html.Encode(Model.ShippingMethod) %>><br />
    
    <% if(String.IsNullOrEmpty(ViewData.Model.TrackingID)) { %>
	<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Commerce") %>" class="product-create">
	<%= Html.Hidden("id", ViewData["id"])%>
		<label for="trackingID"><strong>Tracking Number:</strong></label>
		<%= Html.TextBox("trackingId")%>
		<button type="submit" id="product-create-button">Save</button>
	</form>
    <% } else { %>
	<strong>Tracking Number: </strong><%= ViewData.Model.TrackingID %>
    <% } %>
</div>
</div>

<table width="100%" cellpadding="2" cellspacing="0" align="left" summary="User Grid" border="1">
<thead>
<tr style="background-color:#A8C3CB;">
	<th>Item</th>
	<th style="width:100px">Quantity</th>
	<th style="width:100px">Price</th>
</tr>
</thead>
<tbody>
<% foreach(OrderItem orderItem in ViewData.Model.OrderItems) { %>
<tr>
	<td align="center"><%= Html.Encode(orderItem.Title) %></td>
	<td align="center"><%= orderItem.Quantity%></td>
	<td align="right"><%= (orderItem.UnitPrice * orderItem.Quantity).ToString("C")%></td>
</tr>
<% } %>
</tbody>
<tfoot>
<tr style="background-color:#A8C3CB;">
	<th align="right" colspan="2">Sub Total:</th>
	<td align="right"><%= Model.SubTotal.ToString("C")%></td>
</tr>
<tr style="background-color:#A8C3CB;">
	<th align="right" colspan="2">Shipping:</th>
	<td align="right"><%= Model.Shipping.ToString("C")%></td>
</tr>
<tr style="background-color:#A8C3CB;">
	<th align="right" colspan="2">Total:</th>
	<td align="right"><%= (Model.SubTotal + Model.Shipping).ToString("C")%></td>
</tr>
</tfoot>
</table>	
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>
