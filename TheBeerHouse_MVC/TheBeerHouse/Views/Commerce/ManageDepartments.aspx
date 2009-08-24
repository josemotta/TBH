<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TheBeerHouse.Models.Department>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<div id="departments">
<% foreach (Department department in ViewData.Model) { %>
	<div class="admin" id="admin-<%= department.DepartmentID %>">
		<%= Html.ActionLink("Edit", "EditDepartment", new { departmentID = department.DepartmentID })%>&nbsp;|&nbsp;
		<a href="#" class="delete-department-button" meta:id="<%= department.DepartmentID %>">Remove</a>
	</div>
	<% Html.RenderPartial("~/Views/Shared/Commerce/DepartmentItem.ascx", department); %>
	<div id="spacer-<%= department.DepartmentID %>"><hr /></div>
<% } %>
</div>
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/commerece.js"></script>
</asp:Content>
