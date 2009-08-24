<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Department>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<div id="departments">
<% foreach (Department department in ViewData.Model) { %>
	<% Html.RenderPartial("~/Views/Shared/Commerce/DepartmentItem.ascx", department); %>
	<hr />
<% } %>
</div>
</asp:Content>