<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Pagination<TheBeerHouse.Models.Article>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="articles">
<% foreach(Article article in ViewData.Model) { %>
	<div class="admin"><%= Html.ActionLink("Edit", "EditArticle", new { controller = "Article", articleId = article.ArticleID })%>&nbsp;|&nbsp;<%= Html.ActionLink("Remove", "RemoveArticle", new { controller = "Article", articleId = article.ArticleID })%></div>
	<% Html.RenderPartial("~/Views/Shared/Article/ArticleItem.ascx", article); %>
	<hr />
<% } %>

<% Html.RenderPartial("~/Views/Shared/Pager.ascx", ViewData.Model); %>
</div>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Article/AdminSidebar.ascx"); %>
</asp:Content>
