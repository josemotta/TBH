<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TheBeerHouse.Models.Product>" %>

<div id="product-<%= ViewData.Model.ProductID %>" class="product">
	<img src="<%= ViewData.Model.SmallImageUrl %>" title="<%= Html.Encode(Model.Title) %>>" alt="<%= Html.Encode(Model.Title) %>" class="main-image" />
	<h3><a href="<%= Url.Action("CreateProduct", new { ProductID = ViewData.Model.ProductID}) %>"><%= Html.Encode(Model.Title) %></a></h3>
	<p><%= Html.Encode(Model.Description) %></p>
	<p>Regular Price: <b><%= ViewData.Model.UnitPrice.ToString() %></b> | 
	Sale Price: <b><font color="red"><%= ((ViewData.Model.UnitPrice * (100 - ViewData.Model.DiscountPercentage)) / 100 ).ToString("C") %></font></b> | 
	Units Available: <b><%= ViewData.Model.UnitsInStock %></b></p>
</div>