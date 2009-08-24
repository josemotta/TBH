<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Forum>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<div id="forums">
<% foreach(var forum in ViewData.Model) { %>
	<div class="admin"><%= Html.ActionLink("Edit", "EditForum", new { forumId = forum.ForumID })%>&nbsp;|&nbsp;<%= Html.ActionLink("Remove", "RemoveForum", new { forumId = forum.ForumID })%></div>
	<div id="forum-<%= forum.ForumID %>" class="forum">
		<h2><%= Html.ActionLink(forum.Title, "ViewForum", new { forumId = forum.ForumID, path = forum.Path }) %></h2>
		<p><%= forum.Moderated ? "<strong>[moderated]</strong> " : "" %><%= Html.Encode(forum.Description) %></p>
	</div>
	<hr />
<% } %>
</div>

</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Forum/AdminSidebar.ascx"); %>
</asp:Content>