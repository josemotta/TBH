<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TheBeerHouse.Models.Department>" %>

<div id="department-<%= ViewData.Model.DepartmentID %>" class="department">
	<img src="<%= ViewData.Model.ImageUrl %>" title="<%= Html.Encode(Model.Title) %>" alt="<%= Html.Encode(Model.Title) %>" class="main-image" />
	<h3><a href="<%= Url.Action("ViewDepartment", new { DepartmentID = ViewData.Model.DepartmentID}) %>"><%= Html.Encode(Model.Title) %></a></h3>
	<p><%= Html.Encode(Model.Description) %></p>
</div>