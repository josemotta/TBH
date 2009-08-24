<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TheBeerHouse.Models.Category>" %>

<div id="category-<%= ViewData.Model.CategoryID %>" class="category">
	<img src="<%= ViewData.Model.ImageUrl %>" title="<%= Html.Encode(Model.Title) %>>" alt="<%= Html.Encode(Model.Title) %>" class="main-image" />
	<h3>
		<a href="<%= Url.Action("Index", new { category = ViewData.Model.Path, page = 1 }) %>?type=atom" rel="feed" type="application/atom+xml"><img src="/content/images/feed.png" alt="RSS" /></a>&nbsp;
		<a href="<%= Url.Action("Index", new { category = ViewData.Model.Path, page = 1 }) %>"><%= Html.Encode(Model.Title) %></a>
	</h3>
	<p><%= Html.Encode(Model.Description) %></p>
</div>