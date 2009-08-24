<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Poll>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="polls">
<% foreach(Poll poll in ViewData.Model) { %>
	<div class="admin"><% if (!poll.IsCurrent) { %><a id="set-current-<%= poll.PollID %>" class="set-current" meta:id="<%= poll.PollID %>" href="#current">Make Current</a>&nbsp;|&nbsp;<% } %><a id="set-archived-<%= poll.PollID %>" class="set-archived<%= poll.IsArchived ? " archived" : "" %>" meta:id="<%= poll.PollID %>" href="#archived"><%= poll.IsArchived ? "Allow Voting" : "Archive" %></a>&nbsp;|&nbsp;<%= Html.ActionLink("Edit", "EditPoll", new { controller = "Poll", pollId = poll.PollID })%>&nbsp;|&nbsp;<%= Html.ActionLink("Remove", "RemovePoll", new { controller = "Poll", pollId = poll.PollID })%></div>
	<% Html.RenderPartial("~/Views/Shared/Poll/PollItem.ascx", poll); %>
	<hr />
<% } %>

<% Html.RenderPartial("~/Views/Shared/Pager.ascx", ViewData.Model); %>
</div>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Poll/AdminSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-polls.js"></script>
</asp:Content>