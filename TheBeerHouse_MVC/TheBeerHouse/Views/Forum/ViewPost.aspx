<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Post>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
<% var userVote = (int)ViewData["userVote"]; %>

<div id="posts">
	<div id="post-<%= ViewData.Model.PostID %>" class="post">
		<div class="vote-button">
			<a class="vote-up<%= userVote >= 1 ? " selected" : "" %>" href="#up">Like</a>
			<strong><%= ViewData.Model.VoteCount %></strong>
			<a class="vote-down<%= userVote <= -1 ? " selected" : "" %>" href="#down">Dislike</a>
		</div>
		<%= Html.Encode(Model.Body) %>
		<div class="posted-last">
			<span class="posted-at"><%= ViewData.Model.AddedDate.ToUtcTimeSinceString()%> ago</span>
			<span class="posted-by">by <img src="<%= ViewData.Model.GetAddedByAvatarUrl(16) %>" /> <%= ViewData.Model.AddedBy%></span>
		</div>
	</div>
	<hr style="visibility:hidden;"/>
	<h3>Replies</h3>
	<div id="forum-post-replies">
<% var replies = ViewData.Model.Posts.Where(p => p.Approved).OrderBy(p => p.LastPostDate).AsPagination((int)ViewData["index"], (int)ViewData["count"]); %>
<% foreach(var post in replies) { %>
	<div id="reply-<%= post.PostID %>" class="reply">
		<div class="reply-header">Reply posted by <span class="name"><img src="<%= post.GetAddedByAvatarUrl(16) %>" /> <%= post.LastPostBy%></span> <%= post.LastPostDate.ToUtcTimeSinceString()%> ago</div>
		<% if (User.IsInRole("Editor")) { %>
		<div class="admin">
			<a class="remove" meta:id="<%= post.PostID %>" href="#remove">remove</a>
		</div>
		<% } %>
		<blockquote class="body"><%= Html.Encode(post.Body) %></blockquote>
	</div>
<% } %>
	</div>
	
<% Html.RenderPartial("~/Views/Shared/Pager.ascx", replies); %>
</div>

<% if (!ViewData.Model.Closed) { %>
<form method="post" action="/forums/posts/<%= ViewData.Model.PostID %>/reply" class="post-reply-create">
	<input type="hidden" id="postId" name="postId" value="<%= ViewData.Model.PostID %>" />
	<input type="hidden" id="title" name="title" value="RE: <%= ViewData.Model.Title %>" />
	<p class="field input">
		<label for="body">Enter Your Reply</label><br />
		<%= Html.TextArea("body", String.Empty, new { style = "height: 300px" })%>
		<span class="input-message"></span>
	</p>
	<hr />
	<p><button type="submit" id="post-reply-create-button">Add Reply</button></p>
</form>
<% } else { %>
<div class="info">This post has been closed, and no more replies can be made.</div>
<% } %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<% if (User.IsInRole("Editor")) { %>
<script type="text/javascript" src="/content/scripts/manage-forums.js"></script>
<% } %>
<script type="text/javascript" src="/content/scripts/tiny_mce/tiny_mce_src.js"></script>
<script type="text/javascript" src="/content/scripts/forums.js"></script>
<% if (IsPostBack) { %>
<script type="text/javascript">
	ValidatePost();
</script>
<% } %>
</asp:Content>
