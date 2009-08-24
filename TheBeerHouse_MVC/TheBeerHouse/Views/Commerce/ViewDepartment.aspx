<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Product>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
<br />
<div id="products">
<% foreach (Product product in ViewData.Model) { %>
	<% Html.RenderPartial("~/Views/Shared/Commerce/ProductItem.ascx", product); %>
	<hr />
<% } %>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>

