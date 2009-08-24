<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Category>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div id="categories">
<% foreach (Category category in ViewData.Model) { %>
	<% Html.RenderPartial("~/Views/Shared/Article/CategoryItem.ascx", category); %>
	<hr />
<% } %>
</div>
</asp:Content>