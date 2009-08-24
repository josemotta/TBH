<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="commerce">
        <h2><%= Html.ActionLink("Manage Departments", "ManageDepartments") %></h2>
        <p>This section will allow you to add "Departments" to your store. A department is like a category such as Mugs, Brews, and Accessories</p>
	</div>
	<hr />
	
	<div class="commerce">
        <h2><%= Html.ActionLink("Manage Products", "ManageProducts") %></h2>
        <p>This section will allow you to add Products to your store.</p>
	</div>
	<hr />
	
	<div class="commerce">
        <h2><%= Html.ActionLink("Manage Shipping Options", "ManageShipping") %></h2>
        <p>You can configure different shipping options and pricing here.</p>
	</div>
	<hr />
	
	<div class="commerce">
        <h2><%= Html.ActionLink("Manage Orders", "ManageOrders") %></h2>
        <p>View currently active orders placed in the store and update their statuses here.</p>
	</div>
	<hr />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Commerce/CommerceSidebar.ascx"); %>
</asp:Content>

