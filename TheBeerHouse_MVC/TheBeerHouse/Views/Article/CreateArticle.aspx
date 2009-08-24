<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Article") %>" class="article-create">
		
	<h3>Article</h3>
		
	<p class="field input"><label for="categoryId">Category</label><br />
		<%= Html.DropDownList("categoryId")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="title">Title</label><br />
		<%= Html.TextBox("title", ViewData["title"], new { @maxlength = 256 })%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="summary">Summary</label><br />
		<%= Html.TextArea("summary")%>
		<span class="input-message"></span></p>

	<p class="field input"><label for="body">Body</label><br />
		<%= Html.TextArea("body", new { style = "height: 500px"})%>
		<span class="input-message"></span></p>
		
	<h3>Meta Data</h3>
		
	<p class="field input"><label for="country">Country</label><br />
		<%= Html.DropDownList("country") %>
		<span class="input-message"></span></p>

	<p class="field input"><label for="state">State</label><br />
		<%= Html.TextBox("state", ViewData["state"], new { @maxlength = 256 })%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="city">City</label><br />
		<%= Html.TextBox("city", ViewData["city"], new { @maxlength = 256 })%>
		<span class="input-message"></span></p>

	<p class="field input"><label for="releaseDate">Release Date</label><br />
		<%= Html.TextBox("releaseDate")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="expireDate">Expire Date</label><br />
		<%= Html.TextBox("expireDate")%>
		<span class="input-message"></span></p>
		
<% if (Roles.IsUserInRole("Editor")) { %>
	<h3>Options</h3>
		
	<p class="field"><ul class="options">
			<li><%= Html.CheckBox("approved") %> <label for="approved">Approved To Be Published</label></li>
			<li><%= Html.CheckBox("listed") %> <label for="listed">Listed</label></li>
			<li><%= Html.CheckBox("commentsEnabled") %> <label for="commentsEnabled">Enable Comments</label></li>
			<li><%= Html.CheckBox("onlyForMembers") %> <label for="onlyForMembers">Only Members Can View</label></li>
		</ul>
		<span class="input-message"></span></p>
<% } %>
	<hr />
<% if(this.ViewContext.RouteData.Values["action"] as string == "EditArticle") { %>
	<p><button type="submit" id="article-create-button">Update Article</button></p>
<% } else { %>
	<p><button type="submit" id="article-create-button">Create Article</button></p>
<% } %>
</form>

</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Article/AdminSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/tiny_mce/tiny_mce_src.js"></script>
<script type="text/javascript" src="/content/scripts/manage-articles.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidateArticle();
</script>
<% } %>
</asp:Content>