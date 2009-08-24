<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Pagination<TheBeerHouse.Models.Comment>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="article-comments">
<% foreach(Comment comment in ViewData.Model) { %>
	<% Html.RenderPartial("~/Views/Shared/Article/CommentItem.ascx", comment); %>
	<div class="admin"><a href="#edit" class="edit-comment" meta:id="<%= comment.CommentID %>">Edit</a>&nbsp;|&nbsp;<a href="#remove" class="remove-comment" meta:id="<%= comment.CommentID %>">Remove</a></div>
<% } %>

<% Html.RenderPartial("~/Views/Shared/Pager.ascx", ViewData.Model); %>
</div>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Article/AdminSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-comments.js"></script>
</asp:Content>
