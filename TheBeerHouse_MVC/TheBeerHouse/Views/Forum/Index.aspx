<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Forum>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<div id="forums">
<% foreach(var forum in ViewData.Model) { %>
	<div id="forum-<%= forum.ForumID %>" class="forum">
		<h2><%= Html.ActionLink(forum.Title, "ViewForum", new { controller = "Forum", forumId = forum.ForumID, path = forum.Path }) %></h2>
		<p><%= forum.Moderated ? "<strong>[moderated]</strong> " : "" %><%= Html.Encode(forum.Description) %></p>
	</div>
	<hr />
<% } %>
</div>

</asp:Content>