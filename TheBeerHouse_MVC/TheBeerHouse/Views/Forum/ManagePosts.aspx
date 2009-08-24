<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Post>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% foreach(var post in ViewData.Model) { %>
	<div id="post-<%= post.PostID %>" class="post">
		<div class="admin">
			<a class="approve" meta:id="<%= post.PostID %>" href="#approve">Approve</a>&nbsp;|&nbsp;
			<a class="remove" meta:id="<%= post.PostID %>" href="#remove">Remove</a>
		</div>
		<h3><%= Html.Encode(post.Title) %></h3>
		<div class="posted-last">
			<span class="posted-at"><%= post.LastPostDate.ToUtcTimeSinceString() %> ago</span>
			<span class="posted-by">by <img src="<%= post.GetLastPostByAvatarUrl(16) %>" /> <%= post.LastPostBy %></span>
		</div>
		<a class="toggle-body" href="#">Show Body</a>
		<div class="body" style="display:none"><%= Html.Encode(post.Body) %></div>
	</div>
	<hr />
<% } %>

</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Forum/AdminSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-forums.js"></script>
</asp:Content>
