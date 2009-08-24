<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action("CreatePost", "Forum") %>" class="post-create">
						
	<p class="field input"><label for="title">Title</label><br />
		<%= Html.TextBox("title")%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="body">Body</label><br />
		<%= Html.TextArea("body", new { style = "height: 500px"})%>
		<span class="input-message"></span></p>
		
	<hr />
	<p><button type="submit" id="post-create-button">Post To Forum</button></p>

</form>

</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/tiny_mce/tiny_mce_src.js"></script>
<script type="text/javascript" src="/content/scripts/forums.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidatePost();
</script>
<% } %>
</asp:Content>
