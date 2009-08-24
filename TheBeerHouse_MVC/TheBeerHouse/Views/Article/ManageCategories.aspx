<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Category>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="categories">
<% foreach (Category category in ViewData.Model) { %>
	<div class="admin">
		<%= Html.ActionLink("Edit", "EditCategory", new { controller = "Article", categoryId = category.CategoryID })%>&nbsp;|&nbsp;
		<%= Html.ActionLink("Remove", "RemoveCategory", new { controller = "Article", categoryId = category.CategoryID })%>
	</div>
	<% Html.RenderPartial("~/Views/Shared/Article/CategoryItem.ascx", category); %>
	<hr />
<% } %>
</div>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Article/AdminSidebar.ascx"); %>
</asp:Content>