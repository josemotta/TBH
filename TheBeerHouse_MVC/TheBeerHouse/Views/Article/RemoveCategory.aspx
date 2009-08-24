<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Category>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
		
<% if (ViewData.Model != null) { %>
<form method="post" action="<%= Url.Action("RemoveCategory", "Article", new { categoryId = ViewData.Model.CategoryID }) %>" class="category-remove">

	<p class="field input"><label for="categoryId">Move Articles From <em><%= ViewData.Model.Title %></em> To</label><br />
		<%= Html.DropDownList("newCategoryId")%>
		<span class="input-message"></span></p>

	<button type="submit" name="remove" value="yes" class="yes">yes</button>
	<button type="submit" name="remove" value="no" class="no">no</button>

</form>
<% } %>

</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Article/AdminSidebar.ascx"); %>
</asp:Content>
