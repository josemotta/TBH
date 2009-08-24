<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="forums-admin" class="boxed">
	<h2 class="title">Forums</h2>
	<div class="content">
	<ul>
		<li class="first"><%= Html.ActionLink("View Forums", "ManageForums") %></li>
		<li><%= Html.ActionLink("Approve Posts", "ManagePosts") %></li>
		<li><%= Html.ActionLink("Create Forum", "CreateForum") %></li>
	</ul>
	</div>
</div>