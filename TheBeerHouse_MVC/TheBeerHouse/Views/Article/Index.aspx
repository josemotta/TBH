<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Pagination<Article>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div id="articles">

<% foreach(Article article in ViewData.Model) { %>
	<% Html.RenderPartial("~/Views/Shared/Article/ArticleItem.ascx", article); %>
	<hr />
<% } %>
</div>

<% Html.RenderPartial("~/Views/Shared/Pager.ascx", ViewData.Model); %>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
		<div id="news" class="boxed">
			<h2 class="title">Categories</h2>
			<div class="content">
				<ul>
					<li class="first"><a href="/">All</a></li>
	<% foreach(var category in ViewData["Categories"] as IEnumerable<Category>) { %>
					<li><%= Html.ActionLink(category.Title, "Index", new { controller = "Article", category = category.Path })%></li>
	<% } %>
				</ul>
			</div>
		</div>
</asp:Content>