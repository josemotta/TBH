<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="commerce-admin" class="boxed">
	<h2 class="title">Store</h2>
	<div class="content">
		<ul>
			<li class="first"><%= Html.ActionLink("View Departments", "ManageDepartments") %></li>
			<li><%= Html.ActionLink("Create Department", "CreateDepartment") %></li>
			<li><%= Html.ActionLink("Manage Products", "ManageProducts") %></li>
			<li><%= Html.ActionLink("Create Product", "CreateProduct") %></li>
			<li><%= Html.ActionLink("Shipping Options", "ManageShipping") %></li>
			<li><%= Html.ActionLink("Manage Orders", "ManageOrders") %></li>
		</ul>
	</div>
</div>