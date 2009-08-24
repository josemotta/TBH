<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleItem.ascx.cs" Inherits="TheBeerHouse.Views.Shared.Article.ArticleItem" %>

<div id="article-<%= ViewData.Model.ArticleID %>" class="article">
	<img src="<%= ViewData.Model.Category.ImageUrl %>" class="category-image" title="<%= Html.Encode(Model.Category.Title) %>" />
	<h3><%= Html.ActionLink(ViewData.Model.Title, "ViewArticle", new { controller = "Article", id = ViewData.Model.ArticleID, path = ViewData.Model.Path })%></h3>
	<p><%= Html.Encode(!String.IsNullOrEmpty(ViewData.Model.Abstract) ? Model.Abstract : Model.Body) %></p>
	<ul>
		<li><strong>Rating: </strong><%= ViewData.Model.Votes%> <%= ViewData.Model.Votes == 1 ? "user has" : "users have"%> rated this article <% if (ViewData.Model.AverageRating > 0) { %><img src="<%= ImageRatingUrl %>" alt="<%= ViewData.Model.AverageRating %>" /><% } %></li>
		<li><strong>Posted By: </strong><%= ViewData.Model.AddedBy%></li>
		<li><strong>Views: </strong>this article has been read <%= ViewData.Model.ViewCount%> times</li>
		<li><strong>Location: </strong><%= ViewData.Model.Location%></li>
	</ul>
</div>