<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ViewArticle.aspx.cs" Inherits="TheBeerHouse.Views.Article.View" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="article-view">
		<ul>
			<li><strong>Rating: </strong>
				<%= ViewData.Model.Votes%>
				<%= ViewData.Model.Votes == 1 ? "user has" : "users have"%>
				rated this article <span id="article-rating-value">
					<% if (ViewData.Model.AverageRating > 0) { %><img src="<%= ImageRatingUrl %>" alt="<%= ViewData.Model.AverageRating %>" /><% } %></span></li>
			<li><strong>Posted By: </strong>
				<%= ViewData.Model.AddedBy%></li>
			<li><strong>Views: </strong>this article has been read
				<%= ViewData.Model.ViewCount%>
				times</li>
			<li><strong>Location: </strong>
				<%= Html.Encode(Model.Location) %></li>
		</ul>
		<%= Html.Encode(Model.Body) %>
	</div>
	<form method="post" action="#" class="rate-article">
	<p class="field input">
		<h3><label for="rating">What would you rate this article?</label></h3>
		<br />
		<select name="rating" id="rating">
			<option value="0">0 Beers</option>
			<option value="1">1 Beers</option>
			<option value="2">2 Beers</option>
			<option value="3">3 Beers</option>
			<option value="4">4 Beers</option>
			<option value="5">5 Beers</option>
		</select>
		<button type="submit" id="rate-button">Rate</button>
	</p>
	</form>
	<% if (ViewData.Model.CommentsEnabled) { %>
	<div id="article-comments">
		<h3>Comments</h3>
		<% foreach (Comment comment in ViewData.Model.Comments) { %>
		<% Html.RenderPartial("~/Views/Shared/Article/CommentItem.ascx", comment); %>
		<% } %>
	</div>
	<form method="post" action="#" class="comment-create">
	<input type="hidden" id="articleId" name="articleId" value="<%= ViewData.Model.ArticleID %>" />
	<input type="hidden" id="commentId" name="commentId" value="" />
	<p class="field input">
		<label for="name">Name</label><br />
		<%= Html.TextBox("comment-name", null, new { @maxlength = 256 })%>
		<span class="input-message"></span>
	</p>
	<p class="field input">
		<label for="email">E-Mail</label><br />
		<%= Html.TextBox("comment-email", null, new { @maxlength = 256 })%>
		<span class="input-message"></span>
	</p>
	<p class="field input">
		<label for="body">Body</label><br />
		<%= Html.TextArea("comment-body", String.Empty)%>
		<span class="input-message"></span>
	</p>
	<hr />
	<p>
		<button type="submit" id="comment-create-button">Add Comment</button></p>
	</form>
	<% } %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
	<script type="text/javascript" src="/content/scripts/article.js"></script>
</asp:Content>
