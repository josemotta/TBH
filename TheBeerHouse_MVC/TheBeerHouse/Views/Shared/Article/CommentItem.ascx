<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TheBeerHouse.Models.Comment>" %>

<div id="comment-<%= ViewData.Model.CommentID %>" class="comment">
	<div class="comment-header">Comment posted by <span class="name"><%= ViewData.Model.AddedBy%></span> <%= (DateTime.Now - ViewData.Model.AddedDate).ToLongString()%> ago</div>
	<blockquote class="body"><%= Html.Encode(Model.Body) %></blockquote>
</div>