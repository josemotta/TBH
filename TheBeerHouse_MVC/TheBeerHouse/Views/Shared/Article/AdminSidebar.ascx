<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="articles-admin" class="boxed">
	<h2 class="title">Articles</h2>
	<div class="content">
	<ul>
		<li class="first"><%= Html.ActionLink("View Categories", "ManageCategories") %></li>
		<li><%= Html.ActionLink("Create Category", "CreateCategory") %></li>
		<li><%= Html.ActionLink("View Articles", "ManageArticles") %></li>
		<li><%= Html.ActionLink("Create Article", "CreateArticle") %></li>
		<li><%= Html.ActionLink("Comments", "ManageComments") %></li>
	</ul>
	</div>
</div>