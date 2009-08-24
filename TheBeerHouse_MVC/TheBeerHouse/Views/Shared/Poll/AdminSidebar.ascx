<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="polls-admin" class="boxed">
	<h2 class="title">Polls</h2>
	<div class="content">
		<ul>
			<li class="first"><%= Html.ActionLink("View Polls", "ManagePolls") %></li>
			<li><%= Html.ActionLink("Create Poll", "CreatePoll") %></li>
		</ul>
	</div>
</div>