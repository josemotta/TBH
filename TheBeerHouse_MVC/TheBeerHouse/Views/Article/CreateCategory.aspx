<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
	
<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Article") %>" class="category-create">
		
	<p class="field input"><label for="title">Title</label><br />
		<%= Html.TextBox("title", ViewData["title"], new { maxlength = 256 }) %>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="importance">Importance</label><br />
		<%= Html.TextBox("importance", ViewData["importance"], new { maxlength = 3 }) %>
		<span class="input-message"></span></p>

	<p class="field input"><label for="imageUrl">Image</label><br />
		<%= Html.TextBox("imageUrl", ViewData["imageUrl"], new { maxlength = 256 }) %>
		<span class="input-message"></span></p>

	<p class="field input"><label for="description">Description</label><br />
		<%= Html.TextArea("description", ViewData["description"]) %>
		<span class="input-message"></span></p>
		
<% if(this.ViewContext.RouteData.Values["action"] as string == "EditCategory") { %>
	<p><button type="submit" id="category-create-button">Update Category</button></p>
<% } else { %>
	<p><button type="submit" id="category-create-button">Create Category</button></p>
<% } %>
		
</form>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Article/AdminSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-categories.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidateCategory();
</script>
<% } %>
</asp:Content>