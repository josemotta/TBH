<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action(this.ViewContext.RouteData.Values["action"] as string, "Poll") %>" class="poll-create">
	<p class="field input"><label for="question">Question</label><br />
		<%= Html.TextBox("question", ViewData["question"], new { @maxlength = 256 })%>
		<span class="input-message"></span></p>
		
	<hr />		
	<% if(this.ViewContext.RouteData.Values["action"] as string == "EditPoll") { %>		
	<p><button type="submit" id="Button1">Update Poll</button></p>
	<% } else { %>
	<p><button type="submit" id="poll-create-button">Create Poll</button></p>
	<% } %>
</form>

<% if(this.ViewContext.RouteData.Values["action"] as string == "EditPoll") { %>	
	<h3>Poll Options</h3>

	<ul id="poll-options" class="manage-options">
	<% foreach(var option in ViewData["options"] as IEnumerable<PollOption>) { %>
		<li id="option-<%= option.OptionID %>" class="option"><span class="text"><%= Html.Encode(option.OptionText) %></span> <button type="button" class="edit-option-button" meta:id="<%= option.OptionID %>">Edit</button>&nbsp;<button type="button" class="delete-option-button" meta:id="<%= option.OptionID %>">Delete</button></li>
	<% } %>
	</ul>
	
<form method="post" action="#" class="poll-options-create">
	<input type="hidden" id="pollId" name="pollId" value="<%= ViewData["pollId"] %>" />

	<p class="field input"><label for="option">Option</label><br />
		<%= Html.TextBox("option", ViewData["option"], new { @maxlength = 256 })%>
		<span class="input-message"></span></p>
		
	<hr />
	<p><button type="submit" id="poll-option-create-button">Create Poll Option</button></p>
</form>
<% } %>

</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Poll/AdminSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-polls.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidatePoll();
</script>
<% } %>
</asp:Content>