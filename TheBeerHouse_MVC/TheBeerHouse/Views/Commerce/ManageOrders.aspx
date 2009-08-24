<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Order>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<table width="95%" cellpadding="2" cellspacing="0" align="center" summary="User Grid" border="1">
<thead>
<tr style="background-color:#A8C3CB; ">
	<th>ID#</th>
	<th>Status</th>
	<th>Paypal Transaction #</th>
	<th>Order Placed Date</th>
	<th>Amount</th>
	<th>&nbsp;</th>
</tr>
</thead>
<tbody>
<% foreach(Order order in ViewData.Model) { %>
<tr>
<td align="center"><%= order.OrderID%></td>
<td align="center"><%= order.Status%></td>
<td align="center"><%= order.TransactionID%></td>
<td align="center"><%= order.AddedDate%></td>
<td align="center"><%= order.SubTotal.ToString("C")%></td>
<td align="center"><%= Html.ActionLink("view", "OrderDetail", new { orderId = order.OrderID }) %></td>
</tr>
<% } %>
</tbody>
</table>		

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>
