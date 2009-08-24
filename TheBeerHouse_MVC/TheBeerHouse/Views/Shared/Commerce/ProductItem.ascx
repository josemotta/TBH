<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TheBeerHouse.Models.Product>" %>

<div id="department-<%= ViewData.Model.ProductID %>" class="department">
	<img src="<%= ViewData.Model.SmallImageUrl %>" title="<%= Html.Encode(Model.Title) %>" alt="<%= Html.Encode(Model.Title) %>" class="main-image" />
	<h3><a href="<%= Url.Action("ViewProduct", new { ProductID = ViewData.Model.ProductID}) %>"><%= Html.Encode(Model.Title) %></a></h3>
	<p><%= Html.Encode(Model.Description) %></p>
	<p>Regular Price: <b><%= ViewData.Model.UnitPrice.ToString("C") %></b> | 
	Sale Price: <b><font color="red"><%= ((ViewData.Model.UnitPrice * (100 - ViewData.Model.DiscountPercentage)) / 100 ).ToString("C") %></font></b> | 
	Units Available: <b><%= ViewData.Model.UnitsInStock %></b></p>
    <form method="post" action="<%= Url.Action("AddShoppingCartItem",new { ProductID = ViewData.Model.ProductID, Price = (ViewData.Model.UnitPrice * (100 - ViewData.Model.DiscountPercentage)) / 100, Title = ViewData.Model.Title, SKU = ViewData.Model.SKU}) %>" class="add-cart">
	<p><button type="submit" id="add-item-button">Add to Cart</button></p>
    </form> 
</div>