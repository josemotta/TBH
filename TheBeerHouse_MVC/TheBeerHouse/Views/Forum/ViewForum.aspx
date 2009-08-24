<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Forum>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<%= Html.ActionLink("Create A Post For This Forum", "CreatePost", new { forumId = ViewData.Model.ForumID }, new { @class = "post-button" })%>

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<% if (ViewData.Model.Posts.Count == 0) { %>
	<p>There are no posts in this forum yet, you can be the first.</p>
<% } else { %>
<div id="posts">
<% var posts = ViewData.Model.Posts.Where(p => p.Approved).OrderByDescending(p => p.LastPostDate).AsPagination((int)ViewData["index"], (int)ViewData["count"]); %>
<% foreach(var post in posts) { %>
	<div id="post-<%= post.PostID %>" class="post">
<% if (User.IsInRole("Editor")) { %>
		<div class="admin">
<% if (!post.Closed) { %>
			<a class="close" meta:id="<%= post.PostID %>" href="#close">Close</a>&nbsp;|&nbsp;
<% } %>
			<a class="remove" meta:id="<%= post.PostID %>" href="#remove">Remove</a>
		</div>
<% } %>
		<div class="stats">
			<div class="votes stat"><strong><%= post.VoteCount %></strong><small>votes</small></div>
			<div class="replies stat"><strong><%= post.ReplyCount %></strong><small>replies</small></div>
			<div class="views stat"><strong><%= post.ViewCount %></strong><small>views</small></div>
		</div>
		<h3><%= Html.ActionLink(post.Title, "ViewPost", new { controller = "Forum", postId = post.PostID, path = post.Path }) %><% if (post.Closed) { %> [closed]<% } %></h3>
		<div class="posted-last">
			<span class="posted-at"><%= post.LastPostDate.ToUtcTimeSinceString() %> ago</span>
			<span class="posted-by">by <img src="<%= post.GetLastPostByAvatarUrl(16) %>" /> <%= post.LastPostBy %></span>
		</div>
	</div>
	<hr />
<% } %>
</div>

<% Html.RenderPartial("~/Views/Shared/Pager.ascx", posts); %>
<% } %>

</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<% if (User.IsInRole("Editor")) { %>
<script type="text/javascript" src="/content/scripts/manage-forums.js"></script>
<% } %>
</asp:Content>
