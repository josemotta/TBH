<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Pagination<TheBeerHouse.Models.Poll>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% if (TheBeerHouseSection.Current.Polls.ArchiveIsPublic) { %>
	<%= Html.ActionLink("Public", "Index", "Poll") %> | <%= Html.ActionLink("Archived", "Index", new { controller = "Poll", archived = true })%>
<% } %>

<div id="polls">
<% foreach(Poll poll in Model) { %>
	<% if (poll != null && !poll.IsArchived && Request.Cookies["poll_" + poll.PollID] == null) { %>
		<% Html.RenderPartial("~/Views/Shared/Poll/PollItem.ascx", poll); %>
	<% } else { %>
		<% Html.RenderPartial("~/Views/Shared/Poll/PollResultItem.ascx", poll); %>
	<% } %>	<hr />
<% } %>

<% Html.RenderPartial("~/Views/Shared/Pager.ascx", ViewData.Model); %>
</div>
</asp:Content>
