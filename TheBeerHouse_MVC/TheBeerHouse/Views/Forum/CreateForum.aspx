<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Forum") %>" class="forum-create">
						
	<p class="field input"><label for="title">Title</label><br />
		<%= Html.TextBox("title")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="description">Description</label><br />
		<%= Html.TextArea("description")%>
		<span class="input-message"></span></p>

	<p class="field input">
		<%= Html.CheckBox("moderated") %> <label for="moderated">Is Forum Moderated?</label>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="order">Order</label><br />
		<%= Html.TextBox("order") %>
		<span class="input-message"></span></p>
		
	<hr />
	<% if(this.ViewContext.RouteData.Values["action"] as string == "EditForum") { %>
	<p><button type="submit" id="forum-create-button">Update Forum</button></p>
	<% } else { %>
	<p><button type="submit" id="forum-create-button">Create Forum</button></p>
	<% } %>
</form>

</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Forum/AdminSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/tiny_mce/tiny_mce_src.js"></script>
<script type="text/javascript" src="/content/scripts/manage-forums.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidateForum();
</script>
<% } %>
</asp:Content>
