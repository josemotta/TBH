<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Newsletter>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Newsletter") %>" class="newsletter-create">
		
	<p class="field input"><label for="title">Subject</label><br />
		<%= Html.TextBox("subject", ViewData["subject"], new { @maxlength = 256 })%>
		<span class="input-message"></span></p>
		
	<p class="field input"><label for="body">Body</label><br />
	    <%= Html.TextArea("body", Model.HtmlBody, new { style = "height: 500px"})%>
		<span class="input-message"></span></p>
		
    <p><button type="submit" id="newsletter-create-button">Send Newsletter</button></p>

</form>

</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/tiny_mce/tiny_mce_src.js"></script>
<script type="text/javascript" src="/content/scripts/manage-newsletter.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidateNewsletter();
</script>
<% } %>
</asp:Content>