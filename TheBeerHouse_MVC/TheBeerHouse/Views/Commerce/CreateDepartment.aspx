<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Department>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
	
<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Commerce") %>" class="department-create">

	<p class="field input"><label for="title">Title</label><br />
		<%= Html.TextBox("title", null, new { maxlength = 256 })%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="importance">Importance</label><br />
		<%= Html.TextBox("importance", null, new { maxlength = 3 })%>
		<span class="input-message"></span></p>

	<p class="field input"><label for="imageUrl">Image</label><br />
		<%= Html.TextBox("imageUrl",null, new { maxlength = 256 })%>
		<span class="input-message"></span></p>

	<p class="field input"><label for="description">Description</label><br />
		<%= Html.TextArea("description") %>
		<span class="input-message"></span></p>

    <%= Html.Hidden("id", ViewData["id"])%>
	<p><button type="submit" id="deparment-create-button"><%= ViewData["PageTitle"] %></button></p>

</form>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-department.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
    ValidateDepartment();
</script>
<% } %>
</asp:Content>