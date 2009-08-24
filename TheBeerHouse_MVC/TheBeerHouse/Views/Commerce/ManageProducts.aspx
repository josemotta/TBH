<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Product>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
<br />
<div id="products">
<% foreach (Product product in ViewData.Model) { %>
	<div class="admin">
		<%= Html.ActionLink("Edit", "EditProduct", new { controller = "Commerce", productID = product.ProductID })%>&nbsp;|&nbsp;
		<a href="#" class="delete-product-button" meta:id="<%= product.ProductID %>">Remove</a>
	</div>
	<% Html.RenderPartial("~/Views/Shared/Commerce/AdminProductItem.ascx", product); %>
	<div id="spacer-<%= product.ProductID %>"><hr /></div>
<% } %>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/commerece.js"></script>
</asp:Content>
